using MLTrainer.DataSetup.DynamicObjectSetup;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MLTrainer.Forms
{
    internal partial class DynamicObjectSchemaSetupForm : Form
    {
        private MLDataSchemaBuilder dataSchemaBuilder;
        private List<Type> optimalTypesDescreasingPriority = new List<Type>();

        internal DynamicObjectSchemaSetupForm(MLDataSchemaBuilder dataSchemaBuilder, List<Type> optimalTypesDescreasingPriority)
        {
            InitializeComponent();
            this.dataSchemaBuilder = dataSchemaBuilder;
            this.optimalTypesDescreasingPriority = optimalTypesDescreasingPriority;

            InitialiseSchemaDataGridView();
        }

        private void InitialiseSchemaDataGridView()
        {
            schemaDataGridView.Rows.Clear();
            foreach(ColumnNameStorageAttribute property in dataSchemaBuilder.GetSchemaProperties())
            {
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.Tag = property;

                // First column - name
                DataGridViewCell nameCell = new DataGridViewTextBoxCell();
                nameCell.Value = property.Name;
                newRow.Cells.Add(nameCell);

                // Second column - type
                DataGridViewComboBoxCell typeCell = new DataGridViewComboBoxCell();
                typeCell.Items.AddRange(optimalTypesDescreasingPriority.ToArray());
                typeCell.Value = property.ColumnType;
                newRow.Cells.Add(typeCell);

                // Third column - label indicator
                DataGridViewCheckBoxCell labelCell = new DataGridViewCheckBoxCell();
                labelCell.Value = property.IsLabel;
                newRow.Cells.Add(labelCell);

                schemaDataGridView.Rows.Add(newRow);
            }

            schemaDataGridView.Update();

        }

        private void schemaDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
