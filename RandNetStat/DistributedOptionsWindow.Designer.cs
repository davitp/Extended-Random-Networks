namespace RandNetStat
{
    partial class DistributedOptionsWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DistributedOptionsWindow));
            this.close = new System.Windows.Forms.Button();
            this.infoPanel = new System.Windows.Forms.Panel();
            this.researchInfo = new RandNetStat.ResearchInfo();
            this.graphicsPanel = new System.Windows.Forms.Panel();
            this.graphicsTab = new System.Windows.Forms.TabControl();
            this.save = new System.Windows.Forms.Button();
            this.saveAll = new System.Windows.Forms.Button();
            this.locationDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.infoPanel.SuspendLayout();
            this.graphicsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(787, 371);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 2;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // infoPanel
            // 
            this.infoPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.infoPanel.Controls.Add(this.researchInfo);
            this.infoPanel.Location = new System.Drawing.Point(12, 17);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Size = new System.Drawing.Size(270, 350);
            this.infoPanel.TabIndex = 4;
            // 
            // researchInfo
            // 
            this.researchInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.researchInfo.Location = new System.Drawing.Point(0, 0);
            this.researchInfo.Name = "researchInfo";
            this.researchInfo.Size = new System.Drawing.Size(270, 350);
            this.researchInfo.TabIndex = 0;
            // 
            // graphicsPanel
            // 
            this.graphicsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.graphicsPanel.Controls.Add(this.graphicsTab);
            this.graphicsPanel.Location = new System.Drawing.Point(288, 12);
            this.graphicsPanel.Name = "graphicsPanel";
            this.graphicsPanel.Size = new System.Drawing.Size(574, 353);
            this.graphicsPanel.TabIndex = 5;
            // 
            // graphicsTab
            // 
            this.graphicsTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphicsTab.Location = new System.Drawing.Point(0, 0);
            this.graphicsTab.Name = "graphicsTab";
            this.graphicsTab.SelectedIndex = 0;
            this.graphicsTab.Size = new System.Drawing.Size(574, 353);
            this.graphicsTab.TabIndex = 0;
            // 
            // save
            // 
            this.save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.save.Location = new System.Drawing.Point(625, 372);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(75, 23);
            this.save.TabIndex = 6;
            this.save.Text = "Save";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // saveAll
            // 
            this.saveAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveAll.Location = new System.Drawing.Point(706, 372);
            this.saveAll.Name = "saveAll";
            this.saveAll.Size = new System.Drawing.Size(75, 23);
            this.saveAll.TabIndex = 7;
            this.saveAll.Text = "Save All";
            this.saveAll.UseVisualStyleBackColor = true;
            this.saveAll.Click += new System.EventHandler(this.saveAll_Click);
            // 
            // DistributedOptionsWindow
            // 
            this.AcceptButton = this.save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(878, 407);
            this.Controls.Add(this.saveAll);
            this.Controls.Add(this.save);
            this.Controls.Add(this.graphicsPanel);
            this.Controls.Add(this.infoPanel);
            this.Controls.Add(this.close);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DistributedOptionsWindow";
            this.Load += new System.EventHandler(this.DistributedOptionsWindow_Load);
            this.infoPanel.ResumeLayout(false);
            this.graphicsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Panel infoPanel;
        private ResearchInfo researchInfo;
        private System.Windows.Forms.Panel graphicsPanel;
        private System.Windows.Forms.TabControl graphicsTab;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button saveAll;
        private System.Windows.Forms.FolderBrowserDialog locationDlg;


    }
}