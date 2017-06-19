namespace RandNet
{
    partial class CreateEditResearchWindow
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
            this.create = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.generalGrp = new System.Windows.Forms.GroupBox();
            this.realizationCountTxt = new System.Windows.Forms.NumericUpDown();
            this.realizationCount = new System.Windows.Forms.Label();
            this.tracingCheck = new System.Windows.Forms.CheckBox();
            this.generationTypeCmb = new System.Windows.Forms.ComboBox();
            this.generationType = new System.Windows.Forms.Label();
            this.storageTypeCmb = new System.Windows.Forms.ComboBox();
            this.storageType = new System.Windows.Forms.Label();
            this.modelTypeCmb = new System.Windows.Forms.ComboBox();
            this.modelType = new System.Windows.Forms.Label();
            this.researchNameTxt = new System.Windows.Forms.TextBox();
            this.researchName = new System.Windows.Forms.Label();
            this.researchTypeTxt = new System.Windows.Forms.TextBox();
            this.researchType = new System.Windows.Forms.Label();
            this.parametersGrp = new System.Windows.Forms.GroupBox();
            this.parametersPanel = new System.Windows.Forms.Panel();
            this.optionsGrp = new System.Windows.Forms.GroupBox();
            this.deselectAll = new System.Windows.Forms.Button();
            this.selectAll = new System.Windows.Forms.Button();
            this.optionsPanel = new System.Windows.Forms.Panel();
            this.checkConnected = new System.Windows.Forms.CheckBox();
            this.generalGrp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.realizationCountTxt)).BeginInit();
            this.parametersGrp.SuspendLayout();
            this.optionsGrp.SuspendLayout();
            this.SuspendLayout();
            // 
            // create
            // 
            this.create.Location = new System.Drawing.Point(699, 397);
            this.create.Name = "create";
            this.create.Size = new System.Drawing.Size(75, 23);
            this.create.TabIndex = 6;
            this.create.UseVisualStyleBackColor = true;
            this.create.Click += new System.EventHandler(this.create_Click);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(780, 397);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 7;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // generalGrp
            // 
            this.generalGrp.Controls.Add(this.checkConnected);
            this.generalGrp.Controls.Add(this.realizationCountTxt);
            this.generalGrp.Controls.Add(this.realizationCount);
            this.generalGrp.Controls.Add(this.tracingCheck);
            this.generalGrp.Controls.Add(this.generationTypeCmb);
            this.generalGrp.Controls.Add(this.generationType);
            this.generalGrp.Controls.Add(this.storageTypeCmb);
            this.generalGrp.Controls.Add(this.storageType);
            this.generalGrp.Controls.Add(this.modelTypeCmb);
            this.generalGrp.Controls.Add(this.modelType);
            this.generalGrp.Controls.Add(this.researchNameTxt);
            this.generalGrp.Controls.Add(this.researchName);
            this.generalGrp.Controls.Add(this.researchTypeTxt);
            this.generalGrp.Controls.Add(this.researchType);
            this.generalGrp.Location = new System.Drawing.Point(12, 12);
            this.generalGrp.Name = "generalGrp";
            this.generalGrp.Size = new System.Drawing.Size(846, 93);
            this.generalGrp.TabIndex = 2;
            this.generalGrp.TabStop = false;
            this.generalGrp.Text = "General";
            // 
            // realizationCountTxt
            // 
            this.realizationCountTxt.Location = new System.Drawing.Point(555, 56);
            this.realizationCountTxt.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.realizationCountTxt.Name = "realizationCountTxt";
            this.realizationCountTxt.Size = new System.Drawing.Size(121, 20);
            this.realizationCountTxt.TabIndex = 5;
            this.realizationCountTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.realizationCountTxt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // realizationCount
            // 
            this.realizationCount.AutoSize = true;
            this.realizationCount.Location = new System.Drawing.Point(459, 60);
            this.realizationCount.Name = "realizationCount";
            this.realizationCount.Size = new System.Drawing.Size(93, 13);
            this.realizationCount.TabIndex = 0;
            this.realizationCount.Text = "Realization Count:";
            // 
            // tracingCheck
            // 
            this.tracingCheck.AutoSize = true;
            this.tracingCheck.Location = new System.Drawing.Point(712, 24);
            this.tracingCheck.Name = "tracingCheck";
            this.tracingCheck.Size = new System.Drawing.Size(62, 17);
            this.tracingCheck.TabIndex = 4;
            this.tracingCheck.Text = "Tracing";
            this.tracingCheck.UseVisualStyleBackColor = true;
            // 
            // generationTypeCmb
            // 
            this.generationTypeCmb.FormattingEnabled = true;
            this.generationTypeCmb.Location = new System.Drawing.Point(555, 22);
            this.generationTypeCmb.Name = "generationTypeCmb";
            this.generationTypeCmb.Size = new System.Drawing.Size(121, 21);
            this.generationTypeCmb.TabIndex = 3;
            this.generationTypeCmb.SelectedIndexChanged += new System.EventHandler(this.generationTypeCmb_SelectedIndexChanged);
            // 
            // generationType
            // 
            this.generationType.AutoSize = true;
            this.generationType.Location = new System.Drawing.Point(459, 25);
            this.generationType.Name = "generationType";
            this.generationType.Size = new System.Drawing.Size(89, 13);
            this.generationType.TabIndex = 0;
            this.generationType.Text = "Generation Type:";
            // 
            // storageTypeCmb
            // 
            this.storageTypeCmb.FormattingEnabled = true;
            this.storageTypeCmb.Location = new System.Drawing.Point(305, 56);
            this.storageTypeCmb.Name = "storageTypeCmb";
            this.storageTypeCmb.Size = new System.Drawing.Size(121, 21);
            this.storageTypeCmb.TabIndex = 2;
            // 
            // storageType
            // 
            this.storageType.AutoSize = true;
            this.storageType.Location = new System.Drawing.Point(230, 60);
            this.storageType.Name = "storageType";
            this.storageType.Size = new System.Drawing.Size(74, 13);
            this.storageType.TabIndex = 0;
            this.storageType.Text = "Storage Type:";
            // 
            // modelTypeCmb
            // 
            this.modelTypeCmb.FormattingEnabled = true;
            this.modelTypeCmb.Location = new System.Drawing.Point(305, 22);
            this.modelTypeCmb.Name = "modelTypeCmb";
            this.modelTypeCmb.Size = new System.Drawing.Size(121, 21);
            this.modelTypeCmb.TabIndex = 1;
            this.modelTypeCmb.SelectedIndexChanged += new System.EventHandler(this.modelTypeCmb_SelectedIndexChanged);
            // 
            // modelType
            // 
            this.modelType.AutoSize = true;
            this.modelType.Location = new System.Drawing.Point(230, 26);
            this.modelType.Name = "modelType";
            this.modelType.Size = new System.Drawing.Size(66, 13);
            this.modelType.TabIndex = 0;
            this.modelType.Text = "Model Type:";
            // 
            // researchNameTxt
            // 
            this.researchNameTxt.Location = new System.Drawing.Point(99, 57);
            this.researchNameTxt.Name = "researchNameTxt";
            this.researchNameTxt.Size = new System.Drawing.Size(100, 20);
            this.researchNameTxt.TabIndex = 0;
            this.researchNameTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // researchName
            // 
            this.researchName.AutoSize = true;
            this.researchName.Location = new System.Drawing.Point(10, 60);
            this.researchName.Name = "researchName";
            this.researchName.Size = new System.Drawing.Size(87, 13);
            this.researchName.TabIndex = 0;
            this.researchName.Text = "Research Name:";
            // 
            // researchTypeTxt
            // 
            this.researchTypeTxt.Location = new System.Drawing.Point(99, 23);
            this.researchTypeTxt.Name = "researchTypeTxt";
            this.researchTypeTxt.ReadOnly = true;
            this.researchTypeTxt.Size = new System.Drawing.Size(100, 20);
            this.researchTypeTxt.TabIndex = 1;
            this.researchTypeTxt.TabStop = false;
            this.researchTypeTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // researchType
            // 
            this.researchType.AutoSize = true;
            this.researchType.Location = new System.Drawing.Point(10, 26);
            this.researchType.Name = "researchType";
            this.researchType.Size = new System.Drawing.Size(83, 13);
            this.researchType.TabIndex = 0;
            this.researchType.Text = "Research Type:";
            // 
            // parametersGrp
            // 
            this.parametersGrp.Controls.Add(this.parametersPanel);
            this.parametersGrp.Location = new System.Drawing.Point(12, 111);
            this.parametersGrp.Name = "parametersGrp";
            this.parametersGrp.Size = new System.Drawing.Size(390, 276);
            this.parametersGrp.TabIndex = 3;
            this.parametersGrp.TabStop = false;
            this.parametersGrp.Text = "Parameters";
            // 
            // parametersPanel
            // 
            this.parametersPanel.AutoScroll = true;
            this.parametersPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parametersPanel.Location = new System.Drawing.Point(3, 16);
            this.parametersPanel.Name = "parametersPanel";
            this.parametersPanel.Size = new System.Drawing.Size(384, 257);
            this.parametersPanel.TabIndex = 0;
            // 
            // optionsGrp
            // 
            this.optionsGrp.Controls.Add(this.deselectAll);
            this.optionsGrp.Controls.Add(this.selectAll);
            this.optionsGrp.Controls.Add(this.optionsPanel);
            this.optionsGrp.Location = new System.Drawing.Point(417, 111);
            this.optionsGrp.Name = "optionsGrp";
            this.optionsGrp.Size = new System.Drawing.Size(441, 276);
            this.optionsGrp.TabIndex = 4;
            this.optionsGrp.TabStop = false;
            this.optionsGrp.Text = "Analyze Options";
            // 
            // deselectAll
            // 
            this.deselectAll.Location = new System.Drawing.Point(87, 247);
            this.deselectAll.Name = "deselectAll";
            this.deselectAll.Size = new System.Drawing.Size(75, 23);
            this.deselectAll.TabIndex = 3;
            this.deselectAll.Text = "Deselect All";
            this.deselectAll.UseVisualStyleBackColor = true;
            this.deselectAll.Click += new System.EventHandler(this.deselectAll_Click);
            // 
            // selectAll
            // 
            this.selectAll.Location = new System.Drawing.Point(6, 247);
            this.selectAll.Name = "selectAll";
            this.selectAll.Size = new System.Drawing.Size(75, 23);
            this.selectAll.TabIndex = 2;
            this.selectAll.Text = "Select All";
            this.selectAll.UseVisualStyleBackColor = true;
            this.selectAll.Click += new System.EventHandler(this.selectAll_Click);
            // 
            // optionsPanel
            // 
            this.optionsPanel.AutoScroll = true;
            this.optionsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.optionsPanel.Location = new System.Drawing.Point(3, 16);
            this.optionsPanel.Name = "optionsPanel";
            this.optionsPanel.Size = new System.Drawing.Size(435, 225);
            this.optionsPanel.TabIndex = 0;
            // 
            // checkConnected
            // 
            this.checkConnected.AutoSize = true;
            this.checkConnected.Location = new System.Drawing.Point(712, 57);
            this.checkConnected.Name = "checkConnected";
            this.checkConnected.Size = new System.Drawing.Size(112, 17);
            this.checkConnected.TabIndex = 6;
            this.checkConnected.Text = "Check Connected";
            this.checkConnected.UseVisualStyleBackColor = true;
            // 
            // CreateEditResearchWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(870, 432);
            this.Controls.Add(this.optionsGrp);
            this.Controls.Add(this.parametersGrp);
            this.Controls.Add(this.generalGrp);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.create);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateEditResearchWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.CreateEditResearchWindow_Load);
            this.generalGrp.ResumeLayout(false);
            this.generalGrp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.realizationCountTxt)).EndInit();
            this.parametersGrp.ResumeLayout(false);
            this.optionsGrp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button create;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.GroupBox generalGrp;
        private System.Windows.Forms.Label realizationCount;
        private System.Windows.Forms.CheckBox tracingCheck;
        private System.Windows.Forms.ComboBox generationTypeCmb;
        private System.Windows.Forms.Label generationType;
        private System.Windows.Forms.ComboBox storageTypeCmb;
        private System.Windows.Forms.Label storageType;
        private System.Windows.Forms.ComboBox modelTypeCmb;
        private System.Windows.Forms.Label modelType;
        private System.Windows.Forms.TextBox researchNameTxt;
        private System.Windows.Forms.Label researchName;
        private System.Windows.Forms.TextBox researchTypeTxt;
        private System.Windows.Forms.Label researchType;
        private System.Windows.Forms.GroupBox parametersGrp;
        private System.Windows.Forms.NumericUpDown realizationCountTxt;
        private System.Windows.Forms.Panel parametersPanel;
        private System.Windows.Forms.GroupBox optionsGrp;
        private System.Windows.Forms.Button deselectAll;
        private System.Windows.Forms.Button selectAll;
        private System.Windows.Forms.Panel optionsPanel;
        private System.Windows.Forms.CheckBox checkConnected;
    }
}