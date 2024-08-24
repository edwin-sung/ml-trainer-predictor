using Microsoft.ML;
using MLTrainer.TrainingAlgorithms.CustomisableOption;
using System.Collections.Generic;

namespace MLTrainer.TrainingAlgorithms
{
    public interface IMLTrainingAlgorithm
    {

        IEnumerable<ITrainingAlgorithmOption> GetCustomisableOptions();

        /// <summary>
        /// Gets the training algorithm with a given Machine Learning context instance etc.
        /// </summary>
        /// <param name="mlContext">Machine learning context instance</param>
        /// <param name="labelledInputColumnName">Input column name as label</param>
        /// <param name="featuresName">Name that consists of features</param>
        /// <returns>Training algorithm</returns>
        IEstimator<ITransformer> GetTrainingAlgorithm(MLContext mlContext, string labelledInputColumnName, string featuresName);

    }
}
