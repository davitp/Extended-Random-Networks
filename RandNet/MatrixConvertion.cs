using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using Core;
using Core.Utility;
using Core.Settings;
using Core.Model;
using Core.Exceptions;
using Core.Enumerations;

namespace RandNet
{
    public partial class MatrixConvertionWindow : Form
    {
        public MatrixConvertionWindow()
        {
            InitializeComponent();

            inputPathTxt.Text = outputPathTxt.Text = RandNetSettings.MatrixConvertionToolDirectory;
            sourceBrowserDlg.SelectedPath = targetBrowserDlg.SelectedPath = RandNetSettings.MatrixConvertionToolDirectory;
        }

        #region Event Handlers

        private void inputBrowse_Click(object sender, EventArgs e)
        {
            if (sourceBrowserDlg.ShowDialog() == DialogResult.OK)
            {
                inputPathTxt.Text = sourceBrowserDlg.SelectedPath;
                targetBrowserDlg.SelectedPath = sourceBrowserDlg.SelectedPath;
                outputPathTxt.Text = sourceBrowserDlg.SelectedPath;
                RandNetSettings.MatrixConvertionToolDirectory = sourceBrowserDlg.SelectedPath;
            }
        }

        private void outputBrowse_Click(object sender, EventArgs e)
        {
            if (targetBrowserDlg.ShowDialog() == DialogResult.OK)
                outputPathTxt.Text = targetBrowserDlg.SelectedPath;
        }

        private void convert_Click(object sender, EventArgs e)
        {
            EnableControls(false);

            try
            {
                foreach (string s in Directory.GetFiles(inputPathTxt.Text, "*.txt"))
                    Convert(s);

                MessageBox.Show("Successfully converted connectivity matrixes to degree-lists.");
            }
            catch (MatrixFormatException)
            {
                MessageBox.Show("One of input matrix's format is not correct.", "Error");
                inputPathTxt.Focus();
                inputPathTxt.SelectAll();
                return;
            }
            finally
            {
                EnableControls(true);
            }
        }

        private void MatrixConvertionWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            RandNetSettings.Refresh();
        }

        #endregion

        #region Utilities

        private void EnableControls(bool b)
        {
            inputPathTxt.Enabled = b;
            inputBrowse.Enabled = b;
            outputPathTxt.Enabled = b;
            outputBrowse.Enabled = b;
            convert.Enabled = b;
        }

        private void Convert(string fileName)
        {
            string sourceBranchesFilePath = fileName.Substring(0, fileName.Length - 4) + ".branches";
            string sourceActiveStatesFilePath = fileName.Substring(0, fileName.Length - 4) + ".actives";
            NetworkInfoToRead network = FileManager.Read(fileName, 0);
            Debug.Assert(network is MatrixInfoToRead);
            ArrayList m = (network as MatrixInfoToRead).Matrix;
            fileName = Path.GetFileName(fileName);
            int l = fileName.Length;
            string outFileName = outputPathTxt.Text + "\\" + fileName.Substring(0, l - 4) + "_out.txt";
            string outBranchesFileName = outputPathTxt.Text + "\\" + fileName.Substring(0, l - 4) + "_out.branches";
            string outActiveStatesFileName = outputPathTxt.Text + "\\" + fileName.Substring(0, l - 4) + "_out.actives";
            using (StreamWriter file = new StreamWriter(outFileName))
            {
                ArrayList row = new ArrayList();
                for (int i = 0; i < m.Count - 1; ++i)
                {
                    row = (ArrayList)m[i];
                    for (int j = i; j < m.Count; ++j)
                    {
                        if ((bool)row[j] == true)
                            file.WriteLine(i.ToString() + " " + j.ToString());
                    }
                }
            }
            if (network.Branches != null)
                File.Copy(sourceBranchesFilePath, outBranchesFileName);
            if (network.ActiveStates != null)
                File.Copy(sourceActiveStatesFilePath, outActiveStatesFileName);
        }

        #endregion
    }
}
