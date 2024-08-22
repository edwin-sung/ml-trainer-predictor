using MLTrainer.DataSetup;
using MLTrainer.PredictionTesterUI;
using MLTrainer.TrainingAlgorithms;
using MLTrainer.TrainingAlgorithms.CustomisableOption;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace MLTrainer.Forms
{
    public partial class MLTrainingSetupForm : Form
    {
        private List<IFunctionalitySpecificMLSetupItem> setupItems = new List<IFunctionalitySpecificMLSetupItem>();

        private IFunctionalitySpecificMLSetupItem selectedSetup;

        public MLTrainingSetupForm(IFunctionalitySpecificMLSetupFactory setupFactory)
        {
            InitializeComponent();
            setupItems.AddRange(setupFactory.GetAvailableSetupItems());

            SetupFunctionalityList();
            SetupAlgorithmList();
            SetupPredictionTestDataInputGridView();
        }


        private void SetupFunctionalityList()
        {
            functionalityComboBox.BeginUpdate();
            functionalityComboBox.Items.Clear();
            foreach (IFunctionalitySpecificMLSetupItem item in setupItems)
            {
                functionalityComboBox.Items.Add(item.Name);
            }

            selectedSetup = setupItems.FirstOrDefault();
            functionalityComboBox.SelectedIndex = 0;

            functionalityComboBox.EndUpdate();
        }

        private void SetupAlgorithmList()
        {
            algorithmComboBox.DisplayMember = "Description";
            algorithmComboBox.ValueMember = "Value";
            algorithmComboBox.DataSource = Enum.GetValues(typeof(MLTrainingAlgorithmType)).Cast<Enum>().Select(
                value => (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), 
                typeof(DescriptionAttribute)) as DescriptionAttribute).Description).ToList();
            algorithmComboBox.Update();

            algorithmComboBox.SelectedIndex = 0;
        }

        private void SetupAlgorithmParametersDataGridView()
        {
            algorithmParametersListView.Rows.Clear();
            foreach (ITrainingAlgorithmOption option in selectedSetup.GetTrainingAlgorithmOptions())
            {
                DataGridViewRow newRow = new DataGridViewRow();
                DataGridViewCell cell = new DataGridViewTextBoxCell();
                cell.Value = option.Name;
                newRow.Cells.Add(cell);

                DataGridViewCell valueCell = new DataGridViewTextBoxCell();
                valueCell.Tag = option;
                valueCell.Value = option.TryGetValueAsString(out string valueAsString) ? valueAsString : string.Empty;
                newRow.Cells.Add(valueCell);

                algorithmParametersListView.Rows.Add(newRow);
            }

            algorithmParametersListView.Update();
        }

        private void SetupPredictionTestDataInputGridView()
        {
            testPredictionDataGridView.Rows.Clear();
            foreach(IPredictionTesterDataInputItem dataInputItem in selectedSetup.GetAllPredictionTesterDataInputItems())
            {
                DataGridViewRow newRow = new DataGridViewRow();
                DataGridViewCell cell = new DataGridViewTextBoxCell();
                cell.Value = dataInputItem.Name;
                newRow.Cells.Add(cell);

                DataGridViewCell valueCell = new DataGridViewTextBoxCell();
                valueCell.Tag = dataInputItem;
                valueCell.Value = dataInputItem.GetValueAsString();
                newRow.Cells.Add(valueCell);

                testPredictionDataGridView.Rows.Add(newRow);
            }

            testPredictionDataGridView.Update();
        }

        private void RefreshRows()
        {
            modelDataPreviewComboBox.BeginUpdate();

            modelDataPreviewComboBox.Columns.Clear();
            selectedSetup.GetAllDataInputColumns().ForEach(c => modelDataPreviewComboBox.Columns.Add(c));

            modelDataPreviewComboBox.Items.Clear();
            foreach (List<string> row in selectedSetup.GetAllDataInputsAsStrings())
            {
                if (!row.Any())
                {
                    continue;
                }
                ListViewItem newRow = new ListViewItem(row[0]);
                foreach (string column in row.Skip(1))
                {
                    newRow.SubItems.Add(column);
                }

                modelDataPreviewComboBox.Items.Add(newRow);
            }

            modelDataPreviewComboBox.EndUpdate();
        }

        private void clearAllButton_Click_1(object sender, EventArgs e)
        {
            modelDataPreviewComboBox.Items.Clear();
            selectedSetup.ClearAllDataInput();
            modelDataPreviewComboBox.Update();
        }

        private void importCSVButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileOpener = new OpenFileDialog();
            fileOpener.ValidateNames = true;
            fileOpener.CheckFileExists = true;
            fileOpener.CheckPathExists = true;
            fileOpener.DefaultExt = "csv";
            fileOpener.Filter = "CSV file (*.csv)|*.csv";
            fileOpener.Title = "Select a file to import";
            fileOpener.Multiselect = false;

            DialogResult result = fileOpener.ShowDialog();

            if (result == DialogResult.OK)
            {
                selectedSetup.AddDataInputsByCSVFilePath(fileOpener.FileName);
                RefreshRows();
            }
        }

        private void trainModelButton_Click(object sender, EventArgs e)
        {
            trainingResultsLabel.Text = selectedSetup.TryCreateTrainedModelForTesting(out string filePath)
                ? $"Trained model now created ({filePath}) for testing purposes"
                : "Trained model cannot be created.";

            SetupPredictionTestDataInputGridView();
        }

        private void applyTrainedModelButton_Click(object sender, EventArgs e)
        {
            trainingResultsLabel.Text = selectedSetup.ApplyTrainedModel(out string filePath)
                ? $"Trained model applied successfully to {filePath}"
                : "Trained model application unsuccessful.";
        }

        private void saveCSVButton_Click(object sender, EventArgs e)
        {
            selectedSetup.SaveModelInputAsCSV();
            trainingResultsLabel.Text = "CSV successfully saved.";
        }

        private void removeSelectedButton_Click(object sender, EventArgs e)
        {
            List<int> indices = new List<int>();
            foreach (int index in modelDataPreviewComboBox.SelectedIndices)
            {
                indices.Add(index);
            }

            foreach (var i in indices.OrderByDescending(t => t))
            {
                modelDataPreviewComboBox.Items.RemoveAt(i);
                selectedSetup.RemoveDataInputByIndex(i);
            }

            RefreshRows();
        }

        private void modelDataPreviewComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedSetup = setupItems[functionalityComboBox.SelectedIndex];
            RefreshRows();
        }

        private void algorithmComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                selectedSetup.SetTrainingAlgorithm((MLTrainingAlgorithmType)algorithmComboBox.SelectedIndex);
                SetupAlgorithmParametersDataGridView();
            } 
            catch
            {

            }

        }

        private void algorithmParametersListView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string newValue = (string)algorithmParametersListView[e.ColumnIndex, e.RowIndex].Value;
                foreach (DataGridViewCell cell in algorithmParametersListView.SelectedCells)
                {
                    if (!(cell.Tag is ITrainingAlgorithmOption validOption))
                    {
                        continue;
                    }

                    if (!validOption.TrySetValue(newValue))
                    {
                        // Revert to the original value.
                        validOption.TryGetValueAsString(out string oldValue);
                        algorithmParametersListView[e.ColumnIndex, e.RowIndex].Value = oldValue;
                    }
                }
            } catch
            {

            }
        }

        private void testPredictionDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string newValue = (string)testPredictionDataGridView[e.ColumnIndex, e.RowIndex].Value;
                foreach (DataGridViewCell cell in testPredictionDataGridView.SelectedCells)
                {
                    if (!(cell.Tag is IPredictionTesterDataInputItem validTestInputItem))
                    {
                        continue;
                    }

                    if (!validTestInputItem.TrySetValue(newValue))
                    {
                        // Revert to the original value.
                        testPredictionDataGridView[e.ColumnIndex, e.RowIndex].Value = validTestInputItem.GetValueAsString();
                    }
                }
            } 
            catch
            {

            }
        }

        private void functionalityComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedSetup = setupItems.SingleOrDefault(item => item.Name == functionalityComboBox.SelectedItem.ToString());
            SetupAlgorithmParametersDataGridView();
        }

        private void runTestPredictionButton_Click(object sender, EventArgs e)
        {
            selectedSetup.RunTestPrediction(out string predictedValueAsString);
            testPredictionResultsLabel.Text = string.IsNullOrEmpty(predictedValueAsString)
                ? "Predicted result cannot be calculated, please review the data input"
                : $"The predicted output is {predictedValueAsString}";
        }
    }
}
