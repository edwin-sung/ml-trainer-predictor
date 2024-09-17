using MLTrainer.DataSetup;
using MLTrainer.Trainer;
using MLTrainer.TrainingAlgorithms;
using MLTrainer.TrainingAlgorithms.AutoSelection;
using MLTrainer.TrainingAlgorithms.CustomisableOption;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace MLTrainer.Forms
{
    public partial class AutoSelectTrainingAlgorithmForm : Form
    {

        private IFunctionalitySpecificMLSetupItem selectedSetupItem;

        public AutoSelectTrainingAlgorithmForm(IFunctionalitySpecificMLSetupItem selectedSetupItem)
        {
            InitializeComponent();

            SetupOptimisableObjectiveList();

            this.selectedSetupItem = selectedSetupItem;
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

        private void startTrainingButton_Click(object sender, EventArgs e)
        {
            int? seed = 42;
            double testFraction = 0.2;

            double highestAccuracy = double.MinValue;
            MLTrainingAlgorithmType bestAlgorithmToUse = default;

            // Use the set-up item to set algorithm and kick off the train modules
            foreach(MLTrainingAlgorithmType algorithm in selectedSetupItem.GetAllTrainingAlgorithms())
            {
                selectedSetupItem.SetTrainingAlgorithm(algorithm);
                if (selectedSetupItem.TryCreateTrainedModelForTesting(out string testTrainedModelFilePath, out TrainerAccuracyCalculator trainedModelAccuracy, testFraction, seed))
                {
                    if (trainedModelAccuracy.GetAccuracy() is double validAccuracy && validAccuracy > highestAccuracy)
                    {
                        highestAccuracy = validAccuracy;
                        bestAlgorithmToUse = algorithm;
                    }
                }
            }

            selectedSetupItem.SetTrainingAlgorithm(bestAlgorithmToUse);
            Close();
        }
    }
}
