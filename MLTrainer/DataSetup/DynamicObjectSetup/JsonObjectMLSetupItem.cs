using MLTrainer.Forms;
using MLTrainer.PredictionTesterUI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MLTrainer.DataSetup.DynamicObjectSetup
{
    internal class JsonObjectMLSetupItem : DynamicObjectMLSetupItem
    {

        private List<Type> optimalTypesDescreasingPriority = new List<Type> { typeof(bool), typeof(int), typeof(float), typeof(string) };

        internal JsonObjectMLSetupItem() : base("JsonObjectTrainingModel")
        {
        }

        public override string Name => "JSON Object Training Model";

        public override string TrainingModelDirectory { get; set; } = "C:\\Temp";
        public override string TrainingModelName { get; set; } = string.Empty;

        public override void InitialiseInstance()
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
                MLDataSchemaBuilder dataSchemaBuilder = ParseJsonFile(fileOpener.FileName);
                DynamicObjectSchemaSetupForm schemaForm = new DynamicObjectSchemaSetupForm(dataSchemaBuilder, optimalTypesDescreasingPriority);
                schemaForm.Show();
            }

        }

        internal MLDataSchemaBuilder ParseJsonFile(string jsonFilePath)
        {
            MLDataSchemaBuilder builderInput = new MLDataSchemaBuilder("JsonImportedModelInput");

            try
            {
                string jsonString = File.ReadAllText(jsonFilePath);
                List<JObject> data = JsonConvert.DeserializeObject<List<JObject>>(jsonString);

                // Set-up the schema based on the deserialised objects, and populate data
                InitialiseDataSchemaBuilder(builderInput, data);
            }
            catch
            {

            }


            return builderInput;
        }

        private void InitialiseDataSchemaBuilder(MLDataSchemaBuilder builder, IEnumerable<JObject> data)
        {
            Dictionary<string, Type> preferredPropertyTypes = new Dictionary<string, Type>();

            // For now, let us accept four different types: string, float, int, bool
            Type GetOptimisedType(object unknown)
            {
                if (unknown.GetType() == typeof(string)) return typeof(string);
                if (bool.TryParse(unknown.ToString(), out bool _)) return typeof(bool);
                if (int.TryParse(unknown.ToString(), out int _)) return typeof(int);
                if (float.TryParse(unknown.ToString(), out float _)) return typeof(float);
                return typeof(string);
            }

            // Storing actions to be called on the data schema builder instace, once properties are in place.
            List<Action<MLDataSchemaBuilder>> addDataInputActions = new List<Action<MLDataSchemaBuilder>>();

            foreach(Dictionary<string, object> pair in data.Select(d => d.ToObject<Dictionary<string, object>>()))
            {
                addDataInputActions.Add(b => b.AddSingularData(pair.Select(kv => (kv.Key, Convert.ChangeType(kv.Value, preferredPropertyTypes[kv.Key])))));
                foreach(string key in pair.Keys)
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

            // Go through each key-value pair and add to properties
            foreach(KeyValuePair<string, Type> pair in preferredPropertyTypes)
            {
                builder.AddProperty(pair.Key, pair.Value);
            }

            // With properties all set, now set the data.
            addDataInputActions.ForEach(a => a?.Invoke(builder));
        }


        public override void AddDataInputsByCSVFilePath(string csvFilePath)
        {
            // For the moment, do not import anything just yet
        }

        public override void ClearAllDataInput()
        {
            // For now, do not do anything until the implementation was complete
        }

        public override List<string> GetAllDataInputColumns()
        {
            throw new System.NotImplementedException();
        }

        public override List<List<string>> GetAllDataInputsAsStrings()
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerable<IPredictionTesterDataInputItem> GetAllPredictionTesterDataInputItems()
        {
            // TODO: Will be done later on
            yield break;
        }

        public override void RemoveDataInputByIndex(int index)
        {
            // Not going to implement this for now
        }

        public override void RunTestPrediction(out string predictedValueAsString)
        {
            predictedValueAsString = string.Empty;
            // Will be done later on
        }

        public override void SaveModelInputAsCSV()
        {
            // This will be done later on
        }

        public override bool TryCreateTrainedModelForTesting(out string testingTrainedModelFilePath)
        {
            throw new System.NotImplementedException();
        }
    }
}
