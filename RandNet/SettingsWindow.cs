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
using Core;
using Core.Enumerations;
using Core.Settings;

namespace RandNet
{
    public partial class SettingsWindow : Form
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        #region Event Handlers

        private void Settings_Load(object sender, EventArgs e)
        {
            loggingDirectoryTxt.Text = RandNetSettings.LoggingDirectory;
            storageDirectoryTxt.Text = RandNetSettings.StorageDirectory;
            //databaseTxt.Text = ExplorerSettings.ConnectionString;
            tracingDirectoryTxt.Text = RandNetSettings.TracingDirectory;
            if (RandNetSettings.TracingType == TracingType.Matrix)
                matrixRadio.Checked = true;
            else if (RandNetSettings.TracingType == TracingType.Neighbourship)
                neighbourshipRadio.Checked = true;
        }

        private void loggingBrowseButton_Click(object sender, EventArgs e)
        {
            browserDlg.SelectedPath = RandNetSettings.LoggingDirectory;
            if (browserDlg.ShowDialog() == DialogResult.OK)
            {
                loggingDirectoryTxt.Text = browserDlg.SelectedPath;
            }
        }

        private void storageBrowseButton_Click(object sender, EventArgs e)
        {
            browserDlg.SelectedPath = RandNetSettings.StorageDirectory;
            if (browserDlg.ShowDialog() == DialogResult.OK)
            {
                storageDirectoryTxt.Text = browserDlg.SelectedPath;
            }
        }

        private void tracingBrowseBtn_Click(object sender, EventArgs e)
        {
            browserDlg.SelectedPath = RandNetSettings.TracingDirectory;
            if (browserDlg.ShowDialog() == DialogResult.OK)
            {
                tracingDirectoryTxt.Text = browserDlg.SelectedPath;
            }
        }

        private void SaveSettingsButton_Click(object sender, EventArgs e)
        {
            RandNetSettings.LoggingDirectory = loggingDirectoryTxt.Text;
            RandNetSettings.StorageDirectory = storageDirectoryTxt.Text;
            //Settings.ConnectionString = textBoxConnStr.Text;
            RandNetSettings.TracingDirectory = tracingDirectoryTxt.Text;
            if (matrixRadio.Checked)
                RandNetSettings.TracingType = TracingType.Matrix;
            else if (neighbourshipRadio.Checked)
                RandNetSettings.TracingType = TracingType.Neighbourship;

            RandNetSettings.Refresh();
            Close();
        }

        #endregion 
    }
}
