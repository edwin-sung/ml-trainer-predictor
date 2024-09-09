using MLTrainer.TrainingAlgorithms.LbfgsMaximumEntropyAlgorithm;
using MLTrainer.TrainingAlgorithms.LbfgsPoissonAlgorithm;
using MLTrainer.TrainingAlgorithms.OneVersusAllAlgorithm;
using MLTrainer.TrainingAlgorithms.OnlineGradientDescentAlgorithm;
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
                case MLTrainingAlgorithmType.ONLINE_GRADIENT_DESCENT: return new OnlineGradientDescent();
                case MLTrainingAlgorithmType.LBFGS_POISSON_REGRESSION: return new LbfgsPoisson();
                default: return null;
            }
        }
    }
}
