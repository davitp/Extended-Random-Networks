using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using Core.Settings;

namespace RandNet
{
    public enum FileInputType
    {
        File,
        Folder
    };

    public partial class FileInput : UserControl
    {
        public FileInputType Type { private get; set; }

        public FileInput()
        {
            InitializeComponent();

            openFileDialog.InitialDirectory = RandNetSettings.StaticGenerationDirectory;
            folderBrowserDlg.SelectedPath = RandNetSettings.StaticGenerationDirectory;
        }

        public int MatrixSize
        {
            get
            { 
                return (int)matrixSizeTxt.Value; 
            }
            set
            {
                matrixSizeTxt.Value = value;
            }
        }

        public string MatrixPath
        {
            get
            {
                return locationTxt.Text;
            }
            set
            {
                if (value == "")
                    return;
                locationTxt.Text = value;
                if (Type == FileInputType.File)
                    openFileDialog.InitialDirectory = Path.GetDirectoryName(value);
                else
                    folderBrowserDlg.SelectedPath = value;
            }
        }

        private void browse_Click(object sender, EventArgs e)
        {
            switch (Type)
            {
                case FileInputType.File:

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        locationTxt.Text = openFileDialog.FileName;
                        RandNetSettings.StaticGenerationDirectory = openFileDialog.InitialDirectory;
                        RandNetSettings.Refresh();
                    }
                    break;
                case FileInputType.Folder:
                    if (folderBrowserDlg.ShowDialog() == DialogResult.OK)
                    {
                        locationTxt.Text = folderBrowserDlg.SelectedPath;
                        RandNetSettings.StaticGenerationDirectory = folderBrowserDlg.SelectedPath;
                        RandNetSettings.Refresh();
                    }
                    break;
            }
        }
    }
}
