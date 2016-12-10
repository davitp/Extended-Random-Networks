namespace RandNet
{
    partial class ModelCheckWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelCheckWindow));
            this.modelName = new System.Windows.Forms.Label();
            this.notExactCheckBtn = new System.Windows.Forms.Button();
            this.notExactResult = new System.Windows.Forms.Label();
            this.notExactResultTxt = new System.Windows.Forms.TextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.checkersTab = new System.Windows.Forms.TabControl();
            this.notExactTab = new System.Windows.Forms.TabPage();
            this.notExactDataGrp = new System.Windows.Forms.GroupBox();
            this.notExactBrowseBtn = new System.Windows.Forms.Button();
            this.notExactFilePath = new System.Windows.Forms.Label();
            this.notExactFilePathTxt = new System.Windows.Forms.TextBox();
            this.fromFileRadio = new System.Windows.Forms.RadioButton();
            this.degreesRadio = new System.Windows.Forms.RadioButton();
            this.degrees = new System.Windows.Forms.Label();
            this.degreesTxt = new System.Windows.Forms.TextBox();
            this.exactTab = new System.Windows.Forms.TabPage();
            this.exactCheckBtn = new System.Windows.Forms.Button();
            this.exactResultTxt = new System.Windows.Forms.TextBox();
            this.exactResult = new System.Windows.Forms.Label();
            this.exactDataGrp = new System.Windows.Forms.GroupBox();
            this.exactBrowseBtn = new System.Windows.Forms.Button();
            this.exactFilePath = new System.Windows.Forms.Label();
            this.exactFilePathTxt = new System.Windows.Forms.TextBox();
            this.modelNameTxt = new System.Windows.Forms.TextBox();
            this.checkersTab.SuspendLayout();
            this.notExactTab.SuspendLayout();
            this.notExactDataGrp.SuspendLayout();
            this.exactTab.SuspendLayout();
            this.exactDataGrp.SuspendLayout();
            this.SuspendLayout();
            // 
            // modelName
            // 
            this.modelName.AutoSize = true;
            this.modelName.Location = new System.Drawing.Point(12, 9);
            this.modelName.Name = "modelName";
            this.modelName.Size = new System.Drawing.Size(67, 13);
            this.modelName.TabIndex = 0;
            this.modelName.Text = "Model Name";
            // 
            // notExactCheckBtn
            // 
            this.notExactCheckBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.notExactCheckBtn.Location = new System.Drawing.Point(6, 168);
            this.notExactCheckBtn.Name = "notExactCheckBtn";
            this.notExactCheckBtn.Size = new System.Drawing.Size(196, 30);
            this.notExactCheckBtn.TabIndex = 4;
            this.notExactCheckBtn.Text = "Check";
            this.notExactCheckBtn.UseVisualStyleBackColor = true;
            this.notExactCheckBtn.Click += new System.EventHandler(this.notExactCheckBtn_Click);
            // 
            // notExactResult
            // 
            this.notExactResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.notExactResult.AutoSize = true;
            this.notExactResult.Location = new System.Drawing.Point(258, 177);
            this.notExactResult.Name = "notExactResult";
            this.notExactResult.Size = new System.Drawing.Size(37, 13);
            this.notExactResult.TabIndex = 5;
            this.notExactResult.Text = "Result";
            // 
            // notExactResultTxt
            // 
            this.notExactResultTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.notExactResultTxt.Location = new System.Drawing.Point(301, 174);
            this.notExactResultTxt.Name = "notExactResultTxt";
            this.notExactResultTxt.ReadOnly = true;
            this.notExactResultTxt.Size = new System.Drawing.Size(258, 20);
            this.notExactResultTxt.TabIndex = 6;
            this.notExactResultTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "\"txt files (*.txt)|*.txt|All files (*.*)|*.*\"";
            // 
            // checkersTab
            // 
            this.checkersTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkersTab.Controls.Add(this.notExactTab);
            this.checkersTab.Controls.Add(this.exactTab);
            this.checkersTab.Location = new System.Drawing.Point(12, 51);
            this.checkersTab.Name = "checkersTab";
            this.checkersTab.SelectedIndex = 0;
            this.checkersTab.Size = new System.Drawing.Size(574, 233);
            this.checkersTab.TabIndex = 9;
            // 
            // notExactTab
            // 
            this.notExactTab.Controls.Add(this.notExactDataGrp);
            this.notExactTab.Controls.Add(this.notExactCheckBtn);
            this.notExactTab.Controls.Add(this.notExactResultTxt);
            this.notExactTab.Controls.Add(this.notExactResult);
            this.notExactTab.Location = new System.Drawing.Point(4, 22);
            this.notExactTab.Name = "notExactTab";
            this.notExactTab.Padding = new System.Windows.Forms.Padding(3);
            this.notExactTab.Size = new System.Drawing.Size(566, 207);
            this.notExactTab.TabIndex = 0;
            this.notExactTab.Text = "Not Exact Method";
            this.notExactTab.UseVisualStyleBackColor = true;
            // 
            // notExactDataGrp
            // 
            this.notExactDataGrp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.notExactDataGrp.Controls.Add(this.notExactBrowseBtn);
            this.notExactDataGrp.Controls.Add(this.notExactFilePath);
            this.notExactDataGrp.Controls.Add(this.notExactFilePathTxt);
            this.notExactDataGrp.Controls.Add(this.fromFileRadio);
            this.notExactDataGrp.Controls.Add(this.degreesRadio);
            this.notExactDataGrp.Controls.Add(this.degrees);
            this.notExactDataGrp.Controls.Add(this.degreesTxt);
            this.notExactDataGrp.Location = new System.Drawing.Point(6, 6);
            this.notExactDataGrp.Name = "notExactDataGrp";
            this.notExactDataGrp.Size = new System.Drawing.Size(552, 156);
            this.notExactDataGrp.TabIndex = 8;
            this.notExactDataGrp.TabStop = false;
            this.notExactDataGrp.Text = "Data";
            // 
            // notExactBrowseBtn
            // 
            this.notExactBrowseBtn.Location = new System.Drawing.Point(391, 112);
            this.notExactBrowseBtn.Name = "notExactBrowseBtn";
            this.notExactBrowseBtn.Size = new System.Drawing.Size(142, 23);
            this.notExactBrowseBtn.TabIndex = 6;
            this.notExactBrowseBtn.Text = "Browse";
            this.notExactBrowseBtn.UseVisualStyleBackColor = true;
            this.notExactBrowseBtn.Click += new System.EventHandler(this.notExactBrowseBtn_Click);
            // 
            // notExactFilePath
            // 
            this.notExactFilePath.AutoSize = true;
            this.notExactFilePath.Location = new System.Drawing.Point(99, 118);
            this.notExactFilePath.Name = "notExactFilePath";
            this.notExactFilePath.Size = new System.Drawing.Size(48, 13);
            this.notExactFilePath.TabIndex = 5;
            this.notExactFilePath.Text = "File Path";
            // 
            // notExactFilePathTxt
            // 
            this.notExactFilePathTxt.Location = new System.Drawing.Point(153, 113);
            this.notExactFilePathTxt.Name = "notExactFilePathTxt";
            this.notExactFilePathTxt.Size = new System.Drawing.Size(217, 20);
            this.notExactFilePathTxt.TabIndex = 4;
            // 
            // fromFileRadio
            // 
            this.fromFileRadio.AutoSize = true;
            this.fromFileRadio.Location = new System.Drawing.Point(6, 116);
            this.fromFileRadio.Name = "fromFileRadio";
            this.fromFileRadio.Size = new System.Drawing.Size(67, 17);
            this.fromFileRadio.TabIndex = 1;
            this.fromFileRadio.TabStop = true;
            this.fromFileRadio.Text = "From File";
            this.fromFileRadio.UseVisualStyleBackColor = true;
            this.fromFileRadio.CheckedChanged += new System.EventHandler(this.fromFileRadio_CheckedChanged);
            // 
            // degreesRadio
            // 
            this.degreesRadio.AutoSize = true;
            this.degreesRadio.Location = new System.Drawing.Point(6, 28);
            this.degreesRadio.Name = "degreesRadio";
            this.degreesRadio.Size = new System.Drawing.Size(65, 17);
            this.degreesRadio.TabIndex = 0;
            this.degreesRadio.TabStop = true;
            this.degreesRadio.Text = "Degrees";
            this.degreesRadio.UseVisualStyleBackColor = true;
            this.degreesRadio.CheckedChanged += new System.EventHandler(this.degreesRadio_CheckedChanged);
            // 
            // degrees
            // 
            this.degrees.AutoSize = true;
            this.degrees.Location = new System.Drawing.Point(99, 30);
            this.degrees.Name = "degrees";
            this.degrees.Size = new System.Drawing.Size(134, 13);
            this.degrees.TabIndex = 2;
            this.degrees.Text = "Degrees (saparated with , )";
            // 
            // degreesTxt
            // 
            this.degreesTxt.Location = new System.Drawing.Point(102, 46);
            this.degreesTxt.Multiline = true;
            this.degreesTxt.Name = "degreesTxt";
            this.degreesTxt.Size = new System.Drawing.Size(431, 60);
            this.degreesTxt.TabIndex = 3;
            // 
            // exactTab
            // 
            this.exactTab.Controls.Add(this.exactCheckBtn);
            this.exactTab.Controls.Add(this.exactResultTxt);
            this.exactTab.Controls.Add(this.exactResult);
            this.exactTab.Controls.Add(this.exactDataGrp);
            this.exactTab.Location = new System.Drawing.Point(4, 22);
            this.exactTab.Name = "exactTab";
            this.exactTab.Padding = new System.Windows.Forms.Padding(3);
            this.exactTab.Size = new System.Drawing.Size(566, 207);
            this.exactTab.TabIndex = 1;
            this.exactTab.Text = "Exact Method";
            this.exactTab.UseVisualStyleBackColor = true;
            // 
            // exactCheckBtn
            // 
            this.exactCheckBtn.Location = new System.Drawing.Point(6, 168);
            this.exactCheckBtn.Name = "exactCheckBtn";
            this.exactCheckBtn.Size = new System.Drawing.Size(196, 30);
            this.exactCheckBtn.TabIndex = 10;
            this.exactCheckBtn.Text = "Check";
            this.exactCheckBtn.UseVisualStyleBackColor = true;
            this.exactCheckBtn.Click += new System.EventHandler(this.exactCheckBtn_Click);
            // 
            // exactResultTxt
            // 
            this.exactResultTxt.Location = new System.Drawing.Point(301, 174);
            this.exactResultTxt.Name = "exactResultTxt";
            this.exactResultTxt.ReadOnly = true;
            this.exactResultTxt.Size = new System.Drawing.Size(258, 20);
            this.exactResultTxt.TabIndex = 12;
            this.exactResultTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // exactResult
            // 
            this.exactResult.AutoSize = true;
            this.exactResult.Location = new System.Drawing.Point(258, 177);
            this.exactResult.Name = "exactResult";
            this.exactResult.Size = new System.Drawing.Size(37, 13);
            this.exactResult.TabIndex = 11;
            this.exactResult.Text = "Result";
            // 
            // exactDataGrp
            // 
            this.exactDataGrp.Controls.Add(this.exactBrowseBtn);
            this.exactDataGrp.Controls.Add(this.exactFilePath);
            this.exactDataGrp.Controls.Add(this.exactFilePathTxt);
            this.exactDataGrp.Location = new System.Drawing.Point(6, 6);
            this.exactDataGrp.Name = "exactDataGrp";
            this.exactDataGrp.Size = new System.Drawing.Size(552, 156);
            this.exactDataGrp.TabIndex = 9;
            this.exactDataGrp.TabStop = false;
            this.exactDataGrp.Text = "Data";
            // 
            // exactBrowseBtn
            // 
            this.exactBrowseBtn.Location = new System.Drawing.Point(313, 26);
            this.exactBrowseBtn.Name = "exactBrowseBtn";
            this.exactBrowseBtn.Size = new System.Drawing.Size(142, 23);
            this.exactBrowseBtn.TabIndex = 6;
            this.exactBrowseBtn.Text = "Browse";
            this.exactBrowseBtn.UseVisualStyleBackColor = true;
            this.exactBrowseBtn.Click += new System.EventHandler(this.exactBrowseBtn_Click);
            // 
            // exactFilePath
            // 
            this.exactFilePath.AutoSize = true;
            this.exactFilePath.Location = new System.Drawing.Point(21, 32);
            this.exactFilePath.Name = "exactFilePath";
            this.exactFilePath.Size = new System.Drawing.Size(48, 13);
            this.exactFilePath.TabIndex = 5;
            this.exactFilePath.Text = "File Path";
            // 
            // exactFilePathTxt
            // 
            this.exactFilePathTxt.Location = new System.Drawing.Point(75, 27);
            this.exactFilePathTxt.Name = "exactFilePathTxt";
            this.exactFilePathTxt.Size = new System.Drawing.Size(217, 20);
            this.exactFilePathTxt.TabIndex = 4;
            // 
            // modelNameTxt
            // 
            this.modelNameTxt.Location = new System.Drawing.Point(12, 25);
            this.modelNameTxt.Name = "modelNameTxt";
            this.modelNameTxt.ReadOnly = true;
            this.modelNameTxt.Size = new System.Drawing.Size(171, 20);
            this.modelNameTxt.TabIndex = 10;
            this.modelNameTxt.Text = "Regular Block-Hierarchic";
            // 
            // ModelCheckWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 293);
            this.Controls.Add(this.modelNameTxt);
            this.Controls.Add(this.checkersTab);
            this.Controls.Add(this.modelName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ModelCheckWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Model Check Tool";
            this.Load += new System.EventHandler(this.ModelCheckWindow_Load);
            this.checkersTab.ResumeLayout(false);
            this.notExactTab.ResumeLayout(false);
            this.notExactTab.PerformLayout();
            this.notExactDataGrp.ResumeLayout(false);
            this.notExactDataGrp.PerformLayout();
            this.exactTab.ResumeLayout(false);
            this.exactTab.PerformLayout();
            this.exactDataGrp.ResumeLayout(false);
            this.exactDataGrp.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label modelName;
        private System.Windows.Forms.Button notExactCheckBtn;
        private System.Windows.Forms.Label notExactResult;
        private System.Windows.Forms.TextBox notExactResultTxt;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TabControl checkersTab;
        private System.Windows.Forms.TabPage notExactTab;
        private System.Windows.Forms.TabPage exactTab;
        private System.Windows.Forms.GroupBox notExactDataGrp;
        private System.Windows.Forms.Button notExactBrowseBtn;
        private System.Windows.Forms.Label notExactFilePath;
        private System.Windows.Forms.TextBox notExactFilePathTxt;
        private System.Windows.Forms.RadioButton fromFileRadio;
        private System.Windows.Forms.RadioButton degreesRadio;
        private System.Windows.Forms.Label degrees;
        private System.Windows.Forms.TextBox degreesTxt;
        private System.Windows.Forms.Button exactCheckBtn;
        private System.Windows.Forms.TextBox exactResultTxt;
        private System.Windows.Forms.Label exactResult;
        private System.Windows.Forms.GroupBox exactDataGrp;
        private System.Windows.Forms.Button exactBrowseBtn;
        private System.Windows.Forms.Label exactFilePath;
        private System.Windows.Forms.TextBox exactFilePathTxt;
        private System.Windows.Forms.TextBox modelNameTxt;
    }
}