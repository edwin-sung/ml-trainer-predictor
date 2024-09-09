using MLTrainer.PredictionTesterUI;
using MLTrainer.TrainingAlgorithms;
using MLTrainer.TrainingAlgorithms.CustomisableOption;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MLTrainer.DataSetup
{
    /// <summary>
    /// Functionality-specific machine learning set-up item interface
    /// It facilitates communication between the form and the trainer/predictor
    /// </summary>
    public interface IFunctionalitySpecificMLSetupItem
    {
        /// <summary>
        /// Name to be shown on the form
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Directory to read/save the trained model in
        /// </summary>
        string TrainingModelDirectory { get; set; }

        /// <summary>
        /// Descriptive training model name to be appended after directory for
        /// both CSV and trained ZIP files to be saved in.
        /// </summary>
        string TrainingModelName { get; set; }

        /// <summary>
        /// Data extension for this particular set-up (.csv, .json etc.)
        /// </summary>
        string DataExtension { get; }


        /// <summary>
        /// Opens up a data schema set up form, passing the action to call upon closure of the form
        /// </summary>
        /// <param name="schemaSetupFormClosureAction">Schema set-up form closure action</param>
        void OpenDataSchemaSetupForm(FormClosedEventHandler schemaSetupFormClosureAction);

        /// <summary>
        /// Opens up an auto-select training algorithm form, passing the action to call upon closure of the form
        /// </summary>
        /// <param name="formClosureAction">Auto-select training algorithm closure action</param>
        void OpenAutoSelectTrainingAlgorithmForm(FormClosedEventHandler formClosureAction);

        /// <summary>
        /// Gets all available training algorithms for end-user to select
        /// </summary>
        /// <returns>All available training algorithms to choose from </returns>
        IEnumerable<MLTrainingAlgorithmType> GetAllTrainingAlgorithms();

        /// <summary>
        /// Sets the training algorithm
        /// </summary>
        /// <param name="algorithmType">Training algroithm type</param>
        void SetTrainingAlgorithm(MLTrainingAlgorithmType algorithmType);

        /// <summary>
        /// Gets the collection of training algorithm options that are customisable
        /// This is useful for forms to output customisable fields for user input
        /// </summary>
        /// <returns>Collection of training algorithm options</returns>
        List<ITrainingAlgorithmOption> GetTrainingAlgorithmOptions();

        /// <summary>
        /// Add data inputs by importing a source file path, with the data extension mentioned above
        /// </summary>
        /// <param name="srcFilePath">Source file path</param>
        void AddDataInputsBySourceFilePath(string srcFilePath);

        /// <summary>
        /// Gets all data input columns
        /// </summary>
        /// <returns>All data input columns</returns>
        List<string> GetAllDataInputColumns();

        /// <summary>
        /// Gets all data inputs as strings
        /// </summary>
        /// <returns>All data inputs, as strings</returns>
        List<List<string>> GetAllDataInputsAsStrings();

        /// <summary>
        /// Remove a data input by index
        /// </summary>
        /// <param name="index">index</param>
        void RemoveDataInputByIndex(int index);

        /// <summary>
        /// Clears all data input
        /// </summary>
        void ClearAllDataInput();

        /// <summary>
        /// Saves model inputs as data file to be referenced in the future, matching the extension stated above
        /// </summary>
        void SaveModelInputAsDataExtension();

        /// <summary>
        /// Create a temporary trained model based on the set of model inputs added
        /// to this instance
        /// </summary>
        /// <param name="dataSplitTestPercentage">Data split test percentage</param>
        /// <param name="rSquared">[Output] R-squared value</param>
        /// <param name="seed">Seed value for the training algorithm to use</param>
        /// <param name="testingTrainedModelFilePath">[Output] Testing trained model file path</param>
        /// <returns>True if the training is successful</returns>
        bool TryCreateTrainedModelForTesting(out string testingTrainedModelFilePath, out double? rSquared, double dataSplitTestPercentage = 0.2, int? seed = null);

        /// <summary>
        /// Gets all test prediction data input items for the form
        /// </summary>
        /// <returns>Collection of prediction tester data input items</returns>
        IEnumerable<IPredictionTesterDataInputItem> GetAllPredictionTesterDataInputItems();

        /// <summary>
        /// Runs the test prediction
        /// </summary>
        /// <param name="predictedValueAsString">[Output] Predicted value as string</param>
        void RunTestPrediction(out string predictedValueAsString);

        /// <summary>
        /// Apply / commit the trained model
        /// </summary>
        /// <param name="trainedModelFilePath">[Output] Trained model file path</param>"
        /// <returns>True if the trained model is applied, and temporary files are deleted</returns>
        bool ApplyTrainedModel(out string trainedModelFilePath);

        /// <summary>
        /// Cancel action that reset the file contents
        /// </summary>
        void CleanupTemporaryFiles();

    }
}
