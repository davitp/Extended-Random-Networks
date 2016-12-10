namespace RandNetStat
{
    partial class GraphicsTab
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.graphic = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.graphicsCSM = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.drawingOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statisticsOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.combineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.graphic)).BeginInit();
            this.graphicsCSM.SuspendLayout();
            this.SuspendLayout();
            // 
            // graphic
            // 
            chartArea1.Name = "ChartArea1";
            this.graphic.ChartAreas.Add(chartArea1);
            this.graphic.ContextMenuStrip = this.graphicsCSM;
            this.graphic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphic.Location = new System.Drawing.Point(0, 0);
            this.graphic.Name = "graphic";
            this.graphic.Size = new System.Drawing.Size(309, 310);
            this.graphic.TabIndex = 0;
            // 
            // graphicsCSM
            // 
            this.graphicsCSM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drawingOptionsToolStripMenuItem,
            this.statisticsOptionsToolStripMenuItem,
            this.combineToolStripMenuItem});
            this.graphicsCSM.Name = "graphicsCSM";
            this.graphicsCSM.Size = new System.Drawing.Size(173, 70);
            // 
            // drawingOptionsToolStripMenuItem
            // 
            this.drawingOptionsToolStripMenuItem.Name = "drawingOptionsToolStripMenuItem";
            this.drawingOptionsToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.drawingOptionsToolStripMenuItem.Text = "&Drawing Options";
            this.drawingOptionsToolStripMenuItem.Click += new System.EventHandler(this.drawingOptionsToolStripMenuItem_Click);
            // 
            // statisticsOptionsToolStripMenuItem
            // 
            this.statisticsOptionsToolStripMenuItem.Name = "statisticsOptionsToolStripMenuItem";
            this.statisticsOptionsToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.statisticsOptionsToolStripMenuItem.Text = "&Statistics Options";
            this.statisticsOptionsToolStripMenuItem.Click += new System.EventHandler(this.statisticsOptionsToolStripMenuItem_Click);
            // 
            // combineToolStripMenuItem
            // 
            this.combineToolStripMenuItem.Name = "combineToolStripMenuItem";
            this.combineToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.combineToolStripMenuItem.Text = "&Combine Graphics";
            this.combineToolStripMenuItem.Click += new System.EventHandler(this.combineToolStripMenuItem_Click);
            // 
            // GraphicsTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.graphic);
            this.Name = "GraphicsTab";
            this.Size = new System.Drawing.Size(309, 310);
            this.Load += new System.EventHandler(this.GraphicsTab_Load);
            ((System.ComponentModel.ISupportInitialize)(this.graphic)).EndInit();
            this.graphicsCSM.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart graphic;
        private System.Windows.Forms.ContextMenuStrip graphicsCSM;
        private System.Windows.Forms.ToolStripMenuItem drawingOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statisticsOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem combineToolStripMenuItem;

    }
}
