using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MLTrainer.TrainingAlgorithms
{
    /// <summary>
    /// Machine learning training pipeline builder, that allows mix-n-match of pipeline set-ups to create a build pipeline
    /// </summary>
    internal class MLTrainingPipelineBuilder
    {
        private readonly List<IEstimator<ITransformer>> wellOrderedSetups = new List<IEstimator<ITransformer>>();

        private readonly MLContext mlContext;
        private readonly IEnumerable<ColumnNameStorageAttribute> inputColumnAttributes = new List<ColumnNameStorageAttribute>();
        private readonly IEnumerable<ColumnNameStorageAttribute> outputColumnAttributes = new List<ColumnNameStorageAttribute>();
        internal const string FeaturesString = "Features";

        internal MLTrainingPipelineBuilder(MLContext mlContext, IEnumerable<ColumnNameStorageAttribute> inputColumnAttributes, IEnumerable<ColumnNameStorageAttribute> outputColumnAttributes)
        {
            this.mlContext = mlContext;
            this.inputColumnAttributes = inputColumnAttributes;
            this.outputColumnAttributes = outputColumnAttributes;
        }

        internal void SetupOneHotEncodingForStrings()
        {
            InputOutputColumnPair[] hotEncodingColumnPairs = inputColumnAttributes.Where(att => !att.IsLabel && att.ColumnType == typeof(string))
                .Select(att => new InputOutputColumnPair(@att.Name, @att.Name)).ToArray();

            if (hotEncodingColumnPairs.Length > 0)
            {
                wellOrderedSetups.Add(mlContext.Transforms.Categorical.OneHotEncoding(hotEncodingColumnPairs));
            }
        }

        internal void SetupMissingValuesReplacementForFloats()
        {
            InputOutputColumnPair[] hotEncodingColumnPairs = inputColumnAttributes.Where(att => !att.IsLabel && att.ColumnType == typeof(float))
                .Select(att => new InputOutputColumnPair(@att.Name, @att.Name)).ToArray();

            if (hotEncodingColumnPairs.Length > 0)
            {
                wellOrderedSetups.Add(mlContext.Transforms.ReplaceMissingValues(hotEncodingColumnPairs));
            }
        }

        internal void SetupFeaturesConcatenation()
        {
            wellOrderedSetups.Add(mlContext.Transforms.Concatenate(FeaturesString, inputColumnAttributes.Where(att => !att.IsLabel).Select(att => att.Name).ToArray()));
        }

        internal void SetupMappingValueToKey()
        {
            string labelledInput = inputColumnAttributes.SingleOrDefault(att => att.IsLabel)?.Name;
            if (string.IsNullOrEmpty(labelledInput))
            {
                return;
            }

            wellOrderedSetups.Add(mlContext.Transforms.Conversion.MapValueToKey(@labelledInput, @labelledInput));
        }

        internal void SetupFeatureMinMaxNormalisation()
        {
            wellOrderedSetups.Add(mlContext.Transforms.NormalizeMinMax(@FeaturesString, @FeaturesString));
        }

        internal void SetupTrainingStrategy(IEstimator<ITransformer> trainingStrategy)
        {
            wellOrderedSetups.Add(trainingStrategy);
        }

        internal void SetupMappingKeyToValue()
        {
            string labelledOutput = outputColumnAttributes.SingleOrDefault(att => att.IsLabel)?.Name;
            if (string.IsNullOrEmpty(labelledOutput))
            {
                return;
            }

            wellOrderedSetups.Add(mlContext.Transforms.Conversion.MapKeyToValue(new[] {new InputOutputColumnPair(@labelledOutput, @labelledOutput) }));
        }

        /// <summary>
        /// Creates a training pipeline based on the set-ups in place
        /// </summary>
        /// <param name="trainingPipeline">[Output] Training pipeline, if the set-up was successful</param>
        /// <param name="errorMessage">[Output] Error message in the event of an unhandled exception</param>
        /// <returns></returns>
        internal bool TryCreatePipeline(out IEstimator<ITransformer> trainingPipeline, out string errorMessage)
        {
            trainingPipeline = null;
            errorMessage = string.Empty;
            try
            {
                foreach (IEstimator<ITransformer> setup in wellOrderedSetups)
                {
                    trainingPipeline = trainingPipeline?.Append(setup) ?? setup;
                }
            } 
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            
            return trainingPipeline != null && errorMessage == string.Empty;

        }

    }
}
