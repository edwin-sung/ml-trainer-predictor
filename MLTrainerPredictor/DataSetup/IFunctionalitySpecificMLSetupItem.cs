namespace MLTrainerPredictor.DataSetup
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
        /// Add data inputs by importing a CSV file path
        /// </summary>
        /// <param name="csvFilePath">CSV file path</param>
        void AddDataInputsByCSVFilePath(string csvFilePath);

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
        /// Saves model inputs as CSV file to be refereced in the future
        /// </summary>
        void SaveModelInputAsCSV();

        /// <summary>
        /// Create a temporary trained model based on the set of model inputs added
        /// to this instance
        /// </summary>
        /// <returns>True if the training is successful</returns>
        bool TryCreateTrainedModelForTesting();

        /// <summary>
        /// Apply / commit the trained model
        /// </summary>
        /// <returns>True if the trained model is applied, and temporary files are deleted</returns>
        bool ApplyTrainedModel();

        /// <summary>
        /// Cancel action that reset the file contents
        /// </summary>
        void CleanupTemporaryFiles();

    }
}
