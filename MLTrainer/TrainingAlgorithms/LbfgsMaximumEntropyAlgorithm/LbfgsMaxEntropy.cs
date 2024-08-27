using Microsoft.ML;
using Microsoft.ML.Trainers;
using MLTrainer.TrainingAlgorithms.CustomisableOption;
using System.Collections.Generic;

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

        public IEstimator<ITransformer> GetTrainingAlgorithm(MLContext mlContext, string labelledInputColumnName, string featuresName)
        {
            LbfgsMaximumEntropyMulticlassTrainer.Options options = new LbfgsMaximumEntropyMulticlassTrainer.Options
            {
                HistorySize = historySizeOption.Value,
                NumberOfThreads = historySizeOption.Value,
                L1Regularization = l1RegularisationOption.Value,
                L2Regularization = l2RegularisationOption.Value,
                OptimizationTolerance = optimisationTolOption.Value,
                EnforceNonNegativity = enforceNonNegOption.Value,
                LabelColumnName = labelledInputColumnName,
                FeatureColumnName = featuresName
            };

            return mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(options);
        }
    }
}
