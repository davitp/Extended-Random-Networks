namespace RandNet
{
    partial class ProbabilityCalculatorWindow
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProbabilityCalculatorWindow));
            this.branchingIndex = new System.Windows.Forms.Label();
            this.branchingIndexTxt = new System.Windows.Forms.TextBox();
            this.levelTxt = new System.Windows.Forms.TextBox();
            this.level = new System.Windows.Forms.Label();
            this.minMuTxt = new System.Windows.Forms.TextBox();
            this.mu = new System.Windows.Forms.Label();
            this.maxMuTxt = new System.Windows.Forms.TextBox();
            this.dash = new System.Windows.Forms.Label();
            this.deltaTxt = new System.Windows.Forms.TextBox();
            this.delta = new System.Windows.Forms.Label();
            this.calculate = new System.Windows.Forms.Button();
            this.result = new System.Windows.Forms.Label();
            this.resultsTable = new System.Windows.Forms.DataGridView();
            this.muColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.probabilityColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.save = new System.Windows.Forms.Button();
            this.saveFileDlg = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.resultsTable)).BeginInit();
            this.SuspendLayout();
            // 
            // branchingIndex
            // 
            this.branchingIndex.AutoSize = true;
            this.branchingIndex.Location = new System.Drawing.Point(12, 9);
            this.branchingIndex.Name = "branchingIndex";
            this.branchingIndex.Size = new System.Drawing.Size(84, 13);
            this.branchingIndex.TabIndex = 0;
            this.branchingIndex.Text = "Branching Index";
            // 
            // branchingIndexTxt
            // 
            this.branchingIndexTxt.Location = new System.Drawing.Point(15, 25);
            this.branchingIndexTxt.Name = "branchingIndexTxt";
            this.branchingIndexTxt.Size = new System.Drawing.Size(100, 20);
            this.branchingIndexTxt.TabIndex = 1;
            this.branchingIndexTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // levelTxt
            // 
            this.levelTxt.Location = new System.Drawing.Point(15, 73);
            this.levelTxt.Name = "levelTxt";
            this.levelTxt.Size = new System.Drawing.Size(100, 20);
            this.levelTxt.TabIndex = 3;
            this.levelTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // level
            // 
            this.level.AutoSize = true;
            this.level.Location = new System.Drawing.Point(12, 57);
            this.level.Name = "level";
            this.level.Size = new System.Drawing.Size(33, 13);
            this.level.TabIndex = 2;
            this.level.Text = "Level";
            // 
            // minMuTxt
            // 
            this.minMuTxt.Location = new System.Drawing.Point(15, 122);
            this.minMuTxt.Name = "minMuTxt";
            this.minMuTxt.Size = new System.Drawing.Size(45, 20);
            this.minMuTxt.TabIndex = 5;
            this.minMuTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // mu
            // 
            this.mu.AutoSize = true;
            this.mu.Location = new System.Drawing.Point(12, 106);
            this.mu.Name = "mu";
            this.mu.Size = new System.Drawing.Size(69, 13);
            this.mu.TabIndex = 4;
            this.mu.Text = "Range of Mu";
            // 
            // maxMuTxt
            // 
            this.maxMuTxt.Location = new System.Drawing.Point(70, 122);
            this.maxMuTxt.Name = "maxMuTxt";
            this.maxMuTxt.Size = new System.Drawing.Size(45, 20);
            this.maxMuTxt.TabIndex = 6;
            this.maxMuTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dash
            // 
            this.dash.AutoSize = true;
            this.dash.Location = new System.Drawing.Point(61, 125);
            this.dash.Name = "dash";
            this.dash.Size = new System.Drawing.Size(10, 13);
            this.dash.TabIndex = 7;
            this.dash.Text = "-";
            // 
            // deltaTxt
            // 
            this.deltaTxt.Location = new System.Drawing.Point(15, 170);
            this.deltaTxt.Name = "deltaTxt";
            this.deltaTxt.Size = new System.Drawing.Size(100, 20);
            this.deltaTxt.TabIndex = 9;
            this.deltaTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // delta
            // 
            this.delta.AutoSize = true;
            this.delta.Location = new System.Drawing.Point(12, 154);
            this.delta.Name = "delta";
            this.delta.Size = new System.Drawing.Size(32, 13);
            this.delta.TabIndex = 8;
            this.delta.Text = "Delta";
            // 
            // calculate
            // 
            this.calculate.Location = new System.Drawing.Point(15, 207);
            this.calculate.Name = "calculate";
            this.calculate.Size = new System.Drawing.Size(100, 23);
            this.calculate.TabIndex = 10;
            this.calculate.Text = "Calculate";
            this.calculate.UseVisualStyleBackColor = true;
            this.calculate.Click += new System.EventHandler(this.calculate_Click);
            // 
            // result
            // 
            this.result.AutoSize = true;
            this.result.Location = new System.Drawing.Point(149, 9);
            this.result.Name = "result";
            this.result.Size = new System.Drawing.Size(37, 13);
            this.result.TabIndex = 11;
            this.result.Text = "Result";
            // 
            // resultsTable
            // 
            this.resultsTable.AllowUserToAddRows = false;
            this.resultsTable.AllowUserToDeleteRows = false;
            this.resultsTable.AllowUserToResizeColumns = false;
            this.resultsTable.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.resultsTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.resultsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultsTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.muColumn,
            this.probabilityColumn});
            this.resultsTable.Location = new System.Drawing.Point(152, 25);
            this.resultsTable.Name = "resultsTable";
            this.resultsTable.ReadOnly = true;
            this.resultsTable.RowHeadersVisible = false;
            this.resultsTable.Size = new System.Drawing.Size(250, 165);
            this.resultsTable.TabIndex = 12;
            // 
            // muColumn
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.muColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.muColumn.HeaderText = "Mu";
            this.muColumn.Name = "muColumn";
            this.muColumn.ReadOnly = true;
            this.muColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.muColumn.Width = 90;
            // 
            // probabilityColumn
            // 
            this.probabilityColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.probabilityColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.probabilityColumn.HeaderText = "Probability";
            this.probabilityColumn.Name = "probabilityColumn";
            this.probabilityColumn.ReadOnly = true;
            this.probabilityColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // save
            // 
            this.save.Enabled = false;
            this.save.Location = new System.Drawing.Point(302, 207);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(100, 23);
            this.save.TabIndex = 13;
            this.save.Text = "Save";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // saveFileDlg
            // 
            this.saveFileDlg.Filter = "\"txt files (*.txt)|*.txt|All files (*.*)|*.*\"";
            // 
            // ProbabilityCalculatorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 242);
            this.Controls.Add(this.save);
            this.Controls.Add(this.resultsTable);
            this.Controls.Add(this.result);
            this.Controls.Add(this.calculate);
            this.Controls.Add(this.deltaTxt);
            this.Controls.Add(this.delta);
            this.Controls.Add(this.dash);
            this.Controls.Add(this.maxMuTxt);
            this.Controls.Add(this.minMuTxt);
            this.Controls.Add(this.mu);
            this.Controls.Add(this.levelTxt);
            this.Controls.Add(this.level);
            this.Controls.Add(this.branchingIndexTxt);
            this.Controls.Add(this.branchingIndex);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ProbabilityCalculatorWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Probability Calculator Tool";
            ((System.ComponentModel.ISupportInitialize)(this.resultsTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label branchingIndex;
        private System.Windows.Forms.TextBox branchingIndexTxt;
        private System.Windows.Forms.TextBox levelTxt;
        private System.Windows.Forms.Label level;
        private System.Windows.Forms.TextBox minMuTxt;
        private System.Windows.Forms.Label mu;
        private System.Windows.Forms.TextBox maxMuTxt;
        private System.Windows.Forms.Label dash;
        private System.Windows.Forms.TextBox deltaTxt;
        private System.Windows.Forms.Label delta;
        private System.Windows.Forms.Button calculate;
        private System.Windows.Forms.Label result;
        private System.Windows.Forms.DataGridView resultsTable;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.DataGridViewTextBoxColumn muColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn probabilityColumn;
        private System.Windows.Forms.SaveFileDialog saveFileDlg;
    }
}