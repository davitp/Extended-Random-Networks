using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

using Core.Settings;

namespace RandNet
{
    public partial class ProbabilityCalculatorWindow : Form
    {
        private SortedDictionary<double, double> results = new SortedDictionary<double, double>();

        public ProbabilityCalculatorWindow()
        {
            InitializeComponent();
        }

        private void calculate_Click(object sender, EventArgs e)
        {
            if ((branchingIndexTxt.Text.ToString() == "") ||
                (levelTxt.Text.ToString() == "") ||
                (minMuTxt.Text.ToString() == "") ||
                (maxMuTxt.Text.ToString() == "") ||
                (deltaTxt.Text.ToString() == ""))
            {
                MessageBox.Show("Some parameters are not specified.", "Error");
                return;
            }

            results.Clear();
            int p = int.Parse(branchingIndexTxt.Text.ToString());
            int level = int.Parse(levelTxt.Text.ToString());
            double mu = double.Parse(minMuTxt.Text.ToString(), CultureInfo.InvariantCulture);
            double muMax = double.Parse(maxMuTxt.Text.ToString(), CultureInfo.InvariantCulture);
            double delta = double.Parse(deltaTxt.Text.ToString(), CultureInfo.InvariantCulture);
            while (mu <= muMax)
            {
                results.Add(mu, CalculateProbability(p, level, mu));
                mu += delta;
            }

            FillResultsTable();

            save.Enabled = true;
        }

        private void save_Click(object sender, EventArgs e)
        {
            saveFileDlg.InitialDirectory = RandNetSettings.TracingDirectory;
            string fileName = null;
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDlg.FileName;
            }

            if (fileName == null)
            {
                return;
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            {
                file.WriteLine("BranchingIndex = " + branchingIndexTxt.Text.ToString());
                file.WriteLine("Level = " + levelTxt.Text.ToString());
                file.WriteLine("-");

                foreach (double m in results.Keys)
                {
                    file.WriteLine(Math.Round(m, 4) + "\t" + results[m]);
                }
            }
        }

        private double CalculateProbability(int p, int level, double mu)
        {
	        double s = Math.Pow((double)p, (double)level);
	        double l = (s - 1) * p;
	        double coeff = (p - 1) / l;

	        double b = Math.Pow(p, (1 - mu));

	        double sum = 0;
	        for(int i = 1; i <= level; ++i)
	        {
		        sum += Math.Pow(b, i);
	        }

	        return coeff * sum;
        }

        private void FillResultsTable()
        {
            resultsTable.Rows.Clear();

            foreach (double m in results.Keys)
            {
                int newRowIndex = resultsTable.Rows.Add();
                DataGridViewRow row = resultsTable.Rows[newRowIndex];
                row.Cells["muColumn"].Value = m.ToString();
                row.Cells["probabilityColumn"].Value = results[m].ToString();
            }
        }
    }
}
