namespace RandNetStat
{
    partial class StatisticsOptionsWindow
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
            this.cancel = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.drawOptionsGrp = new System.Windows.Forms.GroupBox();
            this.approximationType = new System.Windows.Forms.Label();
            this.thickeningType = new System.Windows.Forms.Label();
            this.approximationTypeCmb = new System.Windows.Forms.ComboBox();
            this.thickeningTypeCmb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.thickeningValueTxt = new System.Windows.Forms.TextBox();
            this.drawOptionsGrp.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(192, 133);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 8;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // ok
            // 
            this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok.Location = new System.Drawing.Point(111, 133);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 7;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            // 
            // drawOptionsGrp
            // 
            this.drawOptionsGrp.Controls.Add(this.thickeningValueTxt);
            this.drawOptionsGrp.Controls.Add(this.label1);
            this.drawOptionsGrp.Controls.Add(this.thickeningTypeCmb);
            this.drawOptionsGrp.Controls.Add(this.approximationTypeCmb);
            this.drawOptionsGrp.Controls.Add(this.thickeningType);
            this.drawOptionsGrp.Controls.Add(this.approximationType);
            this.drawOptionsGrp.Location = new System.Drawing.Point(12, 10);
            this.drawOptionsGrp.Name = "drawOptionsGrp";
            this.drawOptionsGrp.Size = new System.Drawing.Size(255, 117);
            this.drawOptionsGrp.TabIndex = 55;
            this.drawOptionsGrp.TabStop = false;
            // 
            // approximationType
            // 
            this.approximationType.AutoSize = true;
            this.approximationType.Location = new System.Drawing.Point(13, 18);
            this.approximationType.Name = "approximationType";
            this.approximationType.Size = new System.Drawing.Size(103, 13);
            this.approximationType.TabIndex = 0;
            this.approximationType.Text = "Approximation Type:";
            // 
            // thickeningType
            // 
            this.thickeningType.AutoSize = true;
            this.thickeningType.Location = new System.Drawing.Point(26, 50);
            this.thickeningType.Name = "thickeningType";
            this.thickeningType.Size = new System.Drawing.Size(90, 13);
            this.thickeningType.TabIndex = 1;
            this.thickeningType.Text = "Thickening Type:";
            // 
            // approximationTypeCmb
            // 
            this.approximationTypeCmb.FormattingEnabled = true;
            this.approximationTypeCmb.Location = new System.Drawing.Point(119, 15);
            this.approximationTypeCmb.Name = "approximationTypeCmb";
            this.approximationTypeCmb.Size = new System.Drawing.Size(121, 21);
            this.approximationTypeCmb.TabIndex = 2;
            // 
            // thickeningTypeCmb
            // 
            this.thickeningTypeCmb.FormattingEnabled = true;
            this.thickeningTypeCmb.Location = new System.Drawing.Point(119, 47);
            this.thickeningTypeCmb.Name = "thickeningTypeCmb";
            this.thickeningTypeCmb.Size = new System.Drawing.Size(121, 21);
            this.thickeningTypeCmb.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Thickening Value:";
            // 
            // thickeningValueTxt
            // 
            this.thickeningValueTxt.Location = new System.Drawing.Point(119, 80);
            this.thickeningValueTxt.Name = "thickeningValueTxt";
            this.thickeningValueTxt.Size = new System.Drawing.Size(121, 20);
            this.thickeningValueTxt.TabIndex = 5;
            this.thickeningValueTxt.Text = "0";
            this.thickeningValueTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.thickeningValueTxt.Validating += new System.ComponentModel.CancelEventHandler(this.thickeningValueTxt_Validating);
            // 
            // StatisticsOptionsWindow
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(279, 167);
            this.Controls.Add(this.drawOptionsGrp);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StatisticsOptionsWindow";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Statistics Options";
            this.drawOptionsGrp.ResumeLayout(false);
            this.drawOptionsGrp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.GroupBox drawOptionsGrp;
        private System.Windows.Forms.TextBox thickeningValueTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox thickeningTypeCmb;
        private System.Windows.Forms.ComboBox approximationTypeCmb;
        private System.Windows.Forms.Label thickeningType;
        private System.Windows.Forms.Label approximationType;
    }
}