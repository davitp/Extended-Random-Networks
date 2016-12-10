using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Core.Settings;
using ModelChecking;

namespace RandNet
{
    // Реализация формы для проверки соответствия модели.
    public partial class ModelCheckWindow : Form
    {
        public ModelCheckWindow()
        {
            InitializeComponent();
        }

        // Обработчики сообщений.

        private void ModelCheckWindow_Load(object sender, EventArgs e)
        {
            this.degreesRadio.Checked = true;
        }

        private void degreesRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (this.degreesRadio.Checked == true)
            {
                this.degrees.Enabled = true;
                this.degreesTxt.Enabled = true;
            }
            else
            {
                this.degrees.Enabled = false;
                this.degreesTxt.Enabled = false;
            }
        }

        private void fromFileRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (this.fromFileRadio.Checked == true)
            {
                this.notExactFilePath.Enabled = true;
                this.notExactFilePathTxt.Enabled = true;
                this.notExactBrowseBtn.Enabled = true;
            }
            else
            {
                this.notExactFilePath.Enabled = false;
                this.notExactFilePathTxt.Enabled = false;
                this.notExactBrowseBtn.Enabled = false;
            }
        }

        private void notExactBrowseBtn_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = RandNetSettings.ModelCheckingToolDirectory;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                RandNetSettings.ModelCheckingToolDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                RandNetSettings.Refresh();
                this.notExactFilePathTxt.Text = openFileDialog.FileName;
            }
        }

        private void exactBrowseBtn_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = RandNetSettings.ModelCheckingToolDirectory;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                RandNetSettings.ModelCheckingToolDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                RandNetSettings.Refresh();
                this.exactFilePathTxt.Text = openFileDialog.FileName;
            }
        }

        private void notExactCheckBtn_Click(object sender, EventArgs e)
        {
            HierarchicChecker checker;
            if (this.degreesRadio.Checked == true)
            {
                checker = new HierarchicChecker(ParceDegrees());
            }
            else
            {
                checker = new HierarchicChecker(this.notExactFilePathTxt.Text);
                FillDegrees(checker.FromMatrixToDegrees());
            }

            this.notExactResultTxt.Text = checker.IsHierarchic().ToString();
        }

        private void exactCheckBtn_Click(object sender, EventArgs e)
        {
            HierarchicExactChecker checker = new HierarchicExactChecker();
            bool result = checker.IsHierarchic(this.exactFilePathTxt.Text);
            this.exactResultTxt.Text = result ? "Is Hierarchic" : "Is Not Hierarchic";
        }

        // Утилиты.

        private List<int> ParceDegrees()
        {
            List<int> degreeList = new List<int>();
            string degrees = this.degreesTxt.Text.ToString();
            string d = "";
            for (int i = 0; i < degrees.Length; ++i)
            {
                if(Char.IsDigit(degrees[i]))
                    d += degrees[i].ToString();
                else if (degrees[i] == ',')
                {
                    degreeList.Add(Convert.ToInt32(d));
                    d = "";
                }
            }
            degreeList.Add(Convert.ToInt32(d));

            return degreeList;
        }

        private void FillDegrees(List<int> degrees)
        {
            string d = "";
            for (int i = 0; i < degrees.Count; ++i)
            {
                if (i != degrees.Count - 1)
                    d += degrees[i].ToString() + ", ";
                else
                    d += degrees[i].ToString();
            }
            this.degreesTxt.Text = d;
        }
    }
}
