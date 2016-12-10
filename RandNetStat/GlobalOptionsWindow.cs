using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Core;
using Core.Result;
using Core.Enumerations;
using Session;
using Session.StatEngine;

namespace RandNetStat
{
    public partial class GlobalOptionsWindow : Form
    {
        private List<Guid> researches;
        private List<AnalyzeOption> options;
        private StatisticResult statisticResults;

        public GlobalOptionsWindow(List<Guid> r, List<AnalyzeOption> o)
        {
            researches = r;
            options = o;

            InitializeComponent();
        }

        private void GlobalOptionsWindow_Load(object sender, EventArgs e)
        {
            Debug.Assert(researches.Count() != 0);
            Debug.Assert(options.Count() != 0);

            if (researches.Count() == 1)
                Text = StatSessionManager.GetResearchName(researches[0]);
            else
                Text = "Group Values";

            researchInfo.Researches = researches;
            foreach (Guid id in researches)
                StatSessionManager.LoadResearchResult(id);

            // TODO optimize
            StatisticResult st = new StatisticResult(researches);
            foreach (AnalyzeOption o in options)
            {
                st.CalculateGlobalOption(o);
                if (st.EnsembleResultsAvg[0].Result.ContainsKey(o))
                    valuesGrd.Rows.Add(o.ToString(), st.EnsembleResultsAvg[0].Result[o].ToString());
            }
            if (valuesGrd.Rows.Count == 0)
                MessageBox.Show("None of checked options is calculated for selected researches.", "Information");
        }

        private void close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (locationDlg.ShowDialog() == DialogResult.OK)
            {
                //t.SaveChartToPng(locationDlg.SelectedPath);
            }
        }
    }
}
