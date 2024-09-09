using Microsoft.ML;
using Microsoft.ML.Trainers;
using MLTrainer.TrainingAlgorithms.CustomisableOption;
using MLTrainer.TrainingAlgorithms.LbfgsMaximumEntropyAlgorithm;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MLTrainer.TrainingAlgorithms.OnlineGradientDescentAlgorithm
{
    internal class OnlineGradientDescent : IMLTrainingAlgorithm
    {
        private L2RegularisationOption l2RegularisationOption;
        private DecreaseLearningRateToggle decreaseLearningRateToggle;
        private LearningRateOption learningRateOption;
        private NumberOfIterationsOption numberOfIterationsOption;

        internal OnlineGradientDescent()
        {
            decreaseLearningRateToggle = new DecreaseLearningRateToggle(true);
            learningRateOption = new LearningRateOption(0.1F);
            numberOfIterationsOption = new NumberOfIterationsOption(1);
            l2RegularisationOption = new L2RegularisationOption(0F);
        }

        public IEnumerable<ITrainingAlgorithmOption> GetCustomisableOptions()
        {
            yield return decreaseLearningRateToggle;
            yield return learningRateOption;
            yield return numberOfIterationsOption;
            yield return l2RegularisationOption;
        }

        public IEstimator<ITransformer> GetTrainingAlgorithm(MLContext mlContext, string labelledInputColumnName, string featuresName)
        {
            OnlineGradientDescentTrainer.Options options = new OnlineGradientDescentTrainer.Options
            {
                L2Regularization = l2RegularisationOption.Value,
                LabelColumnName = labelledInputColumnName,
                FeatureColumnName = featuresName,
                DecreaseLearningRate = decreaseLearningRateToggle.Value,
                LearningRate = learningRateOption.Value,
                NumberOfIterations = numberOfIterationsOption.Value
            };

            return mlContext.Regression.Trainers.OnlineGradientDescent(options);
        }

        /// <inheritdoc />
        public IEstimator<ITransformer> BuildTrainingAlgorithmPipeline(MLContext mlContext, 
                IEnumerable<ColumnNameStorageAttribute> inputDataColumnAttributes, 
                IEnumerable<ColumnNameStorageAttribute> outputDataColumnAttributes)
        {
            string labelledInputColumnName = inputDataColumnAttributes.SingleOrDefault(att => att.IsLabel)?.Name;

            MLTrainingPipelineBuilder trainingBuilder = new MLTrainingPipelineBuilder(mlContext, inputDataColumnAttributes, outputDataColumnAttributes);
            string features = MLTrainingPipelineBuilder.FeaturesString;

            OnlineGradientDescentTrainer.Options options = new OnlineGradientDescentTrainer.Options
            {
                L2Regularization = l2RegularisationOption.Value,
                
                LabelColumnName = @labelledInputColumnName,
                FeatureColumnName = @features,
                DecreaseLearningRate = decreaseLearningRateToggle.Value,
                LearningRate = learningRateOption.Value,
                NumberOfIterations = numberOfIterationsOption.Value
            };

            trainingBuilder.SetupOneHotEncodingForStrings();
            trainingBuilder.SetupMissingValuesReplacementForFloats();
            trainingBuilder.SetupFeaturesConcatenation();
            trainingBuilder.SetupFeatureMinMaxNormalisation();

            trainingBuilder.SetupTrainingStrategy(mlContext.Regression.Trainers.OnlineGradientDescent(options));

            return trainingBuilder.TryCreatePipeline(out IEstimator<ITransformer> pipeline, out string errorMessage) ? pipeline : null;
        }
    }
}
