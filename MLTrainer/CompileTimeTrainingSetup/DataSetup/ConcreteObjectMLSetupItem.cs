using MLTrainer.CompileTimeTrainingSetup.ConcreteObjectPredictionTest;
using MLTrainer.CompileTimeTrainingSetup.ConcreteObjectPredictor;
using MLTrainer.CompileTimeTrainingSetup.ConcreteObjectTrainer;
using MLTrainer.DataSetup;
using MLTrainer.PredictionTesterUI;
using MLTrainer.Trainer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MLTrainer.CompileTimeTrainingSetup.DataSetup
{
    /// <summary>
    /// Abstract class for functionality-specific machine learning set-up item, which has concrete objects defined
    /// </summary>
    /// <typeparam name="ModelInput">Model input generic type</typeparam>
    public abstract class ConcreteObjectMLSetupItem<ModelInput> : FunctionalitySpecificMLSetupItem
        where ModelInput : class, new()
    {
        private List<ModelInput> modelInputs = new List<ModelInput>();
        private List<IPredictionTesterDataInputItem> PredictionTesterDataInputItems { get; set; } = new List<IPredictionTesterDataInputItem>();
        private Func<string> RunTestPredictionFunction = null;

        public override string DataExtension => "csv";

        protected ConcreteObjectMLSetupItem(string functionalityName) : base(functionalityName)
        {
        }

        /// <summary>
        /// Adds a new ModelInput instance to the collection
        /// </summary>
        /// <param name="newDataInput">New ModelInput instance</param>
        protected void Add(ModelInput newDataInput) => modelInputs.Add(newDataInput);

        /// <inheritdoc />
        public override void RemoveDataInputByIndex(int index) => modelInputs.RemoveAt(index);

        /// <inheritdoc />
        public override void ClearAllDataInput() => modelInputs.Clear();

        /// <inheritdoc />
        public override void AddDataInputsBySourceFilePath(string csvFilePath)
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

        /// <inheritdoc />
        public override List<List<string>> GetAllDataInputsAsStrings()
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

        /// <inheritdoc />
        public override List<string> GetAllDataInputColumns()
        {
            return TryGetColumnNamesFor<ModelInput>(label => true, out List<string> results) ? results : new List<string>();
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

        protected void SetOutputDependentTrainModelCreation<T>() where T : class, new()
        {
            CreateTrainedModelForTestingFunction = TryCreateTrainedModelForTesting<T>;
        }
        private delegate bool CreateTrainedModelForTestingDelegate(out string testingTrainedModelFilePath, 
            out TrainerAccuracyCalculator trainedModelAccuracy, double dataSplitTestPercentage = 0.2, int? seed = null);
        private CreateTrainedModelForTestingDelegate CreateTrainedModelForTestingFunction = null;


        private bool TryCreateTrainedModelForTesting<T>(out string testingTrainedModelFilePath, out TrainerAccuracyCalculator accuracyResult, double dataSplitTestPercentage = 0.2, int? seed = null) where T : class, new()
        {
            SaveOriginalTrainedFilePathAsTemp();

            testingTrainedModelFilePath = string.Empty;

            ConcreteObjectModelTrainer<ModelInput, T> trainer = new ConcreteObjectModelTrainer<ModelInput, T>();

            if (trainer.TryTrainModel(trainingAlgorithm, modelInputs, TrainedModelFilePath, out accuracyResult, dataSplitTestPercentage, seed))
            {
                testingTrainedModelFilePath = TrainedModelFilePath;
                ConcreteObjectPredictionTester<ModelInput, T> predictionTester =
                    new ConcreteObjectPredictionTester<ModelInput, T>();
                PredictionTesterDataInputItems = predictionTester.DataInputItems;
                RunTestPredictionFunction = () =>
                {
                    return predictionTester.RunPrediction(new ConcreteObjectModelPredictor<ModelInput, T>(TrainedModelFilePath),
                        out string predictedValueAsString) ? predictedValueAsString : string.Empty;
                };
                return true;
            }
            return false;
        }
             
        /// <inheritdoc />
        public override bool TryCreateTrainedModelForTesting(out string testingTrainedModelFilePath, out TrainerAccuracyCalculator accuracyResult, double dataSplitTestPercentage = 0.2, int? seed = null)
        {
            testingTrainedModelFilePath = string.Empty;
            accuracyResult = null;
            return CreateTrainedModelForTestingFunction?.Invoke(out testingTrainedModelFilePath, out accuracyResult, dataSplitTestPercentage, seed) ?? false;
        }

        /// <inhertidoc />
        public override IEnumerable<IPredictionTesterDataInputItem> GetAllPredictionTesterDataInputItems() => PredictionTesterDataInputItems;

        /// <inheritdoc />
        public override void RunTestPrediction(out string predictedValueAsString) =>
            predictedValueAsString = RunTestPredictionFunction?.Invoke() ?? string.Empty;


        /// <inheritdoc />
        public override void SaveModelInputAsDataExtension()
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
        /// Converts the given model input to a CSV-writable and CSV-readable string
        /// </summary>
        /// <param name="input">ModelInput instance</param>
        /// <param name="csvRow">[Output] CSV row as string</param>
        /// <returns></returns>
        protected abstract bool TryConvertToCSVString(ModelInput input, out string csvRow);

    }
}
