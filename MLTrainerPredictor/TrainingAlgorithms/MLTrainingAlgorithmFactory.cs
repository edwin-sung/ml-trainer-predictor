using MLTrainerPredictor.TrainingAlgorithms.OneVersusAllAlgorithm;

namespace MLTrainerPredictor.TrainingAlgorithms
{
    internal static class MLTrainingAlgorithmFactory
    {

        internal static IMLTrainingAlgorithm? CreateInstance(MLTrainingAlgorithmType algorithmType)
        {
            switch(algorithmType)
            {
                case MLTrainingAlgorithmType.ONE_VERSUS_ALL: return new OneVersusAll();
                default: return null;
            }
        }
    }
}
