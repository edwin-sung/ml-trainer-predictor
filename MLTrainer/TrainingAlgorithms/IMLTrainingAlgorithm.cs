using Microsoft.ML;
using MLTrainer.TrainingAlgorithms.CustomisableOption;
using System;
using System.Collections.Generic;

namespace MLTrainer.TrainingAlgorithms
{
    public interface IMLTrainingAlgorithm
    {

        /// <summary>
        /// Descriptive name of the algorithm
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Different training algorithms have different IDataView-specific keywords for the predicted value
        /// e.g. PredictedLabel, Score etc.
        /// </summary>
        string PredictedValueColumnKeyword { get; }

        /// <summary>
        /// Checks whether or not the algorithm accepts the predicted value of the given type
        /// </summary>
        /// <param name="predictedValueType">Predicted value type</param>
        /// <returns>True if the given predicted value type is valid for this algorithm</returns>
        bool IsValidPredictedValueColumnType(Type predictedValueType);

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
