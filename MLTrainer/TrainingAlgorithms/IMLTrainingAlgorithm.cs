using Microsoft.ML;
using MLTrainer.TrainingAlgorithms.CustomisableOption;
using System.Collections.Generic;

namespace MLTrainer.TrainingAlgorithms
{
    public interface IMLTrainingAlgorithm
    {

        /// <summary>
        /// Gets customisable options for users to edit
        /// </summary>
        /// <returns>Collection of customisable options</returns>
        IEnumerable<ITrainingAlgorithmOption> GetCustomisableOptions();

        /// <summary>
        /// Builds the training algorithm pipeline based on the input data and output data column name attributes
        /// </summary>
        /// <param name="mlContext">Machine learning context instance</param>
        /// <param name="inputDataColumnAttributes">Input data column name attributes</param>
        /// <param name="outputDataColumnAttributes">Output data column name attributes</param>
        /// <returns>Training algorithm pipeline</returns>
        IEstimator<ITransformer> BuildTrainingAlgorithmPipeline(MLContext mlContext, 
            IEnumerable<ColumnNameStorageAttribute> inputDataColumnAttributes, 
            IEnumerable<ColumnNameStorageAttribute> outputDataColumnAttributes);

    }
}
