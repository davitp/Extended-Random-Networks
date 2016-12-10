using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Core;
using Core.Enumerations;
using Core.Attributes;
using Core.Result;
using Core.Settings;
using Storage;

namespace RandNet
{
    public partial class DataConvertionsWindow : Form
    {
        public DataConvertionsWindow()
        {
            InitializeComponent();
        }

        private void DataConvertionsWindow_Load(object sender, EventArgs e)
        {
            sourceStorageTypeCmb.Items.Clear();
            targetStorageCmb.Items.Clear();

            string[] storageTypeNames = Enum.GetNames(typeof(StorageType));
            sourceStorageTypeCmb.Items.AddRange(storageTypeNames);
            targetStorageCmb.Items.AddRange(storageTypeNames);

            if (sourceStorageTypeCmb.Items.Count != 0)
                sourceStorageTypeCmb.SelectedIndex = 0;
            if (targetStorageCmb.Items.Count != 0)
                targetStorageCmb.SelectedIndex = 0;

            sourceResultTxt.Text = targetResultTxt.Text = RandNetSettings.DataConvertionToolDirectory;
            sourceBrowserDlg.SelectedPath = targetBrowserDlg.SelectedPath = RandNetSettings.DataConvertionToolDirectory;
        }

        private void sourceBrowse_Click(object sender, EventArgs e)
        {
            if (sourceBrowserDlg.ShowDialog() == DialogResult.OK)
            {
                sourceResultTxt.Text = sourceBrowserDlg.SelectedPath;
                targetBrowserDlg.SelectedPath = sourceBrowserDlg.SelectedPath;
                targetResultTxt.Text = sourceBrowserDlg.SelectedPath;
                RandNetSettings.DataConvertionToolDirectory = sourceBrowserDlg.SelectedPath;
            }
        }

        private void targetBrowse_Click(object sender, EventArgs e)
        {
            if (targetBrowserDlg.ShowDialog() == DialogResult.OK)
                targetResultTxt.Text = targetBrowserDlg.SelectedPath;
        }

        private void convert_Click(object sender, EventArgs e)
        {
            convert.Enabled = false;

            StorageType sourceType = (StorageType)Enum.Parse(typeof(StorageType), sourceStorageTypeCmb.Text);
            string sourceStr = sourceResultTxt.Text;
            StorageType targetType = (StorageType)Enum.Parse(typeof(StorageType), targetStorageCmb.Text);
            string targetStr = targetResultTxt.Text;

            Type[] patametersType = { typeof(String) };
            object[] sinvokeParameters = { sourceStr };
            StorageTypeInfo[] sinfo = (StorageTypeInfo[])sourceType.GetType().GetField(sourceType.ToString()).GetCustomAttributes(typeof(StorageTypeInfo), false);
            Type st = Type.GetType(sinfo[0].Implementation, true);
            AbstractResultStorage sourceStorage = (AbstractResultStorage)st.GetConstructor(patametersType).Invoke(sinvokeParameters);

            object[] tinvokeParameters = { targetStr };
            StorageTypeInfo[] tinfo = (StorageTypeInfo[])targetType.GetType().GetField(targetType.ToString()).GetCustomAttributes(typeof(StorageTypeInfo), false);
            Type tt = Type.GetType(tinfo[0].Implementation, true);
            AbstractResultStorage targetStorage = (AbstractResultStorage)tt.GetConstructor(patametersType).Invoke(tinvokeParameters);

            List<ResearchResult> allResearchInfo = sourceStorage.LoadAllResearchInfo();
            ResearchResult temp;
            foreach (ResearchResult r in allResearchInfo)
            {
                temp = sourceStorage.Load(r.ResearchID);
                targetStorage.Save(temp);
            }

            MessageBox.Show("Successfully converted from " + sourceStorageTypeCmb.Text + 
                " to " + targetStorageCmb.Text + ".");
            convert.Enabled = true;
        }
    }
}
