using MLTrainer.TrainingAlgorithms;
using System.Collections.Generic;
using System.Reflection;
using System;
using Microsoft.ML;
using System.Linq;
using MLTrainer.Trainer;

namespace MLTrainer.CompileTimeTrainingSetup.ConcreteObjectTrainer
{
    /// <summary>
    /// ML.NET trainer for concrete objects, with model input and model output
    /// </summary>
    /// <typeparam name="ModelInput">Model input type</typeparam>
    /// <typeparam name="ModelOutput">Model output type</typeparam>
    internal class ConcreteObjectModelTrainer<ModelInput, ModelOutput> : ModelTrainer
        where ModelInput : class
        where ModelOutput : class, new()
    {

        internal ConcreteObjectModelTrainer(IMLTrainingAlgorithm trainingAlgorithm) : base(trainingAlgorithm)
        {
        }

        /// <summary>
        /// Gets all column names for a given generic type, based on the predicate for label
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="attributePredicate">Predicate on whether one wants the labels, non-labels, or both</param>
        /// <param name="columnNames">[Output] Column names</param>
        /// <returns>True if any items are found</returns>
        private bool TryGetColumnNamesFor<T>(Predicate<ColumnNameStorageAttribute> attributePredicate, out List<string> columnNames)
        {
            columnNames = new List<string>();
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                try
                {
                    ColumnNameStorageAttribute att = property.GetCustomAttribute<ColumnNameStorageAttribute>();
                    if (!string.IsNullOrEmpty(att.Name) && attributePredicate(att))
                    {
                        columnNames.Add(att.Name);
                    }
                }
                catch
                {
                    continue;
                }
            }

            return columnNames.Any();
        }

        /// <summary>
        /// Train the model
        /// </summary>
        internal bool TryTrainModel(IEnumerable<ModelInput> inputs, string trainedModelFilePath)
        {
            MLContext mlContextInstance = new MLContext();
            IDataView trainData = mlContextInstance.Data.LoadFromEnumerable(inputs);

            return TryTrainModel(mlContextInstance, BuildPipeline(mlContextInstance), trainData, trainedModelFilePath);
        }

        /// <inheritdoc/>
        private IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Make sure we have one or more non-label inputs, only one label input, and only one label output
            if (!TryGetColumnNamesFor<ModelInput>(att => !att.IsLabel, out List<string> nonLabelInputs) ||
                !TryGetColumnNamesFor<ModelInput>(att => att.IsLabel, out List<string> labelledInputs) ||
                !(labelledInputs.SingleOrDefault() is string labelledInput) ||
                !TryGetColumnNamesFor<ModelOutput>(att => att.IsLabel, out List<string> labelledOutputs) ||
                !(labelledOutputs.SingleOrDefault() is string labelledOutput))
            {
                return null;
            }

            string[] features = nonLabelInputs.Select(c => @c).ToArray();
            List<IEstimator<ITransformer>> estimatorChainActions = new List<IEstimator<ITransformer>>();

            List<InputOutputColumnPair> hotEncodingColumnPairs = new List<InputOutputColumnPair>();
            if (TryGetColumnNamesFor<ModelInput>(att => att.ColumnType == typeof(string) && !att.IsLabel, out List<string> stringColumns))
            {
                stringColumns.ForEach(c => hotEncodingColumnPairs.Add(new InputOutputColumnPair(@c, @c)));
                estimatorChainActions.Add(mlContext.Transforms.Categorical.OneHotEncoding(hotEncodingColumnPairs.ToArray()));
            }

            List<InputOutputColumnPair> missingValuesColumnPairs = new List<InputOutputColumnPair>();
            if (TryGetColumnNamesFor<ModelInput>(att => att.ColumnType == typeof(float) && !att.IsLabel, out List<string> floatColumns))
            {
                floatColumns.ForEach(c => missingValuesColumnPairs.Add(new InputOutputColumnPair(@c, @c)));

                estimatorChainActions.Add(mlContext.Transforms.ReplaceMissingValues(missingValuesColumnPairs.ToArray()));
            }

            estimatorChainActions.Add(mlContext.Transforms.Concatenate(@"Features", features));
            estimatorChainActions.Add(mlContext.Transforms.Conversion.MapValueToKey(@labelledInput, @labelledInput));
            estimatorChainActions.Add(mlContext.Transforms.NormalizeMinMax(@"Features", @"Features"));
            estimatorChainActions.Add(trainingAlgorithm.GetTrainingAlgorithm(mlContext, labelledInput, @"Features"));
            estimatorChainActions.Add(mlContext.Transforms.Conversion.MapKeyToValue(@labelledOutput, @labelledOutput));
            return CreateEstimatorChain(estimatorChainActions);
        }

    }
}
