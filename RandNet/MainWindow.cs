using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;

using Microsoft.Practices.EnterpriseLibrary.Logging;

using Session;
using Core;
using Core.Exceptions;
using Core.Enumerations;
using Core.Attributes;
using Core.Events;
using Core.Settings;
using Core.Utility;

namespace RandNet
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            RandNetSettings.InitializeLogging("randnet_" + DateTime.Now.AddHours(4).ToString("yyyy_MM_dd_HH_mm_ss"));
            Logger.Write("-------------------------------- xRandNet STARTED --------------------------------");

            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        #region Event Handlers

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CheckClosing())
                e.Cancel = true;
            Logger.Write("-------------------------------- xRandNet FINISHED --------------------------------");
        }

        private void basicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowCreateResearchDialog(ResearchType.Basic);
        }

        private void evolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowCreateResearchDialog(ResearchType.Evolution);
        }

        private void percolationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowCreateResearchDialog(ResearchType.Percolation);
        }

        private void basicCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowCreateResearchDialog(ResearchType.Collection);
        }

        private void structuralToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowCreateResearchDialog(ResearchType.Structural);
        }

        private void editResearch_Click(object sender, EventArgs e)
        {
            Debug.Assert(researchesTable.SelectedRows.Count == 1);
            Guid id = GetSelectedResearchId();
            CreateEditResearchWindow editWindow = new CreateEditResearchWindow(DialogType.Edit,
                SessionManager.GetResearchType(id), id);
            editWindow.ShowDialog(this);
            if (editWindow.DialogResult == DialogResult.OK)
                UpdateSelectedResearchInTable();
        }

        private void deleteResearch_Click(object sender, EventArgs e)
        {
            Debug.Assert(researchesTable.SelectedRows.Count == 1);
            RemoveSelectedResearch();
        }

        private void cloneResearch_Click(object sender, EventArgs e)
        {
            Debug.Assert(researchesTable.SelectedRows.Count == 1);
            Guid id = GetSelectedResearchId();
            CreateEditResearchWindow cloneWindow = new CreateEditResearchWindow(DialogType.Clone,
                SessionManager.GetResearchType(id), id);
            cloneWindow.ShowDialog(this);
            if (cloneWindow.DialogResult == DialogResult.OK)
                AddResearchToTable(cloneWindow.ResultResearchId);
        }

        private void settingsMenuItem_Click(object sender, EventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog(this);
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            if(CheckClosing())
                Application.Exit();
        }

        private void dataConvertionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataConvertionsWindow dataConvertionsWnd = new DataConvertionsWindow();
            dataConvertionsWnd.Show();
        }

        private void matrixConvertionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MatrixConvertionWindow matrixConvertionWnd = new MatrixConvertionWindow();
            matrixConvertionWnd.Show();
        }

        private void probabilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProbabilityCalculatorWindow probabilityCalculatorWnd = new ProbabilityCalculatorWindow();
            probabilityCalculatorWnd.Show();
        }

        private void modelCheckingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModelCheckWindow modelCheckWnd = new ModelCheckWindow();
            modelCheckWnd.Show();
        }

        private void statisticAnalyzerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("RandNetStat.exe");
        }

        private void classicalTestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logger.Write("-------------------------------- CLASSICAL TESTS STARTED --------------------------------");
            RunTestsWindow w = new RunTestsWindow();
            w.ModelType = ModelType.ER;
            w.ShowDialog();
            Logger.Write("-------------------------------- CLASSICAL TESTS FINISHED --------------------------------");
        }

        private void regularHierarchicTestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logger.Write("-------------------------------- REGULAR HIERARCHIC TESTS STARTED --------------------------------");
            RunTestsWindow w = new RunTestsWindow();
            w.ModelType = ModelType.RegularHierarchic;
            w.ShowDialog();
            Logger.Write("-------------------------------- REGULAR HIERARCHIC TESTS FINISHED --------------------------------");
        }

        private void nonRegularHierarchicTestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logger.Write("-------------------------------- NON REGULAR HIERARCHIC TESTS STARTED --------------------------------");
            RunTestsWindow w = new RunTestsWindow();
            w.ModelType = ModelType.NonRegularHierarchic;
            w.ShowDialog();
            Logger.Write("-------------------------------- NON REGULAR HIERARCHIC TESTS FINISHED --------------------------------");
        }

        private void researchTable_SelectionChanged(object sender, EventArgs e)
        {
            Debug.Assert(researchesTable.SelectedRows.Count <= 1);
            if (researchesTable.SelectedRows.Count == 1)
                FillResearchInformation(GetSelectedResearchId());
        }

        private void researchTable_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            DataGridView.HitTestInfo hit = researchesTable.HitTest(e.X, e.Y);
            if (hit.RowIndex != -1)
            {
                researchesTable.Rows[hit.RowIndex].Selected = true;
                InitializeCSM(false);
            }
            else
            {
                InitializeCSM(true);
            }
            researchTableCSM.Show(researchesTable, e.X, e.Y);
        }

        private void startstop_Click(object sender, EventArgs e)
        {
            Debug.Assert(researchesTable.SelectedRows.Count <= 1);
            if (researchesTable.SelectedRows.Count == 0)
                return;
            Guid id = GetSelectedResearchId();
            Debug.Assert(SessionManager.ResearchExists(id));
            if (startstop.Text == "Start")
            {
                StartSelectedResearch(id);
                startstop.Text = "Stop";
                startstop.Enabled = true;
                statusProgress.Visible = true;
            }
            else if (startstop.Text == "Stop")
            {
                StopSelectedResearch(id);
                startstop.Text = "Start";
                startstop.Enabled = false;
            }
        }

        private void StartSelectedResearch(Guid id)
        {
            try
            {
                SessionManager.AddResearchUpdateHandler(id, UpdateStatusesInTableAndInPanel);
                SessionManager.StartResearch(id);
            }
            catch (CoreException ex)
            {
                MessageBox.Show("Unable to start research: " + ex.Message, "Error");
            }
        }

        private void StopSelectedResearch(Guid id)
        {
            try
            {
                SessionManager.StopResearch(id);
                SessionManager.RemoveResearchUpdateHandler(id, UpdateStatusesInTableAndInPanel);
            }
            catch (CoreException ex)
            {
                MessageBox.Show("Unable to stop research: " + ex.Message, "Error");
            }
        }

        #endregion

        #region Utilities

        private bool CheckClosing()
        {
            if (SessionManager.ExistsAnyRunningResearch())
            {
                DialogResult res = 
                    MessageBox.Show("There are running researches. \nDo you want to abort them and close?",
                    "Warning",
                    MessageBoxButtons.OKCancel);
                if (DialogResult.OK == res)
                {
                    try
                    {
                        SessionManager.StopAllRunningResearches();
                    }
                    catch (CoreException ex)
                    {
                        MessageBox.Show("Unable to stop 1 or more research: " + ex.Message, "Error");
                        return false;
                    }
                    return true;
                }
                else if (DialogResult.Cancel == res)
                    return false;
            }

            return true;
        }

        private void ShowCreateResearchDialog(ResearchType rt)
        {
            CreateEditResearchWindow d = new CreateEditResearchWindow(DialogType.Create, rt);
            d.ShowDialog(this);
            if (d.DialogResult == DialogResult.OK)
                AddResearchToTable(d.ResultResearchId);
        }

        private void AddResearchToTable(Guid id)
        {
            Debug.Assert(SessionManager.ResearchExists(id));
            int i = researchesTable.Rows.Add(id, SessionManager.GetResearchType(id),
                SessionManager.GetResearchName(id),
                SessionManager.GetResearchModelType(id).ToString(),
                SessionManager.GetResearchStorageType(id).ToString(),
                SessionManager.GetResearchGenerationType(id).ToString(),
                (SessionManager.GetResearchTracingPath(id) != ""),
                SessionManager.GetResearchRealizationCount(id).ToString(),
                ResearchStatus.NotStarted);

            researchesTable.Rows[i].Selected = true;
        }

        private void UpdateSelectedResearchInTable()
        {
            Debug.Assert(researchesTable.SelectedRows.Count == 1);
            Guid id = GetSelectedResearchId();
            DataGridViewRow r = researchesTable.SelectedRows[0];
            r.Cells["nameColumn"].Value = SessionManager.GetResearchName(id);
            r.Cells["modelColumn"].Value = SessionManager.GetResearchModelType(id).ToString();
            r.Cells["storageColumn"].Value = SessionManager.GetResearchStorageType(id).ToString();
            r.Cells["generationColumn"].Value = SessionManager.GetResearchGenerationType(id).ToString();
            r.Cells["tracingColumn"].Value = (SessionManager.GetResearchTracingPath(id) != "");
            r.Cells["realizationsColumn"].Value = SessionManager.GetResearchRealizationCount(id).ToString();

            FillParametersTable(id);
            FillAnalyzeOptionsTable(id);
            FillStatusPanel(id);
        }

        private Guid GetSelectedResearchId()
        {
            Debug.Assert(researchesTable.SelectedRows.Count != 0);
            return (Guid)researchesTable.SelectedRows[0].Cells[0].Value;
        }

        private void InitializeCSM(bool creation)
        {
            researchTableCSM.Items["createResearch"].Enabled = creation;
            if (creation)
            {
                researchTableCSM.Items["editResearch"].Enabled = false;
                researchTableCSM.Items["deleteResearch"].Enabled = false;
            }
            else
            {
                Debug.Assert(researchesTable.SelectedRows.Count == 1);
                Guid id = GetSelectedResearchId();
                ResearchStatus s = SessionManager.GetResearchStatus(id).Status;
                researchTableCSM.Items["editResearch"].Enabled = (s == ResearchStatus.NotStarted);
                researchTableCSM.Items["deleteResearch"].Enabled = (s != ResearchStatus.Running);
            }
            researchTableCSM.Items["cloneResearch"].Enabled = !creation;
        }

        private void FillResearchInformation(Guid id)
        {
            FillParametersTable(id);
            FillAnalyzeOptionsTable(id);
            FillStatusPanel(id);
        }

        private void RemoveSelectedResearch()
        {
            Guid id = GetSelectedResearchId();
            Debug.Assert(SessionManager.GetResearchStatus(id).Status != ResearchStatus.Running);
            SessionManager.DestroyResearch(id);

            researchesTable.Rows.RemoveAt(researchesTable.SelectedRows[0].Index);
            if (researchesTable.Rows.Count == 0)
            {
                parametersTable.Rows.Clear();
                analyzeOptionsTable.Rows.Clear();
            }
            else
                researchesTable.Rows[0].Selected = true;
        }

        private void FillParametersTable(Guid researchId)
        {
            parametersTable.Rows.Clear();

            Dictionary<GenerationParameter, object> gValues =
                SessionManager.GetGenerationParameterValues(researchId);
            Dictionary<ResearchParameter, object> rValues =
                SessionManager.GetResearchParameterValues(researchId);

            foreach (ResearchParameter r in rValues.Keys)
            {
                object v = rValues[r];
                if (v != null)
                    parametersTable.Rows.Add(r.ToString(), v.ToString());
                else
                    parametersTable.Rows.Add(r.ToString());
            }

            foreach (GenerationParameter g in gValues.Keys)
            {
                object v = gValues[g];
                if (v != null)
                    parametersTable.Rows.Add(g.ToString(), v.ToString());
                else
                    parametersTable.Rows.Add(g.ToString());
            }
        }

        private void FillAnalyzeOptionsTable(Guid researchId)
        {
            analyzeOptionsTable.Rows.Clear();

            AnalyzeOption availableOptions = SessionManager.GetAvailableAnalyzeOptions(researchId);
            AnalyzeOption checkedOptions = SessionManager.GetAnalyzeOptions(researchId);

            Array existingOptions = Enum.GetValues(typeof(AnalyzeOption));
            foreach (AnalyzeOption opt in existingOptions)
            {
                if ((availableOptions & opt) == opt && opt != AnalyzeOption.None)
                {
                    if((checkedOptions & opt) == opt)
                        analyzeOptionsTable.Rows.Add(opt.ToString(), true);
                    else
                        analyzeOptionsTable.Rows.Add(opt.ToString(), false);
                }
            }
        }

        private void FillStatusPanel(Guid id)
        {
            ResearchStatus rs = SessionManager.GetResearchStatus(id).Status;
            if (rs == ResearchStatus.Running)
            {
                startstop.Enabled = true;
                startstop.Text = "Stop";
                statusProgress.Visible = true;
            }
            else if (rs == ResearchStatus.NotStarted)
            {
                startstop.Enabled = true;
                startstop.Text = "Start";
                statusProgress.Visible = false;
            }
            else
            {
                startstop.Enabled = false;
                startstop.Text = "Start";
                statusProgress.Visible = true;
            }
            statusProgress.Maximum = SessionManager.GetProcessStepsCount(id);
            statusProgress.Value = (int)SessionManager.GetResearchStatus(id).CompletedStepsCount;
        }

        private void UpdateStatusesInTableAndInPanel(object sender, ResearchEventArgs e)
        {
            Guid id = e.ResearchID;
            ResearchStatusInfo rsi = SessionManager.GetResearchStatus(id);
            string status = rsi.Status.ToString() + " ";
            double percent = rsi.CompletedStepsCount * 100.0 / SessionManager.GetProcessStepsCount(id);
            status += Math.Round(percent, 1).ToString() + "%";

            DataGridViewRow r = FindRowInTableById(id);
            Debug.Assert(r != null);
            r.Cells["statusColumn"].Value = status;

            if (GetSelectedResearchId() == id)
            {
                statusProgress.Visible = true;
                statusProgress.Maximum = SessionManager.GetProcessStepsCount(id);
                statusProgress.Value = (int)rsi.CompletedStepsCount;
            }
        }

        private DataGridViewRow FindRowInTableById(Guid id)
        {
            foreach (DataGridViewRow r in researchesTable.Rows)
            {
                if ((Guid)r.Cells[0].Value == id)
                    return r;
            }
            return null;
        }

        #endregion
    }
}
