using System.Reflection;
using System.Text;

namespace MLTrainerPredictor.DataSetup
{

    /// <summary>
    /// Abstract class for functionality-specific machine learning set-up item
    /// </summary>
    /// <typeparam name="ModelInput">Model input generic type</typeparam>
    /// <typeparam name="ModelOutput">Model output generic type</typeparam>
    public abstract class FunctionalitySpecificMLSetupItem<ModelInput, ModelOutput> : IFunctionalitySpecificMLSetupItem
        where ModelInput : class 
        where ModelOutput: class, new()
    {
        /// <summary>
        /// Separator character
        /// </summary>
        protected const char SEPARATOR = ',';

        /// <inheritdoc />
        public abstract string Name { get; }

        /// <inheritdoc />
        public abstract string TrainingModelDirectory { get; set; }

        /// <inheritdoc />
        public abstract string TrainingModelName { get; set; }

        private string TrainingModelFilePath => TrainingModelDirectory + "/" + TrainingModelName + ".csv";

        /// <summary>
        /// Trained model file path
        /// </summary>
        protected string TrainedModelFilePath => TrainingModelDirectory + "/" + TrainingModelName + ".zip";

        // Temporary training model file path for ML testing area
        private string TempTrainedModelFilePath => TrainingModelDirectory + "/" + TrainingModelName + "-temp.zip";

        private string BackupTrainedModelFilePath => TrainingModelDirectory + "/" + TrainingModelName + $"-backup{DateTime.Now}.zip";


        private List<ModelInput> modelInputs = new List<ModelInput>();

        /// <summary>
        /// Constructor for the machine learning set-up item
        /// </summary>
        protected FunctionalitySpecificMLSetupItem(string functionalityName)
        {
            TrainingModelName = functionalityName;

            // If the CSV file already exists, import the CSV automatically.
            if (File.Exists(TrainingModelFilePath))
            {
                AddDataInputsByCSVFilePath(TrainingModelFilePath);
            }
        }

        /// <inheritdoc />
        public void AddDataInputsByCSVFilePath(string csvFilePath)
        {
            if (!File.Exists(csvFilePath))
            {
                return;
            }

            using (StreamReader sr = new StreamReader(csvFilePath))
            {
                string currentLine;
                while ((currentLine = sr.ReadLine()) != null)
                {
                    if (TryParse(currentLine, out ModelInput validModelInput))
                    {
                        modelInputs.Add(validModelInput);
                    }
                }
            }
        }

        /// <summary>
        /// Parses the CSV row as string, and outputs a valid ModelInput instance if successful
        /// </summary>
        /// <param name="csvRow">CSV row as string</param>
        /// <param name="validModelInput">[Output] ModelInput instance, if valid</param>
        /// <returns></returns>
        protected abstract bool TryParse(string csvRow, out ModelInput validModelInput);

        /// <summary>
        /// Converts the given model input to a CSV-writable and CSV-readable string
        /// </summary>
        /// <param name="input">ModelInput instance</param>
        /// <param name="csvRow">[Output] CSV row as string</param>
        /// <returns></returns>
        protected abstract bool TryConvertToCSVString(ModelInput input, out string csvRow);

        /// <inheritdoc />
        public List<string> GetAllDataInputColumns()
        {
            return TryGetColumnNamesFor<ModelInput>(label => true, out List<string> results) ? results : new List<string>();
        }

        /// <inheritdoc />
        public List<List<string>> GetAllDataInputsAsStrings()
        {
            List<List<string>> allInputs = new List<List<string>>();
            foreach (ModelInput singleInput in modelInputs)
            {
                List<string> fieldValues = new List<string>();

                // Using the Type.getProperties() method to find all the values 
                foreach (var property in typeof(ModelInput).GetProperties())
                {
                    try
                    {
                        if (property.GetCustomAttribute<ColumnNameStorageAttribute>() != null)
                        {
                            if (property.GetValue(singleInput) is string stringVal)
                            {
                                fieldValues.Add(stringVal);
                            }
                            else if (property.GetValue(singleInput) is float floatVal)
                            {
                                fieldValues.Add(floatVal.ToString());
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                allInputs.Add(fieldValues);
            }

            return allInputs;
        }

        /// <summary>
        /// Adds a new ModelInput instance to the collection
        /// </summary>
        /// <param name="newDataInput">New ModelInput instance</param>
        protected void Add(ModelInput newDataInput) => modelInputs.Add(newDataInput);

        /// <inheritdoc />
        public void RemoveDataInputByIndex(int index) => modelInputs.RemoveAt(index);

        /// <inheritdoc />
        public void ClearAllDataInput() => modelInputs.Clear();

        /// <inheritdoc />
        public void SaveModelInputAsCSV()
        {
            // Save the modelInputs to a CSV file
            StringBuilder csvRowBuilder = new StringBuilder();
            if (TryGetColumnNamesFor<ModelInput>(label => true, out List<string> columnNames))
            {
                csvRowBuilder.AppendLine(string.Join(SEPARATOR.ToString(), columnNames));
            }

            foreach (ModelInput item in modelInputs)
            {
                if (TryConvertToCSVString(item, out string csvLine))
                {
                    csvRowBuilder.AppendLine(csvLine);
                }
            }

            // Write CSV to a temp file
            File.WriteAllText(TrainingModelFilePath, csvRowBuilder.ToString());
        }

        /// <summary>
        /// Gets all column names for a given generic type, based on the predicate for label
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="labelPredicate">Predicate on whether one wants the labels, non-labels, or both</param>
        /// <param name="columnNames">[Output] Column names</param>
        /// <returns>True if any items are found</returns>
        private bool TryGetColumnNamesFor<T>(Predicate<bool> labelPredicate, out List<string> columnNames)
        {
            columnNames = new List<string>();
            foreach (var property in typeof(T).GetProperties())
            {
                try
                {
                    ColumnNameStorageAttribute att = property.GetCustomAttribute<ColumnNameStorageAttribute>();
                    if (!string.IsNullOrEmpty(att.Name) && labelPredicate(att.IsLabel))
                    {
                        columnNames.Add(att.Name);
                    }
                }
                catch
                {
                    continue;
                }
            }

            // If we are looking for label, make sure there is only one column, otherwise simply check whether there are any.
            return columnNames.Any();
        }

        /// <inheritdoc />
        public bool TryCreateTrainedModelForTesting()
        {
            // Create a copy of the original trained model ZIP and save it as a temp
            // This way we can change the state of the original one for testing.
            if (File.Exists(TrainedModelFilePath))
            {
                File.Copy(TrainedModelFilePath, TempTrainedModelFilePath);
            }

            ModelTrainer<ModelInput, ModelOutput> trainer = new ModelTrainer<ModelInput, ModelOutput>();
            return trainer.TryTrainModel(modelInputs, TrainedModelFilePath);
        }

        /// <inheritdoc />
        public bool ApplyTrainedModel()
        {
            try
            {
                // Replace the file path of the trained model with the temp one, and delete the temp.
                // Anything goes wrong, a backup is supplied
                if (File.Exists(TempTrainedModelFilePath))
                {
                    File.Delete(TempTrainedModelFilePath);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc />
        public void CleanupTemporaryFiles()
        {
            if (File.Exists(TempTrainedModelFilePath))
            {
                File.Replace(TrainedModelFilePath, TempTrainedModelFilePath, BackupTrainedModelFilePath);
                File.Delete(TempTrainedModelFilePath);
            }
            else if (File.Exists(TrainedModelFilePath))
            {
                File.Delete(TrainedModelFilePath);
            }
        }


    }
}
