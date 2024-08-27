using MLTrainer.RuntimeTrainingSetup.DynamicObjectSetup;
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

        // This event handler manually raises the CellValueChanged event
        // by calling the CommitEdit method.
        void schemaDataGridView_CurrentCellDirtyStateChanged(object sender,
            EventArgs e)
        {
            if (schemaDataGridView.IsCurrentCellDirty)
            {
                schemaDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void schemaDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (schemaDataGridView.CurrentCell.ColumnIndex == 1 && e.Control is ComboBox)
            {
                ComboBox comboBox = e.Control as ComboBox;
                comboBox.SelectedIndexChanged += LastColumnComboSelectionChanged;
            }
        }

        private void LastColumnComboSelectionChanged(object sender, EventArgs e)
        {
            if (schemaDataGridView.CurrentCell.ColumnIndex == 1)
            {
                TypeChangeAction(schemaDataGridView.CurrentCell.RowIndex, schemaDataGridView.CurrentCell.ColumnIndex);
            }
            
        }

        private void schemaDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow rowView = schemaDataGridView.Rows[e.RowIndex];
                // Get the schema builder instance from the row
                if (rowView.Tag is ColumnNameStorageAttribute validProperty)
                {
                    switch (e.ColumnIndex)
                    {
                        case 2:
                        {
                            LabelTickAction(e.RowIndex, e.ColumnIndex);
                            break;
                        }
                        default: break;
                    }
                }
            } 
            catch
            {

            }

        }

        private void TypeChangeAction(int row, int column)
        {
            DataGridViewComboBoxCell selectedCell = (DataGridViewComboBoxCell)schemaDataGridView.Rows[row].Cells[column];
            if (schemaDataGridView.Rows[row].Tag is ColumnNameStorageAttribute propertyAttribute)
            {
                try
                {
                    DataGridViewComboBoxCell typeCell = new DataGridViewComboBoxCell();
                    typeCell.Items.AddRange(optimalTypesDescreasingPriority.ToArray());
                    typeCell.Value = System.Type.GetType(selectedCell.Value.ToString());
                    selectedCell = typeCell;
                } 
                catch
                {
                    selectedCell.Value = propertyAttribute.ColumnType;
                }

            }
            schemaDataGridView.Update();
        }

        private void LabelTickAction(int row, int column)
        {
            // We are simply ticking the one that is selected, and untick any previous values
            for(int i = 0; i < schemaDataGridView.Rows.Count; i++)
            {
                DataGridViewRow rowView = schemaDataGridView.Rows[i];

                rowView.Cells[column].Value = i == row;
                if (rowView.Tag is ColumnNameStorageAttribute propertyAttribute)
                {
                    propertyAttribute.IsLabel = i == row;
                }
            }

            schemaDataGridView.Update();
        }

        private void applySchemaButton_Click(object sender, EventArgs e)
        {
            if (!dataSchemaBuilder.SchemaProperlySetup(out string errorMessage))
            {
                MessageBox.Show(errorMessage);
                return;
            }

            dataSchemaBuilder.InitialiseSchemaType();

            Close();
        }
    }
}
