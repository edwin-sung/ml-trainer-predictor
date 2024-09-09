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
            this.resultsGroupBox = new System.Windows.Forms.GroupBox();
            this.trainingTimeLabel = new System.Windows.Forms.Label();
            this.trainingTimeTextBox = new System.Windows.Forms.TextBox();
            this.optimisableObjectiveGroupBox = new System.Windows.Forms.GroupBox();
            this.rSquaredErrorRadioButton = new System.Windows.Forms.RadioButton();
            this.rootMeanSquaredRadioButton = new System.Windows.Forms.RadioButton();
            this.meanSquaredErrorRadioButton = new System.Windows.Forms.RadioButton();
            this.startTrainingButton = new System.Windows.Forms.Button();
            this.setupGroupBox.SuspendLayout();
            this.optimisableObjectiveGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // setupGroupBox
            // 
            this.setupGroupBox.Controls.Add(this.startTrainingButton);
            this.setupGroupBox.Controls.Add(this.optimisableObjectiveGroupBox);
            this.setupGroupBox.Controls.Add(this.trainingTimeTextBox);
            this.setupGroupBox.Controls.Add(this.trainingTimeLabel);
            this.setupGroupBox.Location = new System.Drawing.Point(13, 13);
            this.setupGroupBox.Name = "setupGroupBox";
            this.setupGroupBox.Size = new System.Drawing.Size(386, 336);
            this.setupGroupBox.TabIndex = 0;
            this.setupGroupBox.TabStop = false;
            this.setupGroupBox.Text = "Auto-Select Training Algorithm Setup";
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
            // trainingTimeLabel
            // 
            this.trainingTimeLabel.AutoSize = true;
            this.trainingTimeLabel.Location = new System.Drawing.Point(7, 20);
            this.trainingTimeLabel.Name = "trainingTimeLabel";
            this.trainingTimeLabel.Size = new System.Drawing.Size(100, 13);
            this.trainingTimeLabel.TabIndex = 0;
            this.trainingTimeLabel.Text = "Training Time (sec.)";
            // 
            // trainingTimeTextBox
            // 
            this.trainingTimeTextBox.Location = new System.Drawing.Point(108, 17);
            this.trainingTimeTextBox.Name = "trainingTimeTextBox";
            this.trainingTimeTextBox.Size = new System.Drawing.Size(76, 20);
            this.trainingTimeTextBox.TabIndex = 1;
            this.trainingTimeTextBox.Text = "60";
            // 
            // optimisableObjectiveGroupBox
            // 
            this.optimisableObjectiveGroupBox.Controls.Add(this.meanSquaredErrorRadioButton);
            this.optimisableObjectiveGroupBox.Controls.Add(this.rootMeanSquaredRadioButton);
            this.optimisableObjectiveGroupBox.Controls.Add(this.rSquaredErrorRadioButton);
            this.optimisableObjectiveGroupBox.Location = new System.Drawing.Point(6, 43);
            this.optimisableObjectiveGroupBox.Name = "optimisableObjectiveGroupBox";
            this.optimisableObjectiveGroupBox.Size = new System.Drawing.Size(374, 110);
            this.optimisableObjectiveGroupBox.TabIndex = 3;
            this.optimisableObjectiveGroupBox.TabStop = false;
            this.optimisableObjectiveGroupBox.Text = "Optimisation Objective";
            // 
            // rSquaredErrorRadioButton
            // 
            this.rSquaredErrorRadioButton.AutoSize = true;
            this.rSquaredErrorRadioButton.Location = new System.Drawing.Point(7, 29);
            this.rSquaredErrorRadioButton.Name = "rSquaredErrorRadioButton";
            this.rSquaredErrorRadioButton.Size = new System.Drawing.Size(101, 17);
            this.rSquaredErrorRadioButton.TabIndex = 0;
            this.rSquaredErrorRadioButton.TabStop = true;
            this.rSquaredErrorRadioButton.Text = "R-Squared Error";
            this.rSquaredErrorRadioButton.UseVisualStyleBackColor = true;
            // 
            // rootMeanSquaredRadioButton
            // 
            this.rootMeanSquaredRadioButton.AutoSize = true;
            this.rootMeanSquaredRadioButton.Location = new System.Drawing.Point(7, 52);
            this.rootMeanSquaredRadioButton.Name = "rootMeanSquaredRadioButton";
            this.rootMeanSquaredRadioButton.Size = new System.Drawing.Size(146, 17);
            this.rootMeanSquaredRadioButton.TabIndex = 1;
            this.rootMeanSquaredRadioButton.TabStop = true;
            this.rootMeanSquaredRadioButton.Text = "Root-Mean-Squared Error";
            this.rootMeanSquaredRadioButton.UseVisualStyleBackColor = true;
            // 
            // meanSquaredErrorRadioButton
            // 
            this.meanSquaredErrorRadioButton.AutoSize = true;
            this.meanSquaredErrorRadioButton.Location = new System.Drawing.Point(7, 75);
            this.meanSquaredErrorRadioButton.Name = "meanSquaredErrorRadioButton";
            this.meanSquaredErrorRadioButton.Size = new System.Drawing.Size(120, 17);
            this.meanSquaredErrorRadioButton.TabIndex = 2;
            this.meanSquaredErrorRadioButton.TabStop = true;
            this.meanSquaredErrorRadioButton.Text = "Mean-Squared Error";
            this.meanSquaredErrorRadioButton.UseVisualStyleBackColor = true;
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
            this.optimisableObjectiveGroupBox.ResumeLayout(false);
            this.optimisableObjectiveGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox setupGroupBox;
        private System.Windows.Forms.GroupBox resultsGroupBox;
        private System.Windows.Forms.TextBox trainingTimeTextBox;
        private System.Windows.Forms.Label trainingTimeLabel;
        private System.Windows.Forms.GroupBox optimisableObjectiveGroupBox;
        private System.Windows.Forms.RadioButton meanSquaredErrorRadioButton;
        private System.Windows.Forms.RadioButton rootMeanSquaredRadioButton;
        private System.Windows.Forms.RadioButton rSquaredErrorRadioButton;
        private System.Windows.Forms.Button startTrainingButton;
    }
}