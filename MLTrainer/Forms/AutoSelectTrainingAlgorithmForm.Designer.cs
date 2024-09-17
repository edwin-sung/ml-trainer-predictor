namespace MLTrainer.Forms
{
    partial class AutoSelectTrainingAlgorithmForm
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
            this.setupGroupBox = new System.Windows.Forms.GroupBox();
            this.startTrainingButton = new System.Windows.Forms.Button();
            this.trainingTimeTextBox = new System.Windows.Forms.TextBox();
            this.trainingTimeLabel = new System.Windows.Forms.Label();
            this.resultsGroupBox = new System.Windows.Forms.GroupBox();
            this.optimisableObjectiveComboBox = new System.Windows.Forms.ComboBox();
            this.optimisableObjectiveLabel = new System.Windows.Forms.Label();
            this.setupGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // setupGroupBox
            // 
            this.setupGroupBox.Controls.Add(this.optimisableObjectiveLabel);
            this.setupGroupBox.Controls.Add(this.optimisableObjectiveComboBox);
            this.setupGroupBox.Controls.Add(this.startTrainingButton);
            this.setupGroupBox.Controls.Add(this.trainingTimeTextBox);
            this.setupGroupBox.Controls.Add(this.trainingTimeLabel);
            this.setupGroupBox.Location = new System.Drawing.Point(13, 13);
            this.setupGroupBox.Name = "setupGroupBox";
            this.setupGroupBox.Size = new System.Drawing.Size(386, 336);
            this.setupGroupBox.TabIndex = 0;
            this.setupGroupBox.TabStop = false;
            this.setupGroupBox.Text = "Auto-Select Training Algorithm Setup";
            // 
            // startTrainingButton
            // 
            this.startTrainingButton.Location = new System.Drawing.Point(13, 300);
            this.startTrainingButton.Name = "startTrainingButton";
            this.startTrainingButton.Size = new System.Drawing.Size(75, 23);
            this.startTrainingButton.TabIndex = 4;
            this.startTrainingButton.Text = "Start";
            this.startTrainingButton.UseVisualStyleBackColor = true;
            // 
            // trainingTimeTextBox
            // 
            this.trainingTimeTextBox.Location = new System.Drawing.Point(108, 17);
            this.trainingTimeTextBox.Name = "trainingTimeTextBox";
            this.trainingTimeTextBox.Size = new System.Drawing.Size(76, 20);
            this.trainingTimeTextBox.TabIndex = 1;
            this.trainingTimeTextBox.Text = "60";
            // 
            // trainingTimeLabel
            // 
            this.trainingTimeLabel.AutoSize = true;
            this.trainingTimeLabel.Location = new System.Drawing.Point(7, 20);
            this.trainingTimeLabel.Name = "trainingTimeLabel";
            this.trainingTimeLabel.Size = new System.Drawing.Size(100, 13);
            this.trainingTimeLabel.TabIndex = 0;
            this.trainingTimeLabel.Text = "Training Time (sec.)";
            // 
            // resultsGroupBox
            // 
            this.resultsGroupBox.Location = new System.Drawing.Point(405, 13);
            this.resultsGroupBox.Name = "resultsGroupBox";
            this.resultsGroupBox.Size = new System.Drawing.Size(386, 336);
            this.resultsGroupBox.TabIndex = 1;
            this.resultsGroupBox.TabStop = false;
            this.resultsGroupBox.Text = "Auto-Selection Results";
            // 
            // optimisableObjectiveComboBox
            // 
            this.optimisableObjectiveComboBox.FormattingEnabled = true;
            this.optimisableObjectiveComboBox.Location = new System.Drawing.Point(123, 36);
            this.optimisableObjectiveComboBox.Name = "optimisableObjectiveComboBox";
            this.optimisableObjectiveComboBox.Size = new System.Drawing.Size(239, 21);
            this.optimisableObjectiveComboBox.TabIndex = 5;
            // 
            // optimisableObjectiveLabel
            // 
            this.optimisableObjectiveLabel.AutoSize = true;
            this.optimisableObjectiveLabel.Location = new System.Drawing.Point(10, 40);
            this.optimisableObjectiveLabel.Name = "optimisableObjectiveLabel";
            this.optimisableObjectiveLabel.Size = new System.Drawing.Size(109, 13);
            this.optimisableObjectiveLabel.TabIndex = 6;
            this.optimisableObjectiveLabel.Text = "Optimisable Objective";
            // 
            // AutoSelectTrainingAlgorithmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.resultsGroupBox);
            this.Controls.Add(this.setupGroupBox);
            this.Name = "AutoSelectTrainingAlgorithmForm";
            this.Text = "Auto-Select Training AlgorithmForm ";
            this.setupGroupBox.ResumeLayout(false);
            this.setupGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox setupGroupBox;
        private System.Windows.Forms.GroupBox resultsGroupBox;
        private System.Windows.Forms.TextBox trainingTimeTextBox;
        private System.Windows.Forms.Label trainingTimeLabel;
        private System.Windows.Forms.Button startTrainingButton;
        private System.Windows.Forms.Label optimisableObjectiveLabel;
        private System.Windows.Forms.ComboBox optimisableObjectiveComboBox;
    }
}