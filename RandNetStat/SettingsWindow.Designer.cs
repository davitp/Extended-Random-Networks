namespace RandNetStat
{
    partial class SettingsWindow
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
            this.dataStorageGrp = new System.Windows.Forms.GroupBox();
            this.excelStorageBrowseButton = new System.Windows.Forms.Button();
            this.excelStorageDirectoryTxt = new System.Windows.Forms.TextBox();
            this.excelStorageDirectory = new System.Windows.Forms.Label();
            this.excelStorageRadio = new System.Windows.Forms.RadioButton();
            this.txtStorageRadio = new System.Windows.Forms.RadioButton();
            this.xmlStorageRadio = new System.Windows.Forms.RadioButton();
            this.txtStorageBrowseButton = new System.Windows.Forms.Button();
            this.xmlStorageBrowseButton = new System.Windows.Forms.Button();
            this.txtStorageDirectoryTxt = new System.Windows.Forms.TextBox();
            this.xmlStorageDirectoryTxt = new System.Windows.Forms.TextBox();
            this.txtStorageDirectory = new System.Windows.Forms.Label();
            this.xmlStorageDirectory = new System.Windows.Forms.Label();
            this.SaveSettingsButton = new System.Windows.Forms.Button();
            this.CancelSettingsButton = new System.Windows.Forms.Button();
            this.browserDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.dataStorageGrp.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataStorageGrp
            // 
            this.dataStorageGrp.Controls.Add(this.excelStorageBrowseButton);
            this.dataStorageGrp.Controls.Add(this.excelStorageDirectoryTxt);
            this.dataStorageGrp.Controls.Add(this.excelStorageDirectory);
            this.dataStorageGrp.Controls.Add(this.excelStorageRadio);
            this.dataStorageGrp.Controls.Add(this.txtStorageRadio);
            this.dataStorageGrp.Controls.Add(this.xmlStorageRadio);
            this.dataStorageGrp.Controls.Add(this.txtStorageBrowseButton);
            this.dataStorageGrp.Controls.Add(this.xmlStorageBrowseButton);
            this.dataStorageGrp.Controls.Add(this.txtStorageDirectoryTxt);
            this.dataStorageGrp.Controls.Add(this.xmlStorageDirectoryTxt);
            this.dataStorageGrp.Controls.Add(this.txtStorageDirectory);
            this.dataStorageGrp.Controls.Add(this.xmlStorageDirectory);
            this.dataStorageGrp.Location = new System.Drawing.Point(12, 12);
            this.dataStorageGrp.Name = "dataStorageGrp";
            this.dataStorageGrp.Size = new System.Drawing.Size(590, 177);
            this.dataStorageGrp.TabIndex = 4;
            this.dataStorageGrp.TabStop = false;
            this.dataStorageGrp.Text = "Data Storage";
            // 
            // excelStorageBrowseButton
            // 
            this.excelStorageBrowseButton.Location = new System.Drawing.Point(509, 139);
            this.excelStorageBrowseButton.Name = "excelStorageBrowseButton";
            this.excelStorageBrowseButton.Size = new System.Drawing.Size(75, 20);
            this.excelStorageBrowseButton.TabIndex = 13;
            this.excelStorageBrowseButton.Text = "Browse...";
            this.excelStorageBrowseButton.UseVisualStyleBackColor = true;
            this.excelStorageBrowseButton.Click += new System.EventHandler(this.excelStorageBrowseButton_Click);
            // 
            // excelStorageDirectoryTxt
            // 
            this.excelStorageDirectoryTxt.Location = new System.Drawing.Point(114, 139);
            this.excelStorageDirectoryTxt.Name = "excelStorageDirectoryTxt";
            this.excelStorageDirectoryTxt.Size = new System.Drawing.Size(388, 20);
            this.excelStorageDirectoryTxt.TabIndex = 12;
            // 
            // excelStorageDirectory
            // 
            this.excelStorageDirectory.AutoSize = true;
            this.excelStorageDirectory.Location = new System.Drawing.Point(39, 142);
            this.excelStorageDirectory.Name = "excelStorageDirectory";
            this.excelStorageDirectory.Size = new System.Drawing.Size(69, 13);
            this.excelStorageDirectory.TabIndex = 11;
            this.excelStorageDirectory.Text = "File directory:";
            // 
            // excelStorageRadio
            // 
            this.excelStorageRadio.AutoSize = true;
            this.excelStorageRadio.Location = new System.Drawing.Point(10, 122);
            this.excelStorageRadio.Name = "excelStorageRadio";
            this.excelStorageRadio.Size = new System.Drawing.Size(91, 17);
            this.excelStorageRadio.TabIndex = 10;
            this.excelStorageRadio.TabStop = true;
            this.excelStorageRadio.Text = "Excel Storage";
            this.excelStorageRadio.UseVisualStyleBackColor = true;
            this.excelStorageRadio.CheckedChanged += new System.EventHandler(this.storageRadio_CheckedChanged);
            // 
            // txtStorageRadio
            // 
            this.txtStorageRadio.AutoSize = true;
            this.txtStorageRadio.Location = new System.Drawing.Point(10, 70);
            this.txtStorageRadio.Name = "txtStorageRadio";
            this.txtStorageRadio.Size = new System.Drawing.Size(86, 17);
            this.txtStorageRadio.TabIndex = 5;
            this.txtStorageRadio.TabStop = true;
            this.txtStorageRadio.Text = "TXT Storage";
            this.txtStorageRadio.UseVisualStyleBackColor = true;
            this.txtStorageRadio.CheckedChanged += new System.EventHandler(this.storageRadio_CheckedChanged);
            // 
            // xmlStorageRadio
            // 
            this.xmlStorageRadio.AutoSize = true;
            this.xmlStorageRadio.Location = new System.Drawing.Point(10, 19);
            this.xmlStorageRadio.Name = "xmlStorageRadio";
            this.xmlStorageRadio.Size = new System.Drawing.Size(87, 17);
            this.xmlStorageRadio.TabIndex = 4;
            this.xmlStorageRadio.TabStop = true;
            this.xmlStorageRadio.Text = "XML Storage";
            this.xmlStorageRadio.UseVisualStyleBackColor = true;
            this.xmlStorageRadio.CheckedChanged += new System.EventHandler(this.storageRadio_CheckedChanged);
            // 
            // txtStorageBrowseButton
            // 
            this.txtStorageBrowseButton.Location = new System.Drawing.Point(509, 87);
            this.txtStorageBrowseButton.Name = "txtStorageBrowseButton";
            this.txtStorageBrowseButton.Size = new System.Drawing.Size(75, 20);
            this.txtStorageBrowseButton.TabIndex = 3;
            this.txtStorageBrowseButton.Text = "Browse...";
            this.txtStorageBrowseButton.UseVisualStyleBackColor = true;
            this.txtStorageBrowseButton.Click += new System.EventHandler(this.txtStorageBrowseButton_Click);
            // 
            // xmlStorageBrowseButton
            // 
            this.xmlStorageBrowseButton.Location = new System.Drawing.Point(509, 36);
            this.xmlStorageBrowseButton.Name = "xmlStorageBrowseButton";
            this.xmlStorageBrowseButton.Size = new System.Drawing.Size(75, 20);
            this.xmlStorageBrowseButton.TabIndex = 1;
            this.xmlStorageBrowseButton.Text = "Browse...";
            this.xmlStorageBrowseButton.UseVisualStyleBackColor = true;
            this.xmlStorageBrowseButton.Click += new System.EventHandler(this.xmlStorageBrowseButton_Click);
            // 
            // txtStorageDirectoryTxt
            // 
            this.txtStorageDirectoryTxt.Location = new System.Drawing.Point(114, 87);
            this.txtStorageDirectoryTxt.Name = "txtStorageDirectoryTxt";
            this.txtStorageDirectoryTxt.Size = new System.Drawing.Size(388, 20);
            this.txtStorageDirectoryTxt.TabIndex = 2;
            // 
            // xmlStorageDirectoryTxt
            // 
            this.xmlStorageDirectoryTxt.Location = new System.Drawing.Point(114, 36);
            this.xmlStorageDirectoryTxt.Name = "xmlStorageDirectoryTxt";
            this.xmlStorageDirectoryTxt.Size = new System.Drawing.Size(388, 20);
            this.xmlStorageDirectoryTxt.TabIndex = 0;
            // 
            // txtStorageDirectory
            // 
            this.txtStorageDirectory.AutoSize = true;
            this.txtStorageDirectory.Location = new System.Drawing.Point(39, 90);
            this.txtStorageDirectory.Name = "txtStorageDirectory";
            this.txtStorageDirectory.Size = new System.Drawing.Size(69, 13);
            this.txtStorageDirectory.TabIndex = 1;
            this.txtStorageDirectory.Text = "File directory:";
            // 
            // xmlStorageDirectory
            // 
            this.xmlStorageDirectory.AutoSize = true;
            this.xmlStorageDirectory.Location = new System.Drawing.Point(39, 39);
            this.xmlStorageDirectory.Name = "xmlStorageDirectory";
            this.xmlStorageDirectory.Size = new System.Drawing.Size(69, 13);
            this.xmlStorageDirectory.TabIndex = 0;
            this.xmlStorageDirectory.Text = "File directory:";
            // 
            // SaveSettingsButton
            // 
            this.SaveSettingsButton.Location = new System.Drawing.Point(446, 195);
            this.SaveSettingsButton.Name = "SaveSettingsButton";
            this.SaveSettingsButton.Size = new System.Drawing.Size(75, 23);
            this.SaveSettingsButton.TabIndex = 5;
            this.SaveSettingsButton.Text = "Save";
            this.SaveSettingsButton.UseVisualStyleBackColor = true;
            this.SaveSettingsButton.Click += new System.EventHandler(this.SaveSettingsButton_Click);
            // 
            // CancelSettingsButton
            // 
            this.CancelSettingsButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelSettingsButton.Location = new System.Drawing.Point(527, 195);
            this.CancelSettingsButton.Name = "CancelSettingsButton";
            this.CancelSettingsButton.Size = new System.Drawing.Size(75, 23);
            this.CancelSettingsButton.TabIndex = 6;
            this.CancelSettingsButton.Text = "Cancel";
            this.CancelSettingsButton.UseVisualStyleBackColor = true;
            // 
            // SettingsWindow
            // 
            this.AcceptButton = this.SaveSettingsButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelSettingsButton;
            this.ClientSize = new System.Drawing.Size(615, 231);
            this.Controls.Add(this.SaveSettingsButton);
            this.Controls.Add(this.CancelSettingsButton);
            this.Controls.Add(this.dataStorageGrp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsWindow";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsWindow_Load);
            this.dataStorageGrp.ResumeLayout(false);
            this.dataStorageGrp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox dataStorageGrp;
        private System.Windows.Forms.Button txtStorageBrowseButton;
        private System.Windows.Forms.Button xmlStorageBrowseButton;
        private System.Windows.Forms.TextBox txtStorageDirectoryTxt;
        private System.Windows.Forms.TextBox xmlStorageDirectoryTxt;
        private System.Windows.Forms.Label txtStorageDirectory;
        private System.Windows.Forms.Label xmlStorageDirectory;
        private System.Windows.Forms.Button SaveSettingsButton;
        private System.Windows.Forms.Button CancelSettingsButton;
        private System.Windows.Forms.Button excelStorageBrowseButton;
        private System.Windows.Forms.TextBox excelStorageDirectoryTxt;
        private System.Windows.Forms.Label excelStorageDirectory;
        private System.Windows.Forms.RadioButton excelStorageRadio;
        private System.Windows.Forms.RadioButton txtStorageRadio;
        private System.Windows.Forms.RadioButton xmlStorageRadio;
        private System.Windows.Forms.FolderBrowserDialog browserDlg;
    }
}