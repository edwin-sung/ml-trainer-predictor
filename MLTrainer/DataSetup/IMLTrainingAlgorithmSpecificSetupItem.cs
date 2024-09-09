using MLTrainer.TrainingAlgorithms;

namespace MLTrainer.DataSetup
{
    /// <summary>
    /// Instance for ML-training algorithm specific set-up item, which also gives the output class.
    /// </summary>
    internal interface IMLTrainingAlgorithmSpecificSetupItem
    {

        /// <summary>
        /// 
        /// </summary>
        IMLTrainingAlgorithm TrainingAlgorithm { get; }

    }
}
