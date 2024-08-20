using Microsoft.ML;
using MLTrainerPredictor.TrainingAlgorithms.CustomisableOption;

namespace MLTrainerPredictor.TrainingAlgorithms.OneVersusAllAlgorithm
{
    internal class OneVersusAll : IMLTrainingAlgorithm
    {
        private L1RegularisationOption l1RegularisationOption;
        private L2RegularisationOption l2RegularisationOption;

        internal OneVersusAll()
        {
            l1RegularisationOption = new L1RegularisationOption(1F);
            l2RegularisationOption = new L2RegularisationOption(1F);
        }

        public IEnumerable<ITrainingAlgorithmOption> GetCustomisableOptions()
        {
            yield return l1RegularisationOption;
            yield return l2RegularisationOption;
        }

        public IEstimator<ITransformer> GetTrainingAlgorithm(MLContext mlContext, string labelledInputColumnName, string featuresName)
        {
            return mlContext.MulticlassClassification.Trainers.OneVersusAll(binaryEstimator: mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(l1Regularization: l1RegularisationOption.Value, l2Regularization: l2RegularisationOption.Value, labelColumnName: labelledInputColumnName, featureColumnName: featuresName), labelledInputColumnName);
        }
    }
}
