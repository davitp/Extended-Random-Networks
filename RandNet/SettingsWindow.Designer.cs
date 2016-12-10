namespace RandNet
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
            this.SaveSettingsButton = new System.Windows.Forms.Button();
            this.CancelSettingsButton = new System.Windows.Forms.Button();
            this.browserDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.generalGrp = new System.Windows.Forms.GroupBox();
            this.tracingGrp = new System.Windows.Forms.GroupBox();
            this.tracingDirectory = new System.Windows.Forms.Label();
            this.tracingDirectoryTxt = new System.Windows.Forms.TextBox();
            this.tracingBrowse = new System.Windows.Forms.Button();
            this.loggingGrp = new System.Windows.Forms.GroupBox();
            this.loggingDirectory = new System.Windows.Forms.Label();
            this.loggingDirectoryTxt = new System.Windows.Forms.TextBox();
            this.loggingBrowse = new System.Windows.Forms.Button();
            this.storageGrp = new System.Windows.Forms.GroupBox();
            this.connections = new System.Windows.Forms.Button();
            this.storageBrowse = new System.Windows.Forms.Button();
            this.databaseTxt = new System.Windows.Forms.TextBox();
            this.storageDirectoryTxt = new System.Windows.Forms.TextBox();
            this.database = new System.Windows.Forms.Label();
            this.storageDirectory = new System.Windows.Forms.Label();
            this.matrixRadio = new System.Windows.Forms.RadioButton();
            this.neighbourshipRadio = new System.Windows.Forms.RadioButton();
            this.generalGrp.SuspendLayout();
            this.tracingGrp.SuspendLayout();
            this.loggingGrp.SuspendLayout();
            this.storageGrp.SuspendLayout();
            this.SuspendLayout();
            // 
            // SaveSettingsButton
            // 
            this.SaveSettingsButton.Location = new System.Drawing.Point(455, 304);
            this.SaveSettingsButton.Name = "SaveSettingsButton";
            this.SaveSettingsButton.Size = new System.Drawing.Size(75, 23);
            this.SaveSettingsButton.TabIndex = 3;
            this.SaveSettingsButton.Text = "Save";
            this.SaveSettingsButton.UseVisualStyleBackColor = true;
            this.SaveSettingsButton.Click += new System.EventHandler(this.SaveSettingsButton_Click);
            // 
            // CancelSettingsButton
            // 
            this.CancelSettingsButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelSettingsButton.Location = new System.Drawing.Point(536, 304);
            this.CancelSettingsButton.Name = "CancelSettingsButton";
            this.CancelSettingsButton.Size = new System.Drawing.Size(75, 23);
            this.CancelSettingsButton.TabIndex = 4;
            this.CancelSettingsButton.Text = "Cancel";
            this.CancelSettingsButton.UseVisualStyleBackColor = true;
            // 
            // generalGrp
            // 
            this.generalGrp.Controls.Add(this.tracingGrp);
            this.generalGrp.Controls.Add(this.loggingGrp);
            this.generalGrp.Controls.Add(this.storageGrp);
            this.generalGrp.Location = new System.Drawing.Point(12, 12);
            this.generalGrp.Name = "generalGrp";
            this.generalGrp.Size = new System.Drawing.Size(599, 286);
            this.generalGrp.TabIndex = 0;
            this.generalGrp.TabStop = false;
            this.generalGrp.Text = "General";
            // 
            // tracingGrp
            // 
            this.tracingGrp.Controls.Add(this.neighbourshipRadio);
            this.tracingGrp.Controls.Add(this.matrixRadio);
            this.tracingGrp.Controls.Add(this.tracingDirectory);
            this.tracingGrp.Controls.Add(this.tracingDirectoryTxt);
            this.tracingGrp.Controls.Add(this.tracingBrowse);
            this.tracingGrp.Location = new System.Drawing.Point(12, 183);
            this.tracingGrp.Name = "tracingGrp";
            this.tracingGrp.Size = new System.Drawing.Size(575, 93);
            this.tracingGrp.TabIndex = 0;
            this.tracingGrp.TabStop = false;
            this.tracingGrp.Text = "Tracing";
            // 
            // tracingDirectory
            // 
            this.tracingDirectory.AutoSize = true;
            this.tracingDirectory.Location = new System.Drawing.Point(6, 28);
            this.tracingDirectory.Name = "tracingDirectory";
            this.tracingDirectory.Size = new System.Drawing.Size(85, 13);
            this.tracingDirectory.TabIndex = 10;
            this.tracingDirectory.Text = "Output directory:";
            // 
            // tracingDirectoryTxt
            // 
            this.tracingDirectoryTxt.Location = new System.Drawing.Point(97, 25);
            this.tracingDirectoryTxt.Name = "tracingDirectoryTxt";
            this.tracingDirectoryTxt.Size = new System.Drawing.Size(388, 20);
            this.tracingDirectoryTxt.TabIndex = 11;
            // 
            // tracingBrowse
            // 
            this.tracingBrowse.Location = new System.Drawing.Point(491, 25);
            this.tracingBrowse.Name = "tracingBrowse";
            this.tracingBrowse.Size = new System.Drawing.Size(75, 20);
            this.tracingBrowse.TabIndex = 12;
            this.tracingBrowse.Text = "Browse...";
            this.tracingBrowse.UseVisualStyleBackColor = true;
            this.tracingBrowse.Click += new System.EventHandler(this.tracingBrowseBtn_Click);
            // 
            // loggingGrp
            // 
            this.loggingGrp.Controls.Add(this.loggingDirectory);
            this.loggingGrp.Controls.Add(this.loggingDirectoryTxt);
            this.loggingGrp.Controls.Add(this.loggingBrowse);
            this.loggingGrp.Location = new System.Drawing.Point(12, 16);
            this.loggingGrp.Name = "loggingGrp";
            this.loggingGrp.Size = new System.Drawing.Size(575, 64);
            this.loggingGrp.TabIndex = 0;
            this.loggingGrp.TabStop = false;
            this.loggingGrp.Text = "Logging";
            // 
            // loggingDirectory
            // 
            this.loggingDirectory.AutoSize = true;
            this.loggingDirectory.Location = new System.Drawing.Point(6, 29);
            this.loggingDirectory.Name = "loggingDirectory";
            this.loggingDirectory.Size = new System.Drawing.Size(85, 13);
            this.loggingDirectory.TabIndex = 1;
            this.loggingDirectory.Text = "Output directory:";
            // 
            // loggingDirectoryTxt
            // 
            this.loggingDirectoryTxt.Location = new System.Drawing.Point(97, 26);
            this.loggingDirectoryTxt.Name = "loggingDirectoryTxt";
            this.loggingDirectoryTxt.Size = new System.Drawing.Size(388, 20);
            this.loggingDirectoryTxt.TabIndex = 1;
            // 
            // loggingBrowse
            // 
            this.loggingBrowse.Location = new System.Drawing.Point(491, 26);
            this.loggingBrowse.Name = "loggingBrowse";
            this.loggingBrowse.Size = new System.Drawing.Size(75, 20);
            this.loggingBrowse.TabIndex = 2;
            this.loggingBrowse.Text = "Browse...";
            this.loggingBrowse.UseVisualStyleBackColor = true;
            this.loggingBrowse.Click += new System.EventHandler(this.loggingBrowseButton_Click);
            // 
            // storageGrp
            // 
            this.storageGrp.Controls.Add(this.connections);
            this.storageGrp.Controls.Add(this.storageBrowse);
            this.storageGrp.Controls.Add(this.databaseTxt);
            this.storageGrp.Controls.Add(this.storageDirectoryTxt);
            this.storageGrp.Controls.Add(this.database);
            this.storageGrp.Controls.Add(this.storageDirectory);
            this.storageGrp.Location = new System.Drawing.Point(12, 86);
            this.storageGrp.Name = "storageGrp";
            this.storageGrp.Size = new System.Drawing.Size(575, 91);
            this.storageGrp.TabIndex = 3;
            this.storageGrp.TabStop = false;
            this.storageGrp.Text = "Default storage locations";
            // 
            // connections
            // 
            this.connections.Location = new System.Drawing.Point(491, 53);
            this.connections.Name = "connections";
            this.connections.Size = new System.Drawing.Size(75, 20);
            this.connections.TabIndex = 9;
            this.connections.Text = "Connections";
            this.connections.UseVisualStyleBackColor = true;
            // 
            // storageBrowse
            // 
            this.storageBrowse.Location = new System.Drawing.Point(491, 24);
            this.storageBrowse.Name = "storageBrowse";
            this.storageBrowse.Size = new System.Drawing.Size(75, 20);
            this.storageBrowse.TabIndex = 6;
            this.storageBrowse.Text = "Browse...";
            this.storageBrowse.UseVisualStyleBackColor = true;
            this.storageBrowse.Click += new System.EventHandler(this.storageBrowseButton_Click);
            // 
            // databaseTxt
            // 
            this.databaseTxt.Location = new System.Drawing.Point(97, 53);
            this.databaseTxt.Name = "databaseTxt";
            this.databaseTxt.Size = new System.Drawing.Size(388, 20);
            this.databaseTxt.TabIndex = 8;
            // 
            // storageDirectoryTxt
            // 
            this.storageDirectoryTxt.Location = new System.Drawing.Point(97, 24);
            this.storageDirectoryTxt.Name = "storageDirectoryTxt";
            this.storageDirectoryTxt.Size = new System.Drawing.Size(388, 20);
            this.storageDirectoryTxt.TabIndex = 5;
            // 
            // database
            // 
            this.database.AutoSize = true;
            this.database.Location = new System.Drawing.Point(13, 56);
            this.database.Name = "database";
            this.database.Size = new System.Drawing.Size(78, 13);
            this.database.TabIndex = 7;
            this.database.Text = "SQL database:";
            // 
            // storageDirectory
            // 
            this.storageDirectory.AutoSize = true;
            this.storageDirectory.Location = new System.Drawing.Point(22, 27);
            this.storageDirectory.Name = "storageDirectory";
            this.storageDirectory.Size = new System.Drawing.Size(69, 13);
            this.storageDirectory.TabIndex = 4;
            this.storageDirectory.Text = "File directory:";
            // 
            // matrixRadio
            // 
            this.matrixRadio.AutoSize = true;
            this.matrixRadio.Location = new System.Drawing.Point(97, 60);
            this.matrixRadio.Name = "matrixRadio";
            this.matrixRadio.Size = new System.Drawing.Size(53, 17);
            this.matrixRadio.TabIndex = 13;
            this.matrixRadio.Text = "Matrix";
            this.matrixRadio.UseVisualStyleBackColor = true;
            // 
            // neighbourshipRadio
            // 
            this.neighbourshipRadio.AutoSize = true;
            this.neighbourshipRadio.Location = new System.Drawing.Point(166, 60);
            this.neighbourshipRadio.Name = "neighbourshipRadio";
            this.neighbourshipRadio.Size = new System.Drawing.Size(93, 17);
            this.neighbourshipRadio.TabIndex = 14;
            this.neighbourshipRadio.Text = "Neighbourship";
            this.neighbourshipRadio.UseVisualStyleBackColor = true;
            // 
            // SettingsWindow
            // 
            this.AcceptButton = this.SaveSettingsButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelSettingsButton;
            this.ClientSize = new System.Drawing.Size(623, 339);
            this.Controls.Add(this.generalGrp);
            this.Controls.Add(this.SaveSettingsButton);
            this.Controls.Add(this.CancelSettingsButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.generalGrp.ResumeLayout(false);
            this.tracingGrp.ResumeLayout(false);
            this.tracingGrp.PerformLayout();
            this.loggingGrp.ResumeLayout(false);
            this.loggingGrp.PerformLayout();
            this.storageGrp.ResumeLayout(false);
            this.storageGrp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SaveSettingsButton;
        private System.Windows.Forms.Button CancelSettingsButton;
        private System.Windows.Forms.FolderBrowserDialog browserDlg;
        private System.Windows.Forms.GroupBox generalGrp;
        private System.Windows.Forms.GroupBox tracingGrp;
        private System.Windows.Forms.Label tracingDirectory;
        private System.Windows.Forms.TextBox tracingDirectoryTxt;
        private System.Windows.Forms.Button tracingBrowse;
        private System.Windows.Forms.GroupBox loggingGrp;
        private System.Windows.Forms.Label loggingDirectory;
        private System.Windows.Forms.TextBox loggingDirectoryTxt;
        private System.Windows.Forms.Button loggingBrowse;
        private System.Windows.Forms.GroupBox storageGrp;
        private System.Windows.Forms.Button connections;
        private System.Windows.Forms.Button storageBrowse;
        private System.Windows.Forms.TextBox databaseTxt;
        private System.Windows.Forms.TextBox storageDirectoryTxt;
        private System.Windows.Forms.Label database;
        private System.Windows.Forms.Label storageDirectory;
        private System.Windows.Forms.RadioButton neighbourshipRadio;
        private System.Windows.Forms.RadioButton matrixRadio;

    }
}