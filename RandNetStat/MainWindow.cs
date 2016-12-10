using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Session;
using Core.Enumerations;
using Core.Attributes;
using Core.Result;

namespace RandNetStat
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();

            InitializeResearchType();
        }

        #region Event Handlers

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            if (settingsWindow.ShowDialog(this) == DialogResult.OK)
            {
                StatSessionManager.Clear();
                InitializeResearchType();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void researchTypeCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeModelType();
        }

        private void modelTypeCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillOptionsPanels();
            FillResearchesTable();
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            StatSessionManager.RefreshExistingResults();
            FillResearchesTable();
        }

        private void researchesTable_SelectionChanged(object sender, EventArgs e)
        {
            if (researchesTable.SelectedRows.Count > 0)
                FillGroupParameters();
        }

        private void researchesTable_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            DataGridView.HitTestInfo hit = researchesTable.HitTest(e.X, e.Y);
            if (hit.RowIndex != -1)
            {
                foreach (DataGridViewRow r in researchesTable.SelectedRows)
                    r.Selected = false;
                researchesTable.Rows[hit.RowIndex].Selected = true;
                researchTableCSM.Items["eraseResearch"].Enabled = true;
                researchTableCSM.Items["selectGroup"].Enabled = true;
            }
            else
            {
                researchTableCSM.Items["eraseResearch"].Enabled = false;
                researchTableCSM.Items["selectGroup"].Enabled = false;
            }
            researchTableCSM.Show(researchesTable, e.X, e.Y);
        }

        private void eraseResearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.Assert(researchesTable.SelectedRows.Count == 1);
            Guid id = GetSelectedResearchId();
            StatSessionManager.DeleteResearch(id);
            FillResearchesTable();
        }

        private void selectGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Debug.Assert(researchesTable.SelectedRows.Count == 1);
            Guid id = GetSelectedResearchId();
            Dictionary<int, List<Guid>> r = StatSessionManager.GetFilteredResultsByGroups(GetCurrentResearchType(),
                GetCurrentModelType());
            foreach (int rid in r.Keys)
                if (r[rid].Contains(id))
                    foreach (Guid j in r[rid])
                        foreach (DataGridViewRow row in researchesTable.Rows)
                        {
                            if ((Guid)row.Cells[0].Value == j)
                                row.Selected = true;
                        }
        }

        private void selectAll_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Parent.Name == "globalOptionsGrp")
            {
                ChangeCheckedStatesForAnalyzeOptions(globalOptionsPanel, true);
            }
            else if (btn.Parent.Name == "distributedOptionsGrp")
            {
                ChangeCheckedStatesForAnalyzeOptions(distributedOptionsPanel, true);
            }
            else
                Debug.Assert(false);
        }

        private void deselectAll_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Parent.Name == "globalOptionsGrp")
            {
                ChangeCheckedStatesForAnalyzeOptions(globalOptionsPanel, false);
            }
            else if (btn.Parent.Name == "distributedOptionsGrp")
            {
                ChangeCheckedStatesForAnalyzeOptions(distributedOptionsPanel, false);
            }
            else
                Debug.Assert(false);
        }

        private void showValues_Click(object sender, EventArgs e)
        {
            List<Guid> researchesToShow = GetSelectedResearchesIds();
            List<AnalyzeOption> optionsToShow = GetCheckedOptions(globalOptionsPanel);
            if (researchesToShow.Count() == 0 || optionsToShow.Count() == 0)
            {
                MessageBox.Show("There are no selected researches or checked options.", "Information");
                return;
            }
            GlobalOptionsWindow w = new GlobalOptionsWindow(researchesToShow, optionsToShow);
            w.Show();
        }

        private void showGraphics_Click(object sender, EventArgs e)
        {
            List<Guid> researchesToShow = GetSelectedResearchesIds();
            List<AnalyzeOption> optionsToShow = GetCheckedOptions(distributedOptionsPanel);
            if (researchesToShow.Count() == 0 || optionsToShow.Count() == 0)
            {
                MessageBox.Show("There are no selected researches or checked options.", "Information");
                return;
            }
            DistributedOptionsWindow w = new DistributedOptionsWindow(researchesToShow, optionsToShow);
            w.Show();

        }

        #endregion

        #region Utilities

        private void InitializeResearchType()
        {
            researchTypeCmb.Items.Clear();
            /*researchTypeCmb.Sorted = true;
            string[] researchTypeNames = Enum.GetNames(typeof(ResearchType));
            for (int i = 0; i < researchTypeNames.Length; ++i)
                researchTypeCmb.Items.Add(researchTypeNames[i]);*/

            researchTypeCmb.Items.Add(ResearchType.Basic.ToString());
            if (researchTypeCmb.Items.Count != 0)
                researchTypeCmb.SelectedIndex = 0;
        }

        private void InitializeModelType()
        {
            modelTypeCmb.Items.Clear();
            modelTypeCmb.Sorted = true;
            foreach (ModelType m in StatSessionManager.GetAvailableModelTypes(GetCurrentResearchType()))
                modelTypeCmb.Items.Add(m.ToString());

            if (modelTypeCmb.Items.Count != 0)
                modelTypeCmb.SelectedIndex = 0;
        }

        private ResearchType GetCurrentResearchType()
        {
            if (researchTypeCmb.Text == "")
                return ResearchType.Basic;
            return (ResearchType)Enum.Parse(typeof(ResearchType), researchTypeCmb.Text);
        }

        private ModelType GetCurrentModelType()
        {
            if (modelTypeCmb.Text == "")
                return ModelType.BA;
            return (ModelType)Enum.Parse(typeof(ModelType), modelTypeCmb.Text);
        }

        private Guid GetSelectedResearchId()
        {
            Debug.Assert(researchesTable.SelectedRows.Count != 0);
            return (Guid)researchesTable.SelectedRows[0].Cells[0].Value;
        }

        private void FillOptionsPanels()
        {
            FillOptionsPanel(globalOptionsPanel, OptionType.Global);
            FillOptionsPanel(distributedOptionsPanel, OptionType.Distribution);
        }

        private void FillOptionsPanel(Panel p, OptionType ot)
        {
            p.Controls.Clear();

            AnalyzeOption availableOptions = StatSessionManager.GetAvailableAnalyzeOptions(GetCurrentResearchType(),
                GetCurrentModelType());

            Array existingOptions = Enum.GetValues(typeof(AnalyzeOption));
            int location = 0;
            foreach (AnalyzeOption opt in existingOptions)
            {
                if ((availableOptions & opt) == opt && opt != AnalyzeOption.None)
                {
                    if (!StatSessionManager.HasOptionType(opt, ot))
                        continue;
                    CheckBox cb = new CheckBox();
                    cb.Name = opt.ToString();
                    cb.Text = opt.ToString();
                    cb.AutoSize = true;
                    cb.Location = new Point(10, (location++) * 20 + 5);
                    cb.Checked = false;
                    p.Controls.Add(cb);
                }
            }
        }

        private void FillResearchesTable()
        {
            researchesTable.Rows.Clear();

            ResearchType rt = GetCurrentResearchType();
            ModelType mt = GetCurrentModelType();
            Dictionary<int, List<Guid>> r = StatSessionManager.GetFilteredResultsByGroups(rt, mt);

            Color c = Color.WhiteSmoke;
            foreach (int i in r.Keys)
            {
                foreach(Guid id in r[i])
                {
                    int newRowIndex = researchesTable.Rows.Add(id,
                        StatSessionManager.GetResearchName(id),
                        StatSessionManager.GetResearchRealizationCount(id).ToString(),
                        StatSessionManager.GetResearchNetworkSize(id).ToString());
                    DataGridViewRow row = researchesTable.Rows[newRowIndex];
                    row.DefaultCellStyle.BackColor = c;
                }
                
                c = (c == Color.WhiteSmoke) ? Color.LightGray : Color.WhiteSmoke;
            }
        }

        private void FillGroupParameters()
        {
            Debug.Assert(researchesTable.SelectedRows.Count >= 0);
            Guid id = GetSelectedResearchId();

            parametersTable.Rows.Clear();

            Dictionary<GenerationParameter, object> gValues = StatSessionManager.GetGenerationParameterValues(id);
            Dictionary<ResearchParameter, object> rValues = StatSessionManager.GetResearchParameterValues(id);
            foreach (ResearchParameter r in rValues.Keys)
                parametersTable.Rows.Add(r.ToString(), rValues[r].ToString());
            foreach (GenerationParameter g in gValues.Keys)
                parametersTable.Rows.Add(g.ToString(), gValues[g].ToString());
        }

        private void ChangeCheckedStatesForAnalyzeOptions(Panel p, bool b)
        {
            foreach (Control c in p.Controls)
            {
                Debug.Assert(c is CheckBox);
                CheckBox cc = c as CheckBox;
                cc.Checked = b;
            }
        }

        private List<Guid> GetSelectedResearchesIds()
        {
            List<Guid> r = new List<Guid>();
            foreach (DataGridViewRow row in researchesTable.SelectedRows)
                r.Add((Guid)row.Cells[0].Value);

            return r;
        }

        private List<AnalyzeOption> GetCheckedOptions(Panel p)
        {
            List<AnalyzeOption> o = new List<AnalyzeOption>();
            foreach (Control c in p.Controls)
            {
                Debug.Assert(c is CheckBox);
                CheckBox cc = c as CheckBox;
                AnalyzeOption current = (AnalyzeOption)Enum.Parse(typeof(AnalyzeOption), cc.Name);
                if (cc.Checked)
                    o.Add(current);
            }

            return o;
        }

        #endregion
    }
}
