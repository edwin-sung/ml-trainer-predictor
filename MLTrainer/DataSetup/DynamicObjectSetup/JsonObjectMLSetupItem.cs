using MLTrainer.PredictionTesterUI;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MLTrainer.DataSetup.DynamicObjectSetup
{
    internal class JsonObjectMLSetupItem : DynamicObjectMLSetupItem
    {

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
                ParseJsonFile(fileOpener.FileName);
            }

        }

        internal void ParseJsonFile(string jsonFilePath)
        {
            // Use data schema builder to build schema for the JSON
            MLDataSchemaBuilder builderInput = new MLDataSchemaBuilder("JsonImportedModelInput");
            MLDataSchemaBuilder builderOutput = new MLDataSchemaBuilder("JsonImportedModelOutput");


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
