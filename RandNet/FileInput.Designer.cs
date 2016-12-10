namespace RandNet
{
    partial class FileInput
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.locationTxt = new System.Windows.Forms.TextBox();
            this.browse = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.matrixSizeTxt = new System.Windows.Forms.NumericUpDown();
            this.matrixSize = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.matrixSizeTxt)).BeginInit();
            this.SuspendLayout();
            // 
            // locationTxt
            // 
            this.locationTxt.Location = new System.Drawing.Point(2, 1);
            this.locationTxt.Name = "locationTxt";
            this.locationTxt.Size = new System.Drawing.Size(100, 20);
            this.locationTxt.TabIndex = 0;
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(106, 1);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(75, 20);
            this.browse.TabIndex = 1;
            this.browse.Text = "Browse...";
            this.browse.UseVisualStyleBackColor = true;
            this.browse.Click += new System.EventHandler(this.browse_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            // 
            // folderBrowserDlg
            // 
            this.folderBrowserDlg.ShowNewFolderButton = false;
            // 
            // matrixSizeTxt
            // 
            this.matrixSizeTxt.Location = new System.Drawing.Point(106, 27);
            this.matrixSizeTxt.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.matrixSizeTxt.Name = "matrixSizeTxt";
            this.matrixSizeTxt.Size = new System.Drawing.Size(75, 20);
            this.matrixSizeTxt.TabIndex = 2;
            this.matrixSizeTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // matrixSize
            // 
            this.matrixSize.AutoSize = true;
            this.matrixSize.Location = new System.Drawing.Point(43, 29);
            this.matrixSize.Name = "matrixSize";
            this.matrixSize.Size = new System.Drawing.Size(59, 13);
            this.matrixSize.TabIndex = 3;
            this.matrixSize.Text = "Matrix size:";
            // 
            // FileInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.matrixSize);
            this.Controls.Add(this.matrixSizeTxt);
            this.Controls.Add(this.browse);
            this.Controls.Add(this.locationTxt);
            this.Name = "FileInput";
            this.Size = new System.Drawing.Size(184, 50);
            ((System.ComponentModel.ISupportInitialize)(this.matrixSizeTxt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox locationTxt;
        private System.Windows.Forms.Button browse;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDlg;
        private System.Windows.Forms.NumericUpDown matrixSizeTxt;
        private System.Windows.Forms.Label matrixSize;
    }
}
