using Microsoft.ML;
using MLTrainer.TrainingAlgorithms.CustomisableOption;
using Microsoft.ML.Trainers.LightGbm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MLTrainer.TrainingAlgorithms.LightGbmAlgorithm
{
    internal class LightGbm : IMLTrainingAlgorithm
    {
        public string Name => "Light GBM";

        public string PredictedValueColumnKeyword => "PredictedLabel";

        public bool IsValidPredictedValueColumnType(Type predictedValueType)
        {
            return predictedValueType == typeof(string);
        }

        public IEnumerable<ITrainingAlgorithmOption> GetCustomisableOptions()
        {
            yield break;
        }

        public IEstimator<ITransformer> BuildTrainingAlgorithmPipeline(MLContext mlContext, IEnumerable<ColumnNameStorageAttribute> inputDataColumnAttributes, IEnumerable<ColumnNameStorageAttribute> outputDataColumnAttributes)
        {
            string labelledInputColumnName = inputDataColumnAttributes.SingleOrDefault(att => att.IsLabel)?.Name;

            MLTrainingPipelineBuilder trainingBuilder = new MLTrainingPipelineBuilder(mlContext, inputDataColumnAttributes, outputDataColumnAttributes);
            string features = MLTrainingPipelineBuilder.FeaturesString;

            LightGbmMulticlassTrainer.Options options = new LightGbmMulticlassTrainer.Options
            {
                NumberOfLeaves = 800,
                NumberOfIterations = 1874,
                MinimumExampleCountPerLeaf = 20,
                LearningRate = 0.5,
                LabelColumnName = labelledInputColumnName,
                FeatureColumnName = features
            };

            trainingBuilder.SetupOneHotEncodingForStrings();
            trainingBuilder.SetupMissingValuesReplacementForFloats();
            trainingBuilder.SetupFeaturesConcatenation();
            trainingBuilder.SetupMappingValueToKey();
            trainingBuilder.SetupFeatureMinMaxNormalisation();


            trainingBuilder.SetupTrainingStrategy(
                mlContext.MulticlassClassification.Trainers.LightGbm(options));

            trainingBuilder.SetupMappingKeyToValue();

            return trainingBuilder.TryCreatePipeline(out IEstimator<ITransformer> pipeline, out string errorMessage) ? pipeline : null;
        }
    }
}
