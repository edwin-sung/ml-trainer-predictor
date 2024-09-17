using MLTrainer.Forms;
using MLTrainer.PredictionTesterUI;
using MLTrainer.Trainer;
using MLTrainer.TrainingAlgorithms;
using MLTrainer.TrainingAlgorithms.CustomisableOption;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MLTrainer.DataSetup
{

    /// <summary>
    /// Abstract class for functionality-specific machine learning set-up item
    /// </summary>
    /// <typeparam name="ModelInput">Model input generic type</typeparam>
    /// <typeparam name="ModelOutput">Model output generic type</typeparam>
    public abstract class FunctionalitySpecificMLSetupItem: IFunctionalitySpecificMLSetupItem
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

        /// <inheritdoc />
        public abstract string DataExtension { get; }

        protected string TrainingModelFilePath => TrainingModelDirectory + "/" + TrainingModelName + "." + DataExtension;

        /// <summary>
        /// Trained model file path
        /// </summary>
        protected string TrainedModelFilePath => TrainingModelDirectory + "/" + TrainingModelName + ".zip";

        // Temporary training model file path for ML testing area
        protected string TempTrainedModelFilePath => TrainingModelDirectory + "/" + TrainingModelName + "-temp.zip";

        protected string BackupTrainedModelFilePath => TrainingModelDirectory + "/" + TrainingModelName + $"-backup{DateTime.Now}.zip";

        protected IMLTrainingAlgorithm trainingAlgorithm = null;

        /// <summary>
        /// Constructor for the machine learning set-up item
        /// </summary>
        protected FunctionalitySpecificMLSetupItem(string functionalityName)
        {
            TrainingModelName = functionalityName;

            // If the CSV file already exists, import the CSV automatically.
            if (File.Exists(TrainingModelFilePath))
            {
                AddDataInputsBySourceFilePath(TrainingModelFilePath);
            }

            SetTrainingAlgorithm(GetAllTrainingAlgorithms().FirstOrDefault());
        }

        public virtual void OpenDataSchemaSetupForm(FormClosedEventHandler schemaSetupFormClosureAction)
        {
        }

        public void OpenAutoSelectTrainingAlgorithmForm(FormClosedEventHandler formClosureAction)
        {
            AutoSelectTrainingAlgorithmForm autoSelectForm  = new AutoSelectTrainingAlgorithmForm();
            autoSelectForm.FormClosed += formClosureAction;
            autoSelectForm.Show();
        }

        public IEnumerable<MLTrainingAlgorithmType> GetAllTrainingAlgorithms()
        {
            return Enum.GetValues(typeof(MLTrainingAlgorithmType)).OfType<MLTrainingAlgorithmType>();
        }

        /// <inheritdoc />
        public void SetTrainingAlgorithm(MLTrainingAlgorithmType algorithmType)
        {
            trainingAlgorithm = MLTrainingAlgorithmFactory.CreateInstance(algorithmType);
            SetTrainingAlgorithmDependencies(algorithmType);
        }

        /// <summary>
        /// Allows training-algorithm-dependent logic to be set according to the algorithm type chosen
        /// </summary>
        /// <param name="algorithmType">New algorithm type</param>
        protected abstract void SetTrainingAlgorithmDependencies(MLTrainingAlgorithmType algorithmType);

        /// <inheritdoc />
        public List<ITrainingAlgorithmOption> GetTrainingAlgorithmOptions()
        {
            return trainingAlgorithm?.GetCustomisableOptions().ToList() ?? new List<ITrainingAlgorithmOption>();
        }

        /// <inheritdoc />
        public abstract void AddDataInputsBySourceFilePath(string srcFilePath);

        /// <inheritdoc />
        public abstract List<string> GetAllDataInputColumns();

        /// <inheritdoc />
        public abstract List<List<string>> GetAllDataInputsAsStrings();

        /// <inheritdoc />
        public abstract void RemoveDataInputByIndex(int index);

        /// <inheritdoc />
        public abstract void ClearAllDataInput();

        /// <inheritdoc />
        public abstract void SaveModelInputAsDataExtension();

        /// <inheritdoc />
        public abstract bool TryCreateTrainedModelForTesting(out string testingTrainedModelFilePath,
            out TrainerAccuracyCalculator trainedModelAccuracy, double dataSplitTestPercentage = 0.2, int? seed = null);

        /// <summary>
        /// Save the original trained file path as temporary, so that the test prediction can hijack the original file path
        /// </summary>
        protected void SaveOriginalTrainedFilePathAsTemp()
        {
            // Create a copy of the original trained model ZIP and save it as a temp
            // This way we can change the state of the original one for testing.
            if (File.Exists(TrainedModelFilePath))
            {
                // Remove any temporary files already in place.
                if (File.Exists(TempTrainedModelFilePath))
                {
                    File.Delete(TempTrainedModelFilePath);
                }
                File.Copy(TrainedModelFilePath, TempTrainedModelFilePath);
            }
        }

        /// <inhertidoc />
        public abstract IEnumerable<IPredictionTesterDataInputItem> GetAllPredictionTesterDataInputItems();

        /// <inheritdoc />
        public abstract void RunTestPrediction(out string predictedValueAsString);

        /// <inheritdoc />
        public bool ApplyTrainedModel(out string trainedModelFilePath)
        {
            trainedModelFilePath = TrainedModelFilePath;
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

            try
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
            catch
            {
            }

        }
    }
}
