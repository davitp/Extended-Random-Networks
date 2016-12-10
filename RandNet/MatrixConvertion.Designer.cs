namespace RandNet
{
    partial class MatrixConvertionWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MatrixConvertionWindow));
            this.inputPath = new System.Windows.Forms.Label();
            this.inputPathTxt = new System.Windows.Forms.TextBox();
            this.inputBrowse = new System.Windows.Forms.Button();
            this.outputBrowse = new System.Windows.Forms.Button();
            this.outputPathTxt = new System.Windows.Forms.TextBox();
            this.outputPath = new System.Windows.Forms.Label();
            this.convert = new System.Windows.Forms.Button();
            this.sourceBrowserDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.targetBrowserDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // inputPath
            // 
            this.inputPath.AutoSize = true;
            this.inputPath.Location = new System.Drawing.Point(12, 17);
            this.inputPath.Name = "inputPath";
            this.inputPath.Size = new System.Drawing.Size(89, 13);
            this.inputPath.TabIndex = 0;
            this.inputPath.Text = "Matrix-input Path:";
            // 
            // inputPathTxt
            // 
            this.inputPathTxt.Location = new System.Drawing.Point(107, 14);
            this.inputPathTxt.Name = "inputPathTxt";
            this.inputPathTxt.Size = new System.Drawing.Size(311, 20);
            this.inputPathTxt.TabIndex = 1;
            this.inputPathTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // inputBrowse
            // 
            this.inputBrowse.Location = new System.Drawing.Point(424, 14);
            this.inputBrowse.Name = "inputBrowse";
            this.inputBrowse.Size = new System.Drawing.Size(75, 20);
            this.inputBrowse.TabIndex = 2;
            this.inputBrowse.Text = "Browse...";
            this.inputBrowse.UseVisualStyleBackColor = true;
            this.inputBrowse.Click += new System.EventHandler(this.inputBrowse_Click);
            // 
            // outputBrowse
            // 
            this.outputBrowse.Location = new System.Drawing.Point(424, 49);
            this.outputBrowse.Name = "outputBrowse";
            this.outputBrowse.Size = new System.Drawing.Size(75, 20);
            this.outputBrowse.TabIndex = 5;
            this.outputBrowse.Text = "Browse...";
            this.outputBrowse.UseVisualStyleBackColor = true;
            this.outputBrowse.Click += new System.EventHandler(this.outputBrowse_Click);
            // 
            // outputPathTxt
            // 
            this.outputPathTxt.Location = new System.Drawing.Point(107, 49);
            this.outputPathTxt.Name = "outputPathTxt";
            this.outputPathTxt.Size = new System.Drawing.Size(311, 20);
            this.outputPathTxt.TabIndex = 4;
            this.outputPathTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // outputPath
            // 
            this.outputPath.AutoSize = true;
            this.outputPath.Location = new System.Drawing.Point(34, 52);
            this.outputPath.Name = "outputPath";
            this.outputPath.Size = new System.Drawing.Size(67, 13);
            this.outputPath.TabIndex = 3;
            this.outputPath.Text = "Output Path:";
            // 
            // convert
            // 
            this.convert.Location = new System.Drawing.Point(424, 88);
            this.convert.Name = "convert";
            this.convert.Size = new System.Drawing.Size(75, 23);
            this.convert.TabIndex = 6;
            this.convert.Text = "Convert";
            this.convert.UseVisualStyleBackColor = true;
            this.convert.Click += new System.EventHandler(this.convert_Click);
            // 
            // MatrixConvertionWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 123);
            this.Controls.Add(this.convert);
            this.Controls.Add(this.outputBrowse);
            this.Controls.Add(this.outputPathTxt);
            this.Controls.Add(this.outputPath);
            this.Controls.Add(this.inputBrowse);
            this.Controls.Add(this.inputPathTxt);
            this.Controls.Add(this.inputPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MatrixConvertionWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Matrix Convertion Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MatrixConvertionWindow_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label inputPath;
        private System.Windows.Forms.TextBox inputPathTxt;
        private System.Windows.Forms.Button inputBrowse;
        private System.Windows.Forms.Button outputBrowse;
        private System.Windows.Forms.TextBox outputPathTxt;
        private System.Windows.Forms.Label outputPath;
        private System.Windows.Forms.Button convert;
        private System.Windows.Forms.FolderBrowserDialog sourceBrowserDlg;
        private System.Windows.Forms.FolderBrowserDialog targetBrowserDlg;
    }
}