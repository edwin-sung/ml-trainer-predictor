using MLTrainer.TrainingAlgorithms.LbfgsMaximumEntropyAlgorithm;
using MLTrainer.TrainingAlgorithms.OneVersusAllAlgorithm;

namespace MLTrainer.TrainingAlgorithms
{
    internal static class MLTrainingAlgorithmFactory
    {

        internal static IMLTrainingAlgorithm CreateInstance(MLTrainingAlgorithmType algorithmType)
        {
            switch(algorithmType)
            {
                case MLTrainingAlgorithmType.ONE_VERSUS_ALL: return new OneVersusAll();
                case MLTrainingAlgorithmType.LBFGS_MAX_ENTROPY: return new LbfgsMaxEntropy();
                default: return null;
            }
        }
    }
}
