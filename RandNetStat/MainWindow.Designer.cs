namespace RandNetStat
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFromToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.researchesGrp = new System.Windows.Forms.GroupBox();
            this.parametersTable = new System.Windows.Forms.DataGridView();
            this.generationParameterNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.generationParameterValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.refresh = new System.Windows.Forms.Button();
            this.researchesTable = new System.Windows.Forms.DataGridView();
            this.guidColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.realizationCountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.researchTableCSM = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.eraseResearch = new System.Windows.Forms.ToolStripMenuItem();
            this.selectGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.modelTypeCmb = new System.Windows.Forms.ComboBox();
            this.modelType = new System.Windows.Forms.Label();
            this.researchTypeCmb = new System.Windows.Forms.ComboBox();
            this.researchType = new System.Windows.Forms.Label();
            this.globalOptionsGrp = new System.Windows.Forms.GroupBox();
            this.showValues = new System.Windows.Forms.Button();
            this.globalDeselectAll = new System.Windows.Forms.Button();
            this.globalSelectAll = new System.Windows.Forms.Button();
            this.globalOptionsPanel = new System.Windows.Forms.Panel();
            this.distributedOptionsGrp = new System.Windows.Forms.GroupBox();
            this.showGraphics = new System.Windows.Forms.Button();
            this.distributedDeselectAll = new System.Windows.Forms.Button();
            this.distributedSelectAll = new System.Windows.Forms.Button();
            this.distributedOptionsPanel = new System.Windows.Forms.Panel();
            this.mainMenu.SuspendLayout();
            this.researchesGrp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parametersTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.researchesTable)).BeginInit();
            this.researchTableCSM.SuspendLayout();
            this.globalOptionsGrp.SuspendLayout();
            this.distributedOptionsGrp.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(777, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "mainMenu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadFromToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // loadFromToolStripMenuItem
            // 
            this.loadFromToolStripMenuItem.Name = "loadFromToolStripMenuItem";
            this.loadFromToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadFromToolStripMenuItem.Text = "&Settings";
            this.loadFromToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // researchesGrp
            // 
            this.researchesGrp.Controls.Add(this.parametersTable);
            this.researchesGrp.Controls.Add(this.refresh);
            this.researchesGrp.Controls.Add(this.researchesTable);
            this.researchesGrp.Controls.Add(this.modelTypeCmb);
            this.researchesGrp.Controls.Add(this.modelType);
            this.researchesGrp.Controls.Add(this.researchTypeCmb);
            this.researchesGrp.Controls.Add(this.researchType);
            this.researchesGrp.Location = new System.Drawing.Point(12, 27);
            this.researchesGrp.Name = "researchesGrp";
            this.researchesGrp.Size = new System.Drawing.Size(753, 350);
            this.researchesGrp.TabIndex = 1;
            this.researchesGrp.TabStop = false;
            // 
            // parametersTable
            // 
            this.parametersTable.AllowUserToAddRows = false;
            this.parametersTable.AllowUserToDeleteRows = false;
            this.parametersTable.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.parametersTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.parametersTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.parametersTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.generationParameterNameColumn,
            this.generationParameterValueColumn});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.parametersTable.DefaultCellStyle = dataGridViewCellStyle3;
            this.parametersTable.Location = new System.Drawing.Point(384, 54);
            this.parametersTable.MultiSelect = false;
            this.parametersTable.Name = "parametersTable";
            this.parametersTable.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.parametersTable.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.parametersTable.RowHeadersVisible = false;
            this.parametersTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.parametersTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.ColumnHeaderSelect;
            this.parametersTable.Size = new System.Drawing.Size(363, 290);
            this.parametersTable.TabIndex = 28;
            // 
            // generationParameterNameColumn
            // 
            this.generationParameterNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.generationParameterNameColumn.FillWeight = 98.47716F;
            this.generationParameterNameColumn.HeaderText = "Parameter";
            this.generationParameterNameColumn.Name = "generationParameterNameColumn";
            this.generationParameterNameColumn.ReadOnly = true;
            this.generationParameterNameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.generationParameterNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.generationParameterNameColumn.ToolTipText = "Parameter Name";
            // 
            // generationParameterValueColumn
            // 
            this.generationParameterValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = null;
            this.generationParameterValueColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.generationParameterValueColumn.FillWeight = 101.5228F;
            this.generationParameterValueColumn.HeaderText = "Value";
            this.generationParameterValueColumn.Name = "generationParameterValueColumn";
            this.generationParameterValueColumn.ReadOnly = true;
            this.generationParameterValueColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.generationParameterValueColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.generationParameterValueColumn.ToolTipText = "Parameter Value";
            // 
            // refresh
            // 
            this.refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refresh.Location = new System.Drawing.Point(668, 19);
            this.refresh.Name = "refresh";
            this.refresh.Size = new System.Drawing.Size(79, 21);
            this.refresh.TabIndex = 10;
            this.refresh.Text = "Refresh";
            this.refresh.UseVisualStyleBackColor = true;
            this.refresh.Click += new System.EventHandler(this.refresh_Click);
            // 
            // researchesTable
            // 
            this.researchesTable.AllowUserToAddRows = false;
            this.researchesTable.AllowUserToDeleteRows = false;
            this.researchesTable.AllowUserToResizeColumns = false;
            this.researchesTable.AllowUserToResizeRows = false;
            this.researchesTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.researchesTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.guidColumn,
            this.nameColumn,
            this.realizationCountColumn,
            this.sizeColumn});
            this.researchesTable.ContextMenuStrip = this.researchTableCSM;
            this.researchesTable.Location = new System.Drawing.Point(6, 54);
            this.researchesTable.Name = "researchesTable";
            this.researchesTable.ReadOnly = true;
            this.researchesTable.RowHeadersVisible = false;
            this.researchesTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.researchesTable.Size = new System.Drawing.Size(366, 290);
            this.researchesTable.TabIndex = 9;
            this.researchesTable.SelectionChanged += new System.EventHandler(this.researchesTable_SelectionChanged);
            this.researchesTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.researchesTable_MouseDown);
            // 
            // guidColumn
            // 
            this.guidColumn.HeaderText = "Guid";
            this.guidColumn.Name = "guidColumn";
            this.guidColumn.ReadOnly = true;
            this.guidColumn.Visible = false;
            // 
            // nameColumn
            // 
            this.nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.nameColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.nameColumn.HeaderText = "Name";
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
            this.nameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // realizationCountColumn
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.realizationCountColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.realizationCountColumn.HeaderText = "Realizations";
            this.realizationCountColumn.Name = "realizationCountColumn";
            this.realizationCountColumn.ReadOnly = true;
            this.realizationCountColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.realizationCountColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.realizationCountColumn.ToolTipText = "Realization Count";
            // 
            // sizeColumn
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.sizeColumn.DefaultCellStyle = dataGridViewCellStyle7;
            this.sizeColumn.HeaderText = "Network Size";
            this.sizeColumn.Name = "sizeColumn";
            this.sizeColumn.ReadOnly = true;
            this.sizeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.sizeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.sizeColumn.ToolTipText = "Network Size";
            // 
            // researchTableCSM
            // 
            this.researchTableCSM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eraseResearch,
            this.selectGroup});
            this.researchTableCSM.Name = "researchTableCSM";
            this.researchTableCSM.Size = new System.Drawing.Size(152, 48);
            // 
            // eraseResearch
            // 
            this.eraseResearch.Name = "eraseResearch";
            this.eraseResearch.Size = new System.Drawing.Size(151, 22);
            this.eraseResearch.Text = "&Erase Research";
            this.eraseResearch.Click += new System.EventHandler(this.eraseResearchToolStripMenuItem_Click);
            // 
            // selectGroup
            // 
            this.selectGroup.Name = "selectGroup";
            this.selectGroup.Size = new System.Drawing.Size(151, 22);
            this.selectGroup.Text = "&Select Group";
            this.selectGroup.Click += new System.EventHandler(this.selectGroupToolStripMenuItem_Click);
            // 
            // modelTypeCmb
            // 
            this.modelTypeCmb.FormattingEnabled = true;
            this.modelTypeCmb.Location = new System.Drawing.Point(308, 19);
            this.modelTypeCmb.Name = "modelTypeCmb";
            this.modelTypeCmb.Size = new System.Drawing.Size(121, 21);
            this.modelTypeCmb.TabIndex = 8;
            this.modelTypeCmb.SelectedIndexChanged += new System.EventHandler(this.modelTypeCmb_SelectedIndexChanged);
            // 
            // modelType
            // 
            this.modelType.AutoSize = true;
            this.modelType.Location = new System.Drawing.Point(236, 22);
            this.modelType.Name = "modelType";
            this.modelType.Size = new System.Drawing.Size(66, 13);
            this.modelType.TabIndex = 7;
            this.modelType.Text = "Model Type:";
            // 
            // researchTypeCmb
            // 
            this.researchTypeCmb.FormattingEnabled = true;
            this.researchTypeCmb.Location = new System.Drawing.Point(95, 19);
            this.researchTypeCmb.Name = "researchTypeCmb";
            this.researchTypeCmb.Size = new System.Drawing.Size(121, 21);
            this.researchTypeCmb.TabIndex = 6;
            this.researchTypeCmb.SelectedIndexChanged += new System.EventHandler(this.researchTypeCmb_SelectedIndexChanged);
            // 
            // researchType
            // 
            this.researchType.AutoSize = true;
            this.researchType.Location = new System.Drawing.Point(6, 22);
            this.researchType.Name = "researchType";
            this.researchType.Size = new System.Drawing.Size(83, 13);
            this.researchType.TabIndex = 5;
            this.researchType.Text = "Research Type:";
            // 
            // globalOptionsGrp
            // 
            this.globalOptionsGrp.Controls.Add(this.showValues);
            this.globalOptionsGrp.Controls.Add(this.globalDeselectAll);
            this.globalOptionsGrp.Controls.Add(this.globalSelectAll);
            this.globalOptionsGrp.Controls.Add(this.globalOptionsPanel);
            this.globalOptionsGrp.Location = new System.Drawing.Point(12, 383);
            this.globalOptionsGrp.Name = "globalOptionsGrp";
            this.globalOptionsGrp.Size = new System.Drawing.Size(372, 228);
            this.globalOptionsGrp.TabIndex = 5;
            this.globalOptionsGrp.TabStop = false;
            this.globalOptionsGrp.Text = "Global Options";
            // 
            // showValues
            // 
            this.showValues.Location = new System.Drawing.Point(271, 193);
            this.showValues.Name = "showValues";
            this.showValues.Size = new System.Drawing.Size(93, 28);
            this.showValues.TabIndex = 4;
            this.showValues.Text = "Show Values";
            this.showValues.UseVisualStyleBackColor = true;
            this.showValues.Click += new System.EventHandler(this.showValues_Click);
            // 
            // globalDeselectAll
            // 
            this.globalDeselectAll.Location = new System.Drawing.Point(87, 193);
            this.globalDeselectAll.Name = "globalDeselectAll";
            this.globalDeselectAll.Size = new System.Drawing.Size(75, 23);
            this.globalDeselectAll.TabIndex = 3;
            this.globalDeselectAll.Text = "Deselect All";
            this.globalDeselectAll.UseVisualStyleBackColor = true;
            this.globalDeselectAll.Click += new System.EventHandler(this.deselectAll_Click);
            // 
            // globalSelectAll
            // 
            this.globalSelectAll.Location = new System.Drawing.Point(6, 193);
            this.globalSelectAll.Name = "globalSelectAll";
            this.globalSelectAll.Size = new System.Drawing.Size(75, 23);
            this.globalSelectAll.TabIndex = 2;
            this.globalSelectAll.Text = "Select All";
            this.globalSelectAll.UseVisualStyleBackColor = true;
            this.globalSelectAll.Click += new System.EventHandler(this.selectAll_Click);
            // 
            // globalOptionsPanel
            // 
            this.globalOptionsPanel.AutoScroll = true;
            this.globalOptionsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.globalOptionsPanel.Location = new System.Drawing.Point(3, 16);
            this.globalOptionsPanel.Name = "globalOptionsPanel";
            this.globalOptionsPanel.Size = new System.Drawing.Size(366, 171);
            this.globalOptionsPanel.TabIndex = 0;
            // 
            // distributedOptionsGrp
            // 
            this.distributedOptionsGrp.Controls.Add(this.showGraphics);
            this.distributedOptionsGrp.Controls.Add(this.distributedDeselectAll);
            this.distributedOptionsGrp.Controls.Add(this.distributedSelectAll);
            this.distributedOptionsGrp.Controls.Add(this.distributedOptionsPanel);
            this.distributedOptionsGrp.Location = new System.Drawing.Point(396, 383);
            this.distributedOptionsGrp.Name = "distributedOptionsGrp";
            this.distributedOptionsGrp.Size = new System.Drawing.Size(372, 228);
            this.distributedOptionsGrp.TabIndex = 5;
            this.distributedOptionsGrp.TabStop = false;
            this.distributedOptionsGrp.Text = "Distributed Options";
            // 
            // showGraphics
            // 
            this.showGraphics.Location = new System.Drawing.Point(271, 193);
            this.showGraphics.Name = "showGraphics";
            this.showGraphics.Size = new System.Drawing.Size(93, 28);
            this.showGraphics.TabIndex = 5;
            this.showGraphics.Text = "Show Graphics";
            this.showGraphics.UseVisualStyleBackColor = true;
            this.showGraphics.Click += new System.EventHandler(this.showGraphics_Click);
            // 
            // distributedDeselectAll
            // 
            this.distributedDeselectAll.Location = new System.Drawing.Point(87, 193);
            this.distributedDeselectAll.Name = "distributedDeselectAll";
            this.distributedDeselectAll.Size = new System.Drawing.Size(75, 23);
            this.distributedDeselectAll.TabIndex = 3;
            this.distributedDeselectAll.Text = "Deselect All";
            this.distributedDeselectAll.UseVisualStyleBackColor = true;
            this.distributedDeselectAll.Click += new System.EventHandler(this.deselectAll_Click);
            // 
            // distributedSelectAll
            // 
            this.distributedSelectAll.Location = new System.Drawing.Point(6, 193);
            this.distributedSelectAll.Name = "distributedSelectAll";
            this.distributedSelectAll.Size = new System.Drawing.Size(75, 23);
            this.distributedSelectAll.TabIndex = 2;
            this.distributedSelectAll.Text = "Select All";
            this.distributedSelectAll.UseVisualStyleBackColor = true;
            this.distributedSelectAll.Click += new System.EventHandler(this.selectAll_Click);
            // 
            // distributedOptionsPanel
            // 
            this.distributedOptionsPanel.AutoScroll = true;
            this.distributedOptionsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.distributedOptionsPanel.Location = new System.Drawing.Point(3, 16);
            this.distributedOptionsPanel.Name = "distributedOptionsPanel";
            this.distributedOptionsPanel.Size = new System.Drawing.Size(366, 171);
            this.distributedOptionsPanel.TabIndex = 0;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 623);
            this.Controls.Add(this.distributedOptionsGrp);
            this.Controls.Add(this.globalOptionsGrp);
            this.Controls.Add(this.researchesGrp);
            this.Controls.Add(this.mainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "xRandNetStat";
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.researchesGrp.ResumeLayout(false);
            this.researchesGrp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parametersTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.researchesTable)).EndInit();
            this.researchTableCSM.ResumeLayout(false);
            this.globalOptionsGrp.ResumeLayout(false);
            this.distributedOptionsGrp.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFromToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.GroupBox researchesGrp;
        private System.Windows.Forms.DataGridView researchesTable;
        private System.Windows.Forms.ComboBox modelTypeCmb;
        private System.Windows.Forms.Label modelType;
        private System.Windows.Forms.ComboBox researchTypeCmb;
        private System.Windows.Forms.Label researchType;
        private System.Windows.Forms.Button refresh;
        private System.Windows.Forms.ContextMenuStrip researchTableCSM;
        private System.Windows.Forms.ToolStripMenuItem eraseResearch;
        private System.Windows.Forms.ToolStripMenuItem selectGroup;
        private System.Windows.Forms.GroupBox globalOptionsGrp;
        private System.Windows.Forms.Button globalDeselectAll;
        private System.Windows.Forms.Button globalSelectAll;
        private System.Windows.Forms.Panel globalOptionsPanel;
        private System.Windows.Forms.GroupBox distributedOptionsGrp;
        private System.Windows.Forms.Button distributedDeselectAll;
        private System.Windows.Forms.Button distributedSelectAll;
        private System.Windows.Forms.Panel distributedOptionsPanel;
        private System.Windows.Forms.DataGridView parametersTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn realizationCountColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizeColumn;
        private System.Windows.Forms.Button showValues;
        private System.Windows.Forms.Button showGraphics;
        private System.Windows.Forms.DataGridViewTextBoxColumn generationParameterNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn generationParameterValueColumn;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}

