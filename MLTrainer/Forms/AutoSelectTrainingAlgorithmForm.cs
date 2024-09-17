using MLTrainer.TrainingAlgorithms.AutoSelection;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace MLTrainer.Forms
{
    public partial class AutoSelectTrainingAlgorithmForm : Form
    {

        public AutoSelectTrainingAlgorithmForm()
        {
            InitializeComponent();

            SetupOptimisableObjectiveList();
        }

        private void SetupOptimisableObjectiveList()
        {
            optimisableObjectiveComboBox.DisplayMember = "Description";
            optimisableObjectiveComboBox.ValueMember = "Value";
            optimisableObjectiveComboBox.DataSource = Enum.GetValues(typeof(OptimisableObjective)).Cast<Enum>().Select(
                value => (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()),
                typeof(DescriptionAttribute)) as DescriptionAttribute).Description).ToList();
            optimisableObjectiveComboBox.Update();

            optimisableObjectiveComboBox.SelectedIndex = 0;
        }
    }
}
