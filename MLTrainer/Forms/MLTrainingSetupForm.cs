using MLTrainer.DataSetup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            //SetupFunctionalityList();
        }


        /*private void SetupFunctionalityList()
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
                ListViewItem newRow = new ListViewItem(string.Join(",", row));
                foreach (string column in row.Skip(1))
                {
                    newRow.SubItems.Add(column);
                }

                modelDataPreviewComboBox.Items.Add(newRow);
            }

            modelDataPreviewComboBox.EndUpdate();
        }

        private void FunctionalityComboBoxOnSelectedIndexChanged(object sender, EventArgs e)
        {
            selectedSetup = setupItems[functionalityComboBox.SelectedIndex];
            RefreshRows();
        }

        private void addCEButton_Click_1(object sender, EventArgs e)
        {
            selectedSetup.AddDataInputsByCurrentElement(CurrentElement.Element);
            RefreshRows();
        }

        private void importFromCSV_Click(object sender, EventArgs e)
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

        private void clearSelectedButton_Click(object sender, EventArgs e)
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

        private void clearAllButton_Click(object sender, EventArgs e)
        {
            modelDataPreviewComboBox.Items.Clear();
            selectedSetup.ClearAllDataInput();
            modelDataPreviewComboBox.Update();
        }

        private void trainModelDataButton_Click(object sender, EventArgs e)
        {
            trainingResultsLabel.Text = selectedSetup.TryCreateTrainedModelForTesting()
                ? "Trained model now created for testing purposes"
                : "Trained model cannot be created.";
        }

        private void saveCSVButton_Click(object sender, EventArgs e)
        {
            selectedSetup.SaveModelInputAsCSV();
            trainingResultsLabel.Text = "CSV successfully saved.";
        }

        private void apply_Click(object sender, EventArgs e)
        {
            trainingResultsLabel.Text = selectedSetup.ApplyTrainedModel()
                ? "Trained model applied successfully"
                : "Trained model application unsuccessful.";
        }*/

    }
}
