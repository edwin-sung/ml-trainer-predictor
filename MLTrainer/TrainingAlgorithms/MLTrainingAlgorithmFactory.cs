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
                default: return null;
            }
        }
    }
}
