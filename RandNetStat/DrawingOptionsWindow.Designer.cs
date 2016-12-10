namespace RandNetStat
{
    partial class DrawingOptionsWindow
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
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.drawOptionsGrp = new System.Windows.Forms.GroupBox();
            this.lineColor = new System.Windows.Forms.Label();
            this.pointsCheck = new System.Windows.Forms.CheckBox();
            this.color = new System.Windows.Forms.Button();
            this.colorDlg = new System.Windows.Forms.ColorDialog();
            this.drawOptionsGrp.SuspendLayout();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok.Location = new System.Drawing.Point(12, 91);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 5;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(93, 91);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 6;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // drawOptionsGrp
            // 
            this.drawOptionsGrp.Controls.Add(this.lineColor);
            this.drawOptionsGrp.Controls.Add(this.pointsCheck);
            this.drawOptionsGrp.Controls.Add(this.color);
            this.drawOptionsGrp.Location = new System.Drawing.Point(12, 12);
            this.drawOptionsGrp.Name = "drawOptionsGrp";
            this.drawOptionsGrp.Size = new System.Drawing.Size(156, 73);
            this.drawOptionsGrp.TabIndex = 54;
            this.drawOptionsGrp.TabStop = false;
            // 
            // lineColor
            // 
            this.lineColor.AutoSize = true;
            this.lineColor.Location = new System.Drawing.Point(21, 16);
            this.lineColor.Name = "lineColor";
            this.lineColor.Size = new System.Drawing.Size(57, 13);
            this.lineColor.TabIndex = 3;
            this.lineColor.Text = "Line Color:";
            // 
            // pointsCheck
            // 
            this.pointsCheck.AutoSize = true;
            this.pointsCheck.Checked = true;
            this.pointsCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pointsCheck.Location = new System.Drawing.Point(51, 45);
            this.pointsCheck.Name = "pointsCheck";
            this.pointsCheck.Size = new System.Drawing.Size(55, 17);
            this.pointsCheck.TabIndex = 1;
            this.pointsCheck.Text = "Points";
            this.pointsCheck.UseVisualStyleBackColor = true;
            // 
            // color
            // 
            this.color.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.color.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.color.Location = new System.Drawing.Point(81, 11);
            this.color.Name = "color";
            this.color.Size = new System.Drawing.Size(55, 23);
            this.color.TabIndex = 0;
            this.color.UseVisualStyleBackColor = false;
            this.color.Click += new System.EventHandler(this.color_Click);
            // 
            // colorDlg
            // 
            this.colorDlg.FullOpen = true;
            // 
            // DrawingOptionsWindow
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(179, 127);
            this.Controls.Add(this.drawOptionsGrp);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DrawingOptionsWindow";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Drawing Options";
            this.drawOptionsGrp.ResumeLayout(false);
            this.drawOptionsGrp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.GroupBox drawOptionsGrp;
        private System.Windows.Forms.Label lineColor;
        private System.Windows.Forms.CheckBox pointsCheck;
        private System.Windows.Forms.Button color;
        private System.Windows.Forms.ColorDialog colorDlg;

    }
}