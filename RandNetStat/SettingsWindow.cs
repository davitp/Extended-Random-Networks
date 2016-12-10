using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Core.Enumerations;
using Core.Settings;
using Session;

namespace RandNetStat
{
    public partial class SettingsWindow : Form
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        #region Event Handlers

        private void SettingsWindow_Load(object sender, EventArgs e)
        {
            InitializeDataStorage();

            xmlStorageDirectoryTxt.Text = RandNetStatSettings.XMLStorageDirectory;
            excelStorageDirectoryTxt.Text = RandNetStatSettings.ExcelStorageDirectory;
            //databaseTxt.Text = StatisticAnalyzerSettings.ConnectionString;
        }

        private void storageRadio_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton checkedRadio = (RadioButton)sender;
            switch (checkedRadio.Name)
            {
                case "xmlStorageRadio":
                    XmlChecked(true);
                    ExcelChecked(false);
                    SqlChecked(false);
                    break;
                case "excelStorageRadio":
                    XmlChecked(false);
                    ExcelChecked(true);
                    SqlChecked(false);
                    break;
                case "sqlStorageRadio":
                    XmlChecked(false);
                    ExcelChecked(false);
                    SqlChecked(true);
                    break;
                default:
                    break;
            }
        }

        private void xmlStorageBrowseButton_Click(object sender, EventArgs e)
        {
            browserDlg.SelectedPath = RandNetStatSettings.XMLStorageDirectory;
            if (browserDlg.ShowDialog() == DialogResult.OK)
            {
                xmlStorageDirectoryTxt.Text = browserDlg.SelectedPath;
            }
        }

        private void databaseBrowseButton_Click(object sender, EventArgs e)
        {

        }

        private void excelStorageBrowseButton_Click(object sender, EventArgs e)
        {
            browserDlg.SelectedPath = RandNetStatSettings.ExcelStorageDirectory;
            if (browserDlg.ShowDialog() == DialogResult.OK)
            {
                excelStorageDirectoryTxt.Text = browserDlg.SelectedPath;
            }
        }

        private void SaveSettingsButton_Click(object sender, EventArgs e)
        {
            RandNetStatSettings.StorageType = GetDataStorage();

            RandNetStatSettings.XMLStorageDirectory = xmlStorageDirectoryTxt.Text;
            RandNetStatSettings.ExcelStorageDirectory = excelStorageDirectoryTxt.Text;
            //StatisticAnalyzerSettings.ConnectionString = textBoxConnStr.Text;

            RandNetStatSettings.Refresh();
            StatSessionManager.InitializeStorage();
            Close();
        }

        #endregion

        #region Utilities

        private void InitializeDataStorage()
        {
            StorageType stType = RandNetStatSettings.StorageType;
            switch (stType)
            {
                case StorageType.XMLStorage:
                    xmlStorageRadio.Checked = true;
                    break;
                case StorageType.ExcelStorage:
                    excelStorageRadio.Checked = true;
                    break;
                case StorageType.SQLStorage:
                    sqlStorageRadio.Checked = true;
                    break;
                default:
                    break;
            }
        }

        private StorageType GetDataStorage()
        {
            if (xmlStorageRadio.Checked == true)
                return StorageType.XMLStorage;
            else if (excelStorageRadio.Checked == true)
                return StorageType.ExcelStorage;
            else 
                return StorageType.SQLStorage;
        }

        private void XmlChecked(bool c)
        {
            xmlStorageDirectory.Enabled = c;
            xmlStorageDirectoryTxt.Enabled = c;
            xmlStorageBrowseButton.Enabled = c;
        }

        private void ExcelChecked(bool c)
        {
            excelStorageDirectory.Enabled = c;
            excelStorageDirectoryTxt.Enabled = c;
            excelStorageBrowseButton.Enabled = c;
        }

        private void SqlChecked(bool c)
        {
            database.Enabled = c;
            databaseTxt.Enabled = c;
            databaseBrowseButton.Enabled = c;
        }

        #endregion
    }
}
