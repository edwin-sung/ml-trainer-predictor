using Microsoft.ML;
using Microsoft.ML.Trainers;
using MLTrainer.TrainingAlgorithms.CustomisableOption;
using System.Collections.Generic;
using System.Linq;

namespace MLTrainer.TrainingAlgorithms.LbfgsPoissonAlgorithm
{
    internal class LbfgsPoisson : IMLTrainingAlgorithm
    {
        private L1RegularisationOption l1RegularisationOption;
        private L2RegularisationOption l2RegularisationOption;

        internal LbfgsPoisson()
        {
            l1RegularisationOption = new L1RegularisationOption(2F);
            l2RegularisationOption = new L2RegularisationOption(0.5F);
        }

        /// <inheritdoc />
        public IEstimator<ITransformer> BuildTrainingAlgorithmPipeline(MLContext mlContext, IEnumerable<ColumnNameStorageAttribute> inputDataColumnAttributes, IEnumerable<ColumnNameStorageAttribute> outputDataColumnAttributes)
        {
            string labelledInputColumnName = inputDataColumnAttributes.SingleOrDefault(att => att.IsLabel)?.Name;
            MLTrainingPipelineBuilder trainingBuilder = new MLTrainingPipelineBuilder(mlContext, inputDataColumnAttributes, outputDataColumnAttributes);

            LbfgsPoissonRegressionTrainer.Options options = new LbfgsPoissonRegressionTrainer.Options
            {
                L1Regularization = l1RegularisationOption.Value,
                L2Regularization = l2RegularisationOption.Value,
                LabelColumnName = labelledInputColumnName,
                FeatureColumnName = MLTrainingPipelineBuilder.FeaturesString
            };

            trainingBuilder.SetupOneHotEncodingForStrings();
            trainingBuilder.SetupMissingValuesReplacementForFloats();
            trainingBuilder.SetupFeaturesConcatenation();
            trainingBuilder.SetupTrainingStrategy(mlContext.Regression.Trainers.LbfgsPoissonRegression(options));

            return trainingBuilder.TryCreatePipeline(out IEstimator<ITransformer> pipeline, out string errorMessage) ? pipeline : null;
        }

        /// <inheritdoc />
        public IEnumerable<ITrainingAlgorithmOption> GetCustomisableOptions()
        {
            yield return l1RegularisationOption;
            yield return l2RegularisationOption;

        }
    }
}
