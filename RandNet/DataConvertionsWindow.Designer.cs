namespace RandNet
{
    partial class DataConvertionsWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataConvertionsWindow));
            this.sourceStorageType = new System.Windows.Forms.Label();
            this.sourceStorageTypeCmb = new System.Windows.Forms.ComboBox();
            this.sourceBrowse = new System.Windows.Forms.Button();
            this.sourceResultTxt = new System.Windows.Forms.TextBox();
            this.sourceResult = new System.Windows.Forms.Label();
            this.targetResult = new System.Windows.Forms.Label();
            this.targetBrowse = new System.Windows.Forms.Button();
            this.targetResultTxt = new System.Windows.Forms.TextBox();
            this.targetStorageCmb = new System.Windows.Forms.ComboBox();
            this.targetStorageType = new System.Windows.Forms.Label();
            this.convert = new System.Windows.Forms.Button();
            this.sourceBrowserDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.targetBrowserDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // sourceStorageType
            // 
            this.sourceStorageType.AutoSize = true;
            this.sourceStorageType.Location = new System.Drawing.Point(12, 19);
            this.sourceStorageType.Name = "sourceStorageType";
            this.sourceStorageType.Size = new System.Drawing.Size(111, 13);
            this.sourceStorageType.TabIndex = 0;
            this.sourceStorageType.Text = "Source Storage Type:";
            // 
            // sourceStorageTypeCmb
            // 
            this.sourceStorageTypeCmb.FormattingEnabled = true;
            this.sourceStorageTypeCmb.Location = new System.Drawing.Point(129, 16);
            this.sourceStorageTypeCmb.Name = "sourceStorageTypeCmb";
            this.sourceStorageTypeCmb.Size = new System.Drawing.Size(121, 21);
            this.sourceStorageTypeCmb.TabIndex = 1;
            // 
            // sourceBrowse
            // 
            this.sourceBrowse.Location = new System.Drawing.Point(424, 52);
            this.sourceBrowse.Name = "sourceBrowse";
            this.sourceBrowse.Size = new System.Drawing.Size(75, 20);
            this.sourceBrowse.TabIndex = 4;
            this.sourceBrowse.Text = "Browse...";
            this.sourceBrowse.UseVisualStyleBackColor = true;
            this.sourceBrowse.Click += new System.EventHandler(this.sourceBrowse_Click);
            // 
            // sourceResultTxt
            // 
            this.sourceResultTxt.Location = new System.Drawing.Point(129, 52);
            this.sourceResultTxt.Name = "sourceResultTxt";
            this.sourceResultTxt.Size = new System.Drawing.Size(289, 20);
            this.sourceResultTxt.TabIndex = 3;
            this.sourceResultTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // sourceResult
            // 
            this.sourceResult.AutoSize = true;
            this.sourceResult.Location = new System.Drawing.Point(46, 55);
            this.sourceResult.Name = "sourceResult";
            this.sourceResult.Size = new System.Drawing.Size(77, 13);
            this.sourceResult.TabIndex = 5;
            this.sourceResult.Text = "Source Result:";
            // 
            // targetResult
            // 
            this.targetResult.AutoSize = true;
            this.targetResult.Location = new System.Drawing.Point(49, 138);
            this.targetResult.Name = "targetResult";
            this.targetResult.Size = new System.Drawing.Size(74, 13);
            this.targetResult.TabIndex = 10;
            this.targetResult.Text = "Target Result:";
            // 
            // targetBrowse
            // 
            this.targetBrowse.Location = new System.Drawing.Point(424, 135);
            this.targetBrowse.Name = "targetBrowse";
            this.targetBrowse.Size = new System.Drawing.Size(75, 20);
            this.targetBrowse.TabIndex = 9;
            this.targetBrowse.Text = "Browse...";
            this.targetBrowse.UseVisualStyleBackColor = true;
            this.targetBrowse.Click += new System.EventHandler(this.targetBrowse_Click);
            // 
            // targetResultTxt
            // 
            this.targetResultTxt.Location = new System.Drawing.Point(129, 135);
            this.targetResultTxt.Name = "targetResultTxt";
            this.targetResultTxt.Size = new System.Drawing.Size(289, 20);
            this.targetResultTxt.TabIndex = 8;
            this.targetResultTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // targetStorageCmb
            // 
            this.targetStorageCmb.FormattingEnabled = true;
            this.targetStorageCmb.Location = new System.Drawing.Point(129, 99);
            this.targetStorageCmb.Name = "targetStorageCmb";
            this.targetStorageCmb.Size = new System.Drawing.Size(121, 21);
            this.targetStorageCmb.TabIndex = 7;
            // 
            // targetStorageType
            // 
            this.targetStorageType.AutoSize = true;
            this.targetStorageType.Location = new System.Drawing.Point(18, 102);
            this.targetStorageType.Name = "targetStorageType";
            this.targetStorageType.Size = new System.Drawing.Size(105, 13);
            this.targetStorageType.TabIndex = 6;
            this.targetStorageType.Text = "TargetStorage Type:";
            // 
            // convert
            // 
            this.convert.Location = new System.Drawing.Point(424, 172);
            this.convert.Name = "convert";
            this.convert.Size = new System.Drawing.Size(75, 23);
            this.convert.TabIndex = 11;
            this.convert.Text = "Convert";
            this.convert.UseVisualStyleBackColor = true;
            this.convert.Click += new System.EventHandler(this.convert_Click);
            // 
            // DataConvertionsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 207);
            this.Controls.Add(this.convert);
            this.Controls.Add(this.targetResult);
            this.Controls.Add(this.targetBrowse);
            this.Controls.Add(this.targetResultTxt);
            this.Controls.Add(this.targetStorageCmb);
            this.Controls.Add(this.targetStorageType);
            this.Controls.Add(this.sourceResult);
            this.Controls.Add(this.sourceBrowse);
            this.Controls.Add(this.sourceResultTxt);
            this.Controls.Add(this.sourceStorageTypeCmb);
            this.Controls.Add(this.sourceStorageType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DataConvertionsWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data Convertions Tool";
            this.Load += new System.EventHandler(this.DataConvertionsWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label sourceStorageType;
        private System.Windows.Forms.ComboBox sourceStorageTypeCmb;
        private System.Windows.Forms.Button sourceBrowse;
        private System.Windows.Forms.TextBox sourceResultTxt;
        private System.Windows.Forms.Label sourceResult;
        private System.Windows.Forms.Label targetResult;
        private System.Windows.Forms.Button targetBrowse;
        private System.Windows.Forms.TextBox targetResultTxt;
        private System.Windows.Forms.ComboBox targetStorageCmb;
        private System.Windows.Forms.Label targetStorageType;
        private System.Windows.Forms.Button convert;
        private System.Windows.Forms.FolderBrowserDialog sourceBrowserDlg;
        private System.Windows.Forms.FolderBrowserDialog targetBrowserDlg;
    }
}