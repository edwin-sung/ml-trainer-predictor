namespace MLTrainer.Forms
{
    partial class DynamicObjectSchemaSetupForm
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
            this.schemaDataGridView = new System.Windows.Forms.DataGridView();
            this.configureSchemaLabel = new System.Windows.Forms.Label();
            this.SchemaName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SchemaType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.SchemaLabel = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.schemaDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // schemaDataGridView
            // 
            this.schemaDataGridView.AllowUserToAddRows = false;
            this.schemaDataGridView.AllowUserToDeleteRows = false;
            this.schemaDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.schemaDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SchemaName,
            this.SchemaType,
            this.SchemaLabel});
            this.schemaDataGridView.Location = new System.Drawing.Point(26, 54);
            this.schemaDataGridView.Name = "schemaDataGridView";
            this.schemaDataGridView.Size = new System.Drawing.Size(346, 150);
            this.schemaDataGridView.TabIndex = 0;
            this.schemaDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.schemaDataGridView_CellContentClick);
            // 
            // configureSchemaLabel
            // 
            this.configureSchemaLabel.AutoSize = true;
            this.configureSchemaLabel.Location = new System.Drawing.Point(26, 22);
            this.configureSchemaLabel.Name = "configureSchemaLabel";
            this.configureSchemaLabel.Size = new System.Drawing.Size(236, 13);
            this.configureSchemaLabel.TabIndex = 1;
            this.configureSchemaLabel.Text = "Please use the table below to define the schema";
            // 
            // Name
            // 
            this.SchemaName.HeaderText = "Name";
            this.SchemaName.Name = "Name";
            this.SchemaName.ReadOnly = true;
            // 
            // Type
            // 
            this.SchemaType.HeaderText = "Type";
            this.SchemaType.Name = "Type";
            // 
            // Label
            // 
            this.SchemaLabel.HeaderText = "Label";
            this.SchemaLabel.Name = "Label";
            // 
            // DynamicObjectSchemaSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.configureSchemaLabel);
            this.Controls.Add(this.schemaDataGridView);
            this.Name = "DynamicObjectSchemaSetupForm";
            this.Text = "Dynamic Object Schema Setup";
            ((System.ComponentModel.ISupportInitialize)(this.schemaDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView schemaDataGridView;
        private System.Windows.Forms.Label configureSchemaLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn SchemaName;
        private System.Windows.Forms.DataGridViewComboBoxColumn SchemaType;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SchemaLabel;
    }
}