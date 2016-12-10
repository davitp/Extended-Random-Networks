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
using Core.Enumerations;
using Session;
using Session.StatEngine;

namespace RandNetStat
{
    public partial class DistributedOptionsWindow : Form
    {
        private List<Guid> researches;
        private List<AnalyzeOption> options;

        public DistributedOptionsWindow(List<Guid> r, List<AnalyzeOption> o)
        {
            researches = r;
            options = o;

            InitializeComponent();
        }

        private void DistributedOptionsWindow_Load(object sender, EventArgs e)
        {
            Debug.Assert(researches.Count() != 0);
            Debug.Assert(options.Count() != 0);

            if (researches.Count() == 1)
                Text = StatSessionManager.GetResearchName(researches[0]);
            else
                Text = "Group Graphics";

            researchInfo.Researches = researches;
            foreach (Guid id in researches)
                StatSessionManager.LoadResearchResult(id);

            // TODO optimize
            StatisticResult st = new StatisticResult(researches);
            foreach (AnalyzeOption o in options)
            {
                st.CalculateDistributedOption(o);

                if (st.EnsembleResultsAvg[0].Result.ContainsKey(o))
                {
                    TabPage optTab = new TabPage(o.ToString());
                    GraphicsTab t = new GraphicsTab(o, (SortedDictionary<Double, Double>)st.EnsembleResultsAvg[0].Result[o]);
                    t.Dock = DockStyle.Fill;
                    optTab.Controls.Add(t);
                    graphicsTab.TabPages.Add(optTab);
                }
                if (graphicsTab.TabPages.Count == 0)
                    MessageBox.Show("None of checked options is calculated for selected researches.", "Information");
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (locationDlg.ShowDialog() == DialogResult.OK)
            {
                Debug.Assert(graphicsTab.SelectedTab.Controls.Count > 0);
                Debug.Assert(graphicsTab.SelectedTab.Controls[0] is GraphicsTab);
                GraphicsTab t = graphicsTab.SelectedTab.Controls[0] as GraphicsTab;
                t.SaveChartToPng(locationDlg.SelectedPath);
            }
        }

        private void saveAll_Click(object sender, EventArgs e)
        {
            if (locationDlg.ShowDialog() == DialogResult.OK)
            {
                foreach (TabPage t in graphicsTab.TabPages)
                {
                    Debug.Assert(t.Controls.Count > 0);
                    Debug.Assert(t.Controls[0] is GraphicsTab);
                    GraphicsTab g = t.Controls[0] as GraphicsTab;
                    g.SaveChartToPng(locationDlg.SelectedPath);
                }
            }
        }

        private void close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

        /*foreach (int i in resultsByGroups.Keys)
            {
                if (resultsByGroups[i].Contains(selectedId))
                {
                    Dictionary<AnalyzeOption, ChartProperties> p = new Dictionary<AnalyzeOption,ChartProperties>();
                    foreach(DataGridViewRow r in distributedOptionsTable.Rows)
                    {
                        DataGridViewCheckBoxCell cell = r.Cells["distributedCheckedColumn"] as DataGridViewCheckBoxCell;
                        if ((bool)(cell.Value) == true)
                        {
                            AnalyzeOption opt = (AnalyzeOption)Enum.Parse(typeof(AnalyzeOption), 
                                r.Cells["distributedNameColumn"].Value.ToString());
                            ThickeningType t = (ThickeningType)Enum.Parse(typeof(ThickeningType), 
                                r.Cells["thickeningTypeColumn"].Value.ToString());
                            ApproximationType at = (ApproximationType)Enum.Parse(typeof(ApproximationType), 
                                r.Cells["approximationColumn"].Value.ToString());
                            ChartProperties pv = new ChartProperties(t, 
                                Int32.Parse(r.Cells["thickeningValueColumn"].Value.ToString()), 
                                at,
                                color.BackColor);
                            p.Add(opt, pv);
                        }
                    }
                    if (p.Count() == 0)
                    {
                        MessageBox.Show("There are no checked options.", "Information");
                        return;
                    }
                    AnalyzeChartsWindow chartsWindow = new AnalyzeChartsWindow(i, p);
                    chartsWindow.Show();
                    return;
                }
            }*/