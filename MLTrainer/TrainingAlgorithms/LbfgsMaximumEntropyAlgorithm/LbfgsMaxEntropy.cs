using Microsoft.ML;
using Microsoft.ML.Trainers;
using MLTrainer.TrainingAlgorithms.CustomisableOption;
using System.Collections.Generic;
using System.Linq;

namespace MLTrainer.TrainingAlgorithms.LbfgsMaximumEntropyAlgorithm
{
    internal class LbfgsMaxEntropy : IMLTrainingAlgorithm
    {
        private HistorySizeOption historySizeOption;
        private NumberOfThreadsOption numberOfThreadsOption;
        private L1RegularisationOption l1RegularisationOption;
        private L2RegularisationOption l2RegularisationOption;
        private OptimisationTolOption optimisationTolOption;
        private EnforceNonNegativityOption enforceNonNegOption;

        internal LbfgsMaxEntropy()
        {
            historySizeOption = new HistorySizeOption(50);
            numberOfThreadsOption = new NumberOfThreadsOption(1);
            l1RegularisationOption = new L1RegularisationOption(1F);
            l2RegularisationOption = new L2RegularisationOption(1F);
            optimisationTolOption = new OptimisationTolOption(1E-07F);
            enforceNonNegOption = new EnforceNonNegativityOption(false);
        }

        public IEnumerable<ITrainingAlgorithmOption> GetCustomisableOptions()
        {
            yield return historySizeOption;
            yield return numberOfThreadsOption;
            yield return l1RegularisationOption;
            yield return l2RegularisationOption;
            yield return optimisationTolOption;
            yield return enforceNonNegOption;
        }

        /// <inheritdoc />
        public IEstimator<ITransformer> BuildTrainingAlgorithmPipeline(MLContext mlContext, 
            IEnumerable<ColumnNameStorageAttribute> inputDataColumnAttributes,
            IEnumerable<ColumnNameStorageAttribute> outputDataColumnAttributes)
        {
            string labelledInputColumnName = inputDataColumnAttributes.SingleOrDefault(att => att.IsLabel)?.Name;

            MLTrainingPipelineBuilder trainingBuilder = new MLTrainingPipelineBuilder(mlContext, inputDataColumnAttributes, outputDataColumnAttributes);
            string features = MLTrainingPipelineBuilder.FeaturesString;

            LbfgsMaximumEntropyMulticlassTrainer.Options options = new LbfgsMaximumEntropyMulticlassTrainer.Options
            {
                HistorySize = historySizeOption.Value,
                NumberOfThreads = historySizeOption.Value,
                L1Regularization = l1RegularisationOption.Value,
                L2Regularization = l2RegularisationOption.Value,
                OptimizationTolerance = optimisationTolOption.Value,
                EnforceNonNegativity = enforceNonNegOption.Value,
                LabelColumnName = labelledInputColumnName,
                FeatureColumnName = features
            };

            trainingBuilder.SetupOneHotEncodingForStrings();
            trainingBuilder.SetupMissingValuesReplacementForFloats();
            trainingBuilder.SetupFeaturesConcatenation();
            trainingBuilder.SetupMappingValueToKey();
            trainingBuilder.SetupFeatureMinMaxNormalisation();


            trainingBuilder.SetupTrainingStrategy(
                mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(options));

            trainingBuilder.SetupMappingKeyToValue();

            return trainingBuilder.TryCreatePipeline(out IEstimator<ITransformer> pipeline, out string errorMessage) ? pipeline : null;
        }
    }
}
