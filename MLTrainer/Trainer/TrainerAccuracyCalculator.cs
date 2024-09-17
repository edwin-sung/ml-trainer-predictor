using Microsoft.ML;
using MLTrainer.TrainingAlgorithms;

namespace MLTrainer.Trainer
{
    /// <summary>
    /// Trainer accuracy calculator which splits a train-test data set to be trained, predicted and its accuracy calculated
    /// </summary>
    public abstract class TrainerAccuracyCalculator
    {
        protected MLContext mlContext;

        protected TrainerAccuracyCalculator(MLContext mlContext)
        {
            this.mlContext = mlContext;
        }

        internal abstract double? GetAccuracy();

    }
}
