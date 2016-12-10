namespace RandNet
{
    partial class RunTestsWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RunTestsWindow));
            this.resultsGrp = new System.Windows.Forms.GroupBox();
            this.optionsTable = new System.Windows.Forms.DataGridView();
            this.optionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.testNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.start = new System.Windows.Forms.Button();
            this.close = new System.Windows.Forms.Button();
            this.resultsGrp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.optionsTable)).BeginInit();
            this.SuspendLayout();
            // 
            // resultsGrp
            // 
            this.resultsGrp.Controls.Add(this.optionsTable);
            this.resultsGrp.Location = new System.Drawing.Point(12, 12);
            this.resultsGrp.Name = "resultsGrp";
            this.resultsGrp.Size = new System.Drawing.Size(489, 291);
            this.resultsGrp.TabIndex = 0;
            this.resultsGrp.TabStop = false;
            // 
            // optionsTable
            // 
            this.optionsTable.AllowUserToAddRows = false;
            this.optionsTable.AllowUserToDeleteRows = false;
            this.optionsTable.AllowUserToResizeColumns = false;
            this.optionsTable.AllowUserToResizeRows = false;
            this.optionsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.optionsTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.optionColumn,
            this.testNameColumn,
            this.statusColumn});
            this.optionsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionsTable.Location = new System.Drawing.Point(3, 16);
            this.optionsTable.Name = "optionsTable";
            this.optionsTable.ReadOnly = true;
            this.optionsTable.RowHeadersVisible = false;
            this.optionsTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.optionsTable.Size = new System.Drawing.Size(483, 272);
            this.optionsTable.TabIndex = 0;
            // 
            // optionColumn
            // 
            this.optionColumn.HeaderText = "Option";
            this.optionColumn.Name = "optionColumn";
            this.optionColumn.ReadOnly = true;
            this.optionColumn.Width = 200;
            // 
            // testNameColumn
            // 
            this.testNameColumn.HeaderText = "Name";
            this.testNameColumn.Name = "testNameColumn";
            this.testNameColumn.ReadOnly = true;
            // 
            // statusColumn
            // 
            this.statusColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.statusColumn.HeaderText = "Status";
            this.statusColumn.Name = "statusColumn";
            this.statusColumn.ReadOnly = true;
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(345, 309);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(75, 23);
            this.start.TabIndex = 1;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // close
            // 
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(426, 309);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 2;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // RunTestsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(513, 344);
            this.Controls.Add(this.close);
            this.Controls.Add(this.start);
            this.Controls.Add(this.resultsGrp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RunTestsWindow";
            this.Load += new System.EventHandler(this.RunTestsWindow_Load);
            this.resultsGrp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.optionsTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox resultsGrp;
        private System.Windows.Forms.DataGridView optionsTable;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.DataGridViewTextBoxColumn optionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn testNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusColumn;
    }
}