using Microsoft.ML;
using Microsoft.ML.Trainers;
using MLTrainer.TrainingAlgorithms.CustomisableOption;
using System.Collections.Generic;

namespace MLTrainer.TrainingAlgorithms.OneVersusAllAlgorithm
{
    internal class OneVersusAll : IMLTrainingAlgorithm
    {
        private HistorySizeOption historySizeOption;
        private L1RegularisationOption l1RegularisationOption;
        private L2RegularisationOption l2RegularisationOption;

        internal OneVersusAll()
        {
            historySizeOption = new HistorySizeOption(50);
            l1RegularisationOption = new L1RegularisationOption(1F);
            l2RegularisationOption = new L2RegularisationOption(1F);
        }

        public IEnumerable<ITrainingAlgorithmOption> GetCustomisableOptions()
        {
            yield return historySizeOption;
            yield return l1RegularisationOption;
            yield return l2RegularisationOption;
        }

        public IEstimator<ITransformer> GetTrainingAlgorithm(MLContext mlContext, string labelledInputColumnName, string featuresName)
        {
            LbfgsLogisticRegressionBinaryTrainer.Options options = new LbfgsLogisticRegressionBinaryTrainer.Options
            {
                HistorySize = historySizeOption.Value,
                L1Regularization = l1RegularisationOption.Value,
                L2Regularization = l2RegularisationOption.Value,
                LabelColumnName = labelledInputColumnName,
                FeatureColumnName = featuresName,
                
            };
            return mlContext.MulticlassClassification.Trainers.OneVersusAll(binaryEstimator: mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(options), labelledInputColumnName);
        }
    }
}
