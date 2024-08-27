using MLTrainer.Forms;
using MLTrainer.PredictionTesterUI;
using MLTrainer.RuntimeTrainingSetup.DynamicObjectPredictionTest;
using MLTrainer.RuntimeTrainingSetup.DynamicObjectPredictor;
using MLTrainer.RuntimeTrainingSetup.DynamicObjectTrainer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MLTrainer.RuntimeTrainingSetup.DynamicObjectSetup
{
    internal class JsonObjectMLSetupItem : DynamicObjectMLSetupItem
    {
        private List<Type> optimalTypesDescreasingPriority = new List<Type> { typeof(bool), typeof(float), typeof(string) };
        private MLDataSchemaBuilder inputDataSchemaBuilder = null, outputDataSchemaBuilder = null;
        private DynamicObjectPredictionTester predictionTester = null;

        internal JsonObjectMLSetupItem() : base("JsonObjectTrainingModel")
        {
        }

        public override string Name => "JSON Object Training Model";

        public override string TrainingModelDirectory { get; set; } = "C:\\Temp";
        public override string TrainingModelName { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string DataExtension => "json";

        public override void OpenDataSchemaSetupForm(FormClosedEventHandler schemaSetupFormClosureAction)
        {
            OpenFileDialog fileOpener = new OpenFileDialog();
            fileOpener.ValidateNames = true;
            fileOpener.CheckFileExists = true;
            fileOpener.CheckPathExists = true;
            fileOpener.DefaultExt = "json";
            fileOpener.Filter = "JSON file (*.json)|*.json";
            fileOpener.Title = "Select a file to import";
            fileOpener.Multiselect = false;

            DialogResult result = fileOpener.ShowDialog();

            if (result == DialogResult.OK)
            {
                int place = fileOpener.SafeFileName.LastIndexOf(".json");
                if (place >= 0)
                {
                    TrainingModelName = fileOpener.SafeFileName.Remove(place, fileOpener.SafeFileName.Length - place).Insert(place, string.Empty);
                }
                else
                {
                    TrainingModelName = fileOpener.SafeFileName;
                }

                inputDataSchemaBuilder = new MLDataSchemaBuilder("JsonImportedModelInput");
                ParseJsonFile(fileOpener.FileName, d => GetNewDataInputActions(d, true).ForEach(a => a?.Invoke(inputDataSchemaBuilder)));
                DynamicObjectSchemaSetupForm schemaForm = new DynamicObjectSchemaSetupForm(inputDataSchemaBuilder, optimalTypesDescreasingPriority);
                schemaForm.FormClosed += schemaSetupFormClosureAction;
                schemaForm.Show();
            }
        }

        internal void ParseJsonFile(string jsonFilePath, Action<IEnumerable<JObject>> parsedDataAction)
        {
            try
            {
                string jsonString = File.ReadAllText(jsonFilePath);

                // Set-up the schema based on the deserialised objects, and populate data
                parsedDataAction?.Invoke(JsonConvert.DeserializeObject<List<JObject>>(jsonString));
            }
            catch
            {
            }
        }

        internal void SaveJSONFile(string newJsonFilePath)
        {
            try
            {
                List<JObject> jsonObjects = new List<JObject>();
                foreach(List<(string, object)> nameValuePairs in inputDataSchemaBuilder.GetInputDataAsNameValuePairs())
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    nameValuePairs.ForEach(pair => dict[pair.Item1] = pair.Item2);
                    jsonObjects.Add(JObject.FromObject(dict));
                }

                string jsonSaveString = JsonConvert.SerializeObject(jsonObjects);
                File.WriteAllText(newJsonFilePath, jsonSaveString);
            } 
            catch
            {

            }
        }

        /// <inheritdoc />
        public override void AddDataInputsBySourceFilePath(string srcFilePath)
        {
            ParseJsonFile(srcFilePath, parsedData =>
                GetNewDataInputActions(parsedData).ForEach(action => action?.Invoke(inputDataSchemaBuilder)));
        }

        private List<Action<MLDataSchemaBuilder>> GetNewDataInputActions(IEnumerable<JObject> data, bool addNewProperties = false)
        {
            Dictionary<string, Type> preferredPropertyTypes = new Dictionary<string, Type>();

            // For now, let us accept four different types: string, float, int, bool
            Type GetOptimisedType(object unknown)
            {
                if (unknown.GetType() == typeof(string)) return typeof(string);
                if (bool.TryParse(unknown.ToString(), out bool _)) return typeof(bool);
                if (float.TryParse(unknown.ToString(), out float _)) return typeof(float);
                return typeof(string);
            }

            // Storing actions to be called on the data schema builder instace, once properties are in place.
            List<Action<MLDataSchemaBuilder>> addDataInputActions = new List<Action<MLDataSchemaBuilder>>();

            foreach (Dictionary<string, object> pair in data.Select(d => d.ToObject<Dictionary<string, object>>()))
            {
                addDataInputActions.Add(b => b.AddSingularData(pair.Select(kv => (kv.Key, Convert.ChangeType(kv.Value, preferredPropertyTypes[kv.Key])))));
                foreach (string key in pair.Keys)
                {
                    object value = pair[key];
                    Type optimisedType = GetOptimisedType(value);
                    if (!preferredPropertyTypes.ContainsKey(key))
                    {
                        preferredPropertyTypes[key] = optimisedType;
                        continue;
                    }
                    Type originalType = preferredPropertyTypes[key];

                    preferredPropertyTypes[key] =
                        optimalTypesDescreasingPriority[Math.Min(optimalTypesDescreasingPriority.IndexOf(originalType), optimalTypesDescreasingPriority.IndexOf(optimisedType))];
                }
            }

            if (addNewProperties)
            {
                // Go through each key-value pair and add to properties first, before adding all the other records.
                addDataInputActions.Insert(0, dataSchemaBuilder =>
                {
                    foreach (KeyValuePair<string, Type> pair in preferredPropertyTypes)
                    {
                        dataSchemaBuilder.AddProperty(pair.Key, pair.Value);
                    }
                });
            }

            return addDataInputActions;
        }

        public override void ClearAllDataInput()
        {
            // For now, do not do anything until the implementation was complete
        }

        public override List<string> GetAllDataInputColumns() =>
            inputDataSchemaBuilder.TryGetColumnNames(label => true, out List<string> columnNames) ? columnNames : new List<string>();

        public override List<List<string>> GetAllDataInputsAsStrings()
        {
            return inputDataSchemaBuilder.GetInputDataAsNameValuePairs().Select(
                pairs => pairs.Select(pair => pair.Item2.ToString()).ToList()).ToList();
        }

        public override IEnumerable<IPredictionTesterDataInputItem> GetAllPredictionTesterDataInputItems()
        {
            return predictionTester?.DataInputItems ?? new List<IPredictionTesterDataInputItem>();
        }

        public override void RemoveDataInputByIndex(int index)
        {
            // Not going to implement this for now
        }

        public override void RunTestPrediction(out string predictedValueAsString)
        {
            predictedValueAsString = string.Empty;
            predictionTester?.RunPrediction(new DynamicObjectModelPredictor(TrainedModelFilePath, inputDataSchemaBuilder.SchemaType, outputDataSchemaBuilder.SchemaType), out predictedValueAsString);
        }

        public override void SaveModelInputAsDataExtension()
        {
            SaveJSONFile(TrainingModelFilePath);
        }

        public override bool TryCreateTrainedModelForTesting(out string testingTrainedModelFilePath)
        {
            SaveOriginalTrainedFilePathAsTemp();
            testingTrainedModelFilePath = string.Empty;
            if (!inputDataSchemaBuilder.TryCreateOutputDataSchemaBuilder(out outputDataSchemaBuilder))
            {
                return false;
            }

            DynamicObjectModelTrainer trainer = new DynamicObjectModelTrainer(inputDataSchemaBuilder.SchemaType, outputDataSchemaBuilder.SchemaType, trainingAlgorithm);

            if (trainer.TryTrainModel(inputDataSchemaBuilder, outputDataSchemaBuilder, TrainedModelFilePath))
            {
                testingTrainedModelFilePath = TrainedModelFilePath;
                predictionTester = new DynamicObjectPredictionTester(inputDataSchemaBuilder);
                return true;
            }

            return false;
        }
    }
}
