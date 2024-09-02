namespace MLTrainer.Forms
{
    partial class MLTrainingSetupForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.selectFunctionalityLabel = new System.Windows.Forms.Label();
            this.functionalityComboBox = new System.Windows.Forms.ComboBox();
            this.importFileButton = new System.Windows.Forms.Button();
            this.clearAllButton = new System.Windows.Forms.Button();
            this.modelDataPreviewComboBox = new System.Windows.Forms.ListView();
            this.trainModelButton = new System.Windows.Forms.Button();
            this.trainingResultsLabel = new System.Windows.Forms.Label();
            this.saveFileButton = new System.Windows.Forms.Button();
            this.applyTrainedModelButton = new System.Windows.Forms.Button();
            this.removeSelectedButton = new System.Windows.Forms.Button();
            this.testPredictionGroupBox = new System.Windows.Forms.GroupBox();
            this.runTestPredictionButton = new System.Windows.Forms.Button();
            this.testPredictionResultsLabel = new System.Windows.Forms.Label();
            this.testPredictionDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trainingAlgorithmGroupBox = new System.Windows.Forms.GroupBox();
            this.algorithmParametersListView = new System.Windows.Forms.DataGridView();
            this.Parameter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.algorithmComboBox = new System.Windows.Forms.ComboBox();
            this.selectAlgorithmLabel = new System.Windows.Forms.Label();
            this.testSplitPercentageTrackBar = new System.Windows.Forms.TrackBar();
            this.specifyTrainSplitRatioLabel = new System.Windows.Forms.Label();
            this.trainSplitRatioLabel = new System.Windows.Forms.Label();
            this.testPredictionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testPredictionDataGridView)).BeginInit();
            this.trainingAlgorithmGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.algorithmParametersListView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.testSplitPercentageTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // selectFunctionalityLabel
            // 
            this.selectFunctionalityLabel.AutoSize = true;
            this.selectFunctionalityLabel.Location = new System.Drawing.Point(13, 13);
            this.selectFunctionalityLabel.Name = "selectFunctionalityLabel";
            this.selectFunctionalityLabel.Size = new System.Drawing.Size(99, 13);
            this.selectFunctionalityLabel.TabIndex = 0;
            this.selectFunctionalityLabel.Text = "Select Functionality";
            // 
            // functionalityComboBox
            // 
            this.functionalityComboBox.FormattingEnabled = true;
            this.functionalityComboBox.Location = new System.Drawing.Point(118, 10);
            this.functionalityComboBox.Name = "functionalityComboBox";
            this.functionalityComboBox.Size = new System.Drawing.Size(274, 21);
            this.functionalityComboBox.TabIndex = 1;
            this.functionalityComboBox.SelectedIndexChanged += new System.EventHandler(this.functionalityComboBox_SelectedIndexChanged);
            // 
            // importFileButton
            // 
            this.importFileButton.Location = new System.Drawing.Point(16, 45);
            this.importFileButton.Name = "importFileButton";
            this.importFileButton.Size = new System.Drawing.Size(75, 23);
            this.importFileButton.TabIndex = 2;
            this.importFileButton.Text = "Import";
            this.importFileButton.UseVisualStyleBackColor = true;
            this.importFileButton.Click += new System.EventHandler(this.importFileButton_Click);
            // 
            // clearAllButton
            // 
            this.clearAllButton.Location = new System.Drawing.Point(216, 45);
            this.clearAllButton.Name = "clearAllButton";
            this.clearAllButton.Size = new System.Drawing.Size(75, 23);
            this.clearAllButton.TabIndex = 3;
            this.clearAllButton.Text = "Remove All";
            this.clearAllButton.UseVisualStyleBackColor = true;
            this.clearAllButton.Click += new System.EventHandler(this.clearAllButton_Click_1);
            // 
            // modelDataPreviewComboBox
            // 
            this.modelDataPreviewComboBox.HideSelection = false;
            this.modelDataPreviewComboBox.Location = new System.Drawing.Point(16, 75);
            this.modelDataPreviewComboBox.Name = "modelDataPreviewComboBox";
            this.modelDataPreviewComboBox.Size = new System.Drawing.Size(376, 92);
            this.modelDataPreviewComboBox.TabIndex = 4;
            this.modelDataPreviewComboBox.UseCompatibleStateImageBehavior = false;
            this.modelDataPreviewComboBox.View = System.Windows.Forms.View.Details;
            // 
            // trainModelButton
            // 
            this.trainModelButton.Location = new System.Drawing.Point(10, 190);
            this.trainModelButton.Name = "trainModelButton";
            this.trainModelButton.Size = new System.Drawing.Size(75, 23);
            this.trainModelButton.TabIndex = 5;
            this.trainModelButton.Text = "Train Model";
            this.trainModelButton.UseVisualStyleBackColor = true;
            this.trainModelButton.Click += new System.EventHandler(this.trainModelButton_Click);
            // 
            // trainingResultsLabel
            // 
            this.trainingResultsLabel.AutoSize = true;
            this.trainingResultsLabel.Location = new System.Drawing.Point(19, 388);
            this.trainingResultsLabel.Name = "trainingResultsLabel";
            this.trainingResultsLabel.Size = new System.Drawing.Size(0, 13);
            this.trainingResultsLabel.TabIndex = 6;
            // 
            // saveFileButton
            // 
            this.saveFileButton.Location = new System.Drawing.Point(633, 415);
            this.saveFileButton.Name = "saveFileButton";
            this.saveFileButton.Size = new System.Drawing.Size(75, 23);
            this.saveFileButton.TabIndex = 7;
            this.saveFileButton.Text = "Save File";
            this.saveFileButton.UseVisualStyleBackColor = true;
            this.saveFileButton.Click += new System.EventHandler(this.saveFileButton_Click);
            // 
            // applyTrainedModelButton
            // 
            this.applyTrainedModelButton.Location = new System.Drawing.Point(714, 415);
            this.applyTrainedModelButton.Name = "applyTrainedModelButton";
            this.applyTrainedModelButton.Size = new System.Drawing.Size(75, 23);
            this.applyTrainedModelButton.TabIndex = 8;
            this.applyTrainedModelButton.Text = "Apply";
            this.applyTrainedModelButton.UseVisualStyleBackColor = true;
            this.applyTrainedModelButton.Click += new System.EventHandler(this.applyTrainedModelButton_Click);
            // 
            // removeSelectedButton
            // 
            this.removeSelectedButton.Location = new System.Drawing.Point(94, 45);
            this.removeSelectedButton.Name = "removeSelectedButton";
            this.removeSelectedButton.Size = new System.Drawing.Size(116, 23);
            this.removeSelectedButton.TabIndex = 9;
            this.removeSelectedButton.Text = "Remove Selected";
            this.removeSelectedButton.UseVisualStyleBackColor = true;
            this.removeSelectedButton.Click += new System.EventHandler(this.removeSelectedButton_Click);
            // 
            // testPredictionGroupBox
            // 
            this.testPredictionGroupBox.Controls.Add(this.runTestPredictionButton);
            this.testPredictionGroupBox.Controls.Add(this.testPredictionResultsLabel);
            this.testPredictionGroupBox.Controls.Add(this.testPredictionDataGridView);
            this.testPredictionGroupBox.Location = new System.Drawing.Point(416, 75);
            this.testPredictionGroupBox.Name = "testPredictionGroupBox";
            this.testPredictionGroupBox.Size = new System.Drawing.Size(372, 327);
            this.testPredictionGroupBox.TabIndex = 10;
            this.testPredictionGroupBox.TabStop = false;
            this.testPredictionGroupBox.Text = "Prediction Test";
            // 
            // runTestPredictionButton
            // 
            this.runTestPredictionButton.Location = new System.Drawing.Point(6, 287);
            this.runTestPredictionButton.Name = "runTestPredictionButton";
            this.runTestPredictionButton.Size = new System.Drawing.Size(54, 23);
            this.runTestPredictionButton.TabIndex = 6;
            this.runTestPredictionButton.Text = "Run";
            this.runTestPredictionButton.UseVisualStyleBackColor = true;
            this.runTestPredictionButton.Click += new System.EventHandler(this.runTestPredictionButton_Click);
            // 
            // testPredictionResultsLabel
            // 
            this.testPredictionResultsLabel.AutoSize = true;
            this.testPredictionResultsLabel.Location = new System.Drawing.Point(88, 269);
            this.testPredictionResultsLabel.Name = "testPredictionResultsLabel";
            this.testPredictionResultsLabel.Size = new System.Drawing.Size(0, 13);
            this.testPredictionResultsLabel.TabIndex = 4;
            // 
            // testPredictionDataGridView
            // 
            this.testPredictionDataGridView.AllowUserToAddRows = false;
            this.testPredictionDataGridView.AllowUserToDeleteRows = false;
            this.testPredictionDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.testPredictionDataGridView.Location = new System.Drawing.Point(6, 28);
            this.testPredictionDataGridView.Name = "testPredictionDataGridView";
            this.testPredictionDataGridView.Size = new System.Drawing.Size(346, 225);
            this.testPredictionDataGridView.TabIndex = 3;
            this.testPredictionDataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.testPredictionDataGridView_CellEndEdit);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Input Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 150;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Input Value";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 150;
            // 
            // trainingAlgorithmGroupBox
            // 
            this.trainingAlgorithmGroupBox.Controls.Add(this.trainSplitRatioLabel);
            this.trainingAlgorithmGroupBox.Controls.Add(this.specifyTrainSplitRatioLabel);
            this.trainingAlgorithmGroupBox.Controls.Add(this.testSplitPercentageTrackBar);
            this.trainingAlgorithmGroupBox.Controls.Add(this.algorithmParametersListView);
            this.trainingAlgorithmGroupBox.Controls.Add(this.algorithmComboBox);
            this.trainingAlgorithmGroupBox.Controls.Add(this.selectAlgorithmLabel);
            this.trainingAlgorithmGroupBox.Controls.Add(this.trainModelButton);
            this.trainingAlgorithmGroupBox.Location = new System.Drawing.Point(15, 172);
            this.trainingAlgorithmGroupBox.Name = "trainingAlgorithmGroupBox";
            this.trainingAlgorithmGroupBox.Size = new System.Drawing.Size(376, 229);
            this.trainingAlgorithmGroupBox.TabIndex = 11;
            this.trainingAlgorithmGroupBox.TabStop = false;
            this.trainingAlgorithmGroupBox.Text = "Training Algorithm";
            // 
            // algorithmParametersListView
            // 
            this.algorithmParametersListView.AllowUserToAddRows = false;
            this.algorithmParametersListView.AllowUserToDeleteRows = false;
            this.algorithmParametersListView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Parameter,
            this.Value});
            this.algorithmParametersListView.Location = new System.Drawing.Point(10, 49);
            this.algorithmParametersListView.Name = "algorithmParametersListView";
            this.algorithmParametersListView.Size = new System.Drawing.Size(346, 107);
            this.algorithmParametersListView.TabIndex = 2;
            this.algorithmParametersListView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.algorithmParametersListView_CellEndEdit);
            // 
            // Parameter
            // 
            this.Parameter.HeaderText = "Parameter";
            this.Parameter.Name = "Parameter";
            this.Parameter.ReadOnly = true;
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            // 
            // algorithmComboBox
            // 
            this.algorithmComboBox.FormattingEnabled = true;
            this.algorithmComboBox.Location = new System.Drawing.Point(95, 17);
            this.algorithmComboBox.Name = "algorithmComboBox";
            this.algorithmComboBox.Size = new System.Drawing.Size(222, 21);
            this.algorithmComboBox.TabIndex = 1;
            this.algorithmComboBox.SelectedIndexChanged += new System.EventHandler(this.algorithmComboBox_SelectedIndexChanged);
            // 
            // selectAlgorithmLabel
            // 
            this.selectAlgorithmLabel.AutoSize = true;
            this.selectAlgorithmLabel.Location = new System.Drawing.Point(7, 20);
            this.selectAlgorithmLabel.Name = "selectAlgorithmLabel";
            this.selectAlgorithmLabel.Size = new System.Drawing.Size(83, 13);
            this.selectAlgorithmLabel.TabIndex = 0;
            this.selectAlgorithmLabel.Text = "Select Algorithm";
            // 
            // testSplitPercentageTrackBar
            // 
            this.testSplitPercentageTrackBar.Location = new System.Drawing.Point(136, 162);
            this.testSplitPercentageTrackBar.Maximum = 20;
            this.testSplitPercentageTrackBar.Name = "testSplitPercentageTrackBar";
            this.testSplitPercentageTrackBar.Size = new System.Drawing.Size(104, 45);
            this.testSplitPercentageTrackBar.TabIndex = 6;
            this.testSplitPercentageTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.testSplitPercentageTrackBar.Value = 16;
            this.testSplitPercentageTrackBar.Scroll += new System.EventHandler(this.testSplitPercentageTrackBar_Scroll);
            // 
            // specifyTrainSplitRatioLabel
            // 
            this.specifyTrainSplitRatioLabel.AutoSize = true;
            this.specifyTrainSplitRatioLabel.Location = new System.Drawing.Point(7, 167);
            this.specifyTrainSplitRatioLabel.Name = "specifyTrainSplitRatioLabel";
            this.specifyTrainSplitRatioLabel.Size = new System.Drawing.Size(123, 13);
            this.specifyTrainSplitRatioLabel.TabIndex = 7;
            this.specifyTrainSplitRatioLabel.Text = "Specify training split ratio";
            // 
            // trainSplitRatioLabel
            // 
            this.trainSplitRatioLabel.AutoSize = true;
            this.trainSplitRatioLabel.Location = new System.Drawing.Point(133, 194);
            this.trainSplitRatioLabel.Name = "trainSplitRatioLabel";
            this.trainSplitRatioLabel.Size = new System.Drawing.Size(127, 13);
            this.trainSplitRatioLabel.TabIndex = 8;
            this.trainSplitRatioLabel.Text = "80% training : 20% testing";
            // 
            // MLTrainingSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.trainingAlgorithmGroupBox);
            this.Controls.Add(this.testPredictionGroupBox);
            this.Controls.Add(this.removeSelectedButton);
            this.Controls.Add(this.applyTrainedModelButton);
            this.Controls.Add(this.saveFileButton);
            this.Controls.Add(this.trainingResultsLabel);
            this.Controls.Add(this.modelDataPreviewComboBox);
            this.Controls.Add(this.clearAllButton);
            this.Controls.Add(this.importFileButton);
            this.Controls.Add(this.functionalityComboBox);
            this.Controls.Add(this.selectFunctionalityLabel);
            this.Name = "MLTrainingSetupForm";
            this.Text = "Machine Learning Training";
            this.testPredictionGroupBox.ResumeLayout(false);
            this.testPredictionGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testPredictionDataGridView)).EndInit();
            this.trainingAlgorithmGroupBox.ResumeLayout(false);
            this.trainingAlgorithmGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.algorithmParametersListView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.testSplitPercentageTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label selectFunctionalityLabel;
        private System.Windows.Forms.ComboBox functionalityComboBox;
        private System.Windows.Forms.Button importFileButton;
        private System.Windows.Forms.Button clearAllButton;
        private System.Windows.Forms.ListView modelDataPreviewComboBox;
        private System.Windows.Forms.Button trainModelButton;
        private System.Windows.Forms.Label trainingResultsLabel;
        private System.Windows.Forms.Button saveFileButton;
        private System.Windows.Forms.Button applyTrainedModelButton;
        private System.Windows.Forms.Button removeSelectedButton;
        private System.Windows.Forms.GroupBox testPredictionGroupBox;
        private System.Windows.Forms.GroupBox trainingAlgorithmGroupBox;
        private System.Windows.Forms.Label selectAlgorithmLabel;
        private System.Windows.Forms.ComboBox algorithmComboBox;
        private System.Windows.Forms.DataGridView algorithmParametersListView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Parameter;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridView testPredictionDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.Label testPredictionResultsLabel;
        private System.Windows.Forms.Button runTestPredictionButton;
        private System.Windows.Forms.Label specifyTrainSplitRatioLabel;
        private System.Windows.Forms.TrackBar testSplitPercentageTrackBar;
        private System.Windows.Forms.Label trainSplitRatioLabel;
    }
}