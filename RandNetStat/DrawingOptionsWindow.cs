using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RandNetStat
{
    /// <summary>
    /// 
    /// </summary>
    public struct DrawingOption
    {
        public Color LineColor;
        public bool IsPoints;

        public DrawingOption(Color lineColor, bool isPoints)
        {
            LineColor = lineColor;
            IsPoints = isPoints;
        }
    };

    public partial class DrawingOptionsWindow : Form
    {
        public DrawingOptionsWindow()
        {
            InitializeComponent();
        }

        public Color LineColor
        {
            get
            {
                return color.BackColor;
            }
            set
            {
                color.BackColor = value;
            }
        }

        public bool Points
        {
            get
            {
                return pointsCheck.Checked;
            }
            set
            {
                pointsCheck.Checked = value;
            }
        }

        #region Event Handlers

        private void color_Click(object sender, EventArgs e)
        {
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                color.BackColor = colorDlg.Color;
            }
        }

        #endregion
    }
}
