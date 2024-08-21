﻿namespace MLTrainer.Forms
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
            this.importCSVButton = new System.Windows.Forms.Button();
            this.clearAllButton = new System.Windows.Forms.Button();
            this.modelDataPreviewComboBox = new System.Windows.Forms.ListView();
            this.trainModelButton = new System.Windows.Forms.Button();
            this.trainingResultsLabel = new System.Windows.Forms.Label();
            this.saveCSVButton = new System.Windows.Forms.Button();
            this.applyTrainedModelButton = new System.Windows.Forms.Button();
            this.removeSelectedButton = new System.Windows.Forms.Button();
            this.testPredictionGroupBox = new System.Windows.Forms.GroupBox();
            this.trainingAlgorithmGroupBox = new System.Windows.Forms.GroupBox();
            this.algorithmParametersListView = new System.Windows.Forms.DataGridView();
            this.algorithmComboBox = new System.Windows.Forms.ComboBox();
            this.selectAlgorithmLabel = new System.Windows.Forms.Label();
            this.Parameter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trainingAlgorithmGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.algorithmParametersListView)).BeginInit();
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
            // importCSVButton
            // 
            this.importCSVButton.Location = new System.Drawing.Point(16, 45);
            this.importCSVButton.Name = "importCSVButton";
            this.importCSVButton.Size = new System.Drawing.Size(75, 23);
            this.importCSVButton.TabIndex = 2;
            this.importCSVButton.Text = "Import";
            this.importCSVButton.UseVisualStyleBackColor = true;
            this.importCSVButton.Click += new System.EventHandler(this.importCSVButton_Click);
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
            this.modelDataPreviewComboBox.SelectedIndexChanged += new System.EventHandler(this.modelDataPreviewComboBox_SelectedIndexChanged);
            // 
            // trainModelButton
            // 
            this.trainModelButton.Location = new System.Drawing.Point(16, 352);
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
            this.trainingResultsLabel.Location = new System.Drawing.Point(20, 380);
            this.trainingResultsLabel.Name = "trainingResultsLabel";
            this.trainingResultsLabel.Size = new System.Drawing.Size(0, 13);
            this.trainingResultsLabel.TabIndex = 6;
            // 
            // saveCSVButton
            // 
            this.saveCSVButton.Location = new System.Drawing.Point(633, 415);
            this.saveCSVButton.Name = "saveCSVButton";
            this.saveCSVButton.Size = new System.Drawing.Size(75, 23);
            this.saveCSVButton.TabIndex = 7;
            this.saveCSVButton.Text = "Save CSV";
            this.saveCSVButton.UseVisualStyleBackColor = true;
            this.saveCSVButton.Click += new System.EventHandler(this.saveCSVButton_Click);
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
            this.testPredictionGroupBox.Location = new System.Drawing.Point(416, 75);
            this.testPredictionGroupBox.Name = "testPredictionGroupBox";
            this.testPredictionGroupBox.Size = new System.Drawing.Size(372, 300);
            this.testPredictionGroupBox.TabIndex = 10;
            this.testPredictionGroupBox.TabStop = false;
            this.testPredictionGroupBox.Text = "Prediction Test";
            // 
            // trainingAlgorithmGroupBox
            // 
            this.trainingAlgorithmGroupBox.Controls.Add(this.algorithmParametersListView);
            this.trainingAlgorithmGroupBox.Controls.Add(this.algorithmComboBox);
            this.trainingAlgorithmGroupBox.Controls.Add(this.selectAlgorithmLabel);
            this.trainingAlgorithmGroupBox.Location = new System.Drawing.Point(15, 172);
            this.trainingAlgorithmGroupBox.Name = "trainingAlgorithmGroupBox";
            this.trainingAlgorithmGroupBox.Size = new System.Drawing.Size(376, 173);
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
            this.algorithmParametersListView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.algorithmParametersListView_CellValueChanged);
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
            // MLTrainingSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.trainingAlgorithmGroupBox);
            this.Controls.Add(this.testPredictionGroupBox);
            this.Controls.Add(this.removeSelectedButton);
            this.Controls.Add(this.applyTrainedModelButton);
            this.Controls.Add(this.saveCSVButton);
            this.Controls.Add(this.trainingResultsLabel);
            this.Controls.Add(this.trainModelButton);
            this.Controls.Add(this.modelDataPreviewComboBox);
            this.Controls.Add(this.clearAllButton);
            this.Controls.Add(this.importCSVButton);
            this.Controls.Add(this.functionalityComboBox);
            this.Controls.Add(this.selectFunctionalityLabel);
            this.Name = "MLTrainingSetupForm";
            this.Text = "Machine Learning Training";
            this.trainingAlgorithmGroupBox.ResumeLayout(false);
            this.trainingAlgorithmGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.algorithmParametersListView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label selectFunctionalityLabel;
        private System.Windows.Forms.ComboBox functionalityComboBox;
        private System.Windows.Forms.Button importCSVButton;
        private System.Windows.Forms.Button clearAllButton;
        private System.Windows.Forms.ListView modelDataPreviewComboBox;
        private System.Windows.Forms.Button trainModelButton;
        private System.Windows.Forms.Label trainingResultsLabel;
        private System.Windows.Forms.Button saveCSVButton;
        private System.Windows.Forms.Button applyTrainedModelButton;
        private System.Windows.Forms.Button removeSelectedButton;
        private System.Windows.Forms.GroupBox testPredictionGroupBox;
        private System.Windows.Forms.GroupBox trainingAlgorithmGroupBox;
        private System.Windows.Forms.Label selectAlgorithmLabel;
        private System.Windows.Forms.ComboBox algorithmComboBox;
        private System.Windows.Forms.DataGridView algorithmParametersListView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Parameter;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
    }
}