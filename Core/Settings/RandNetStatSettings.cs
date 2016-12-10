using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

using Core.Enumerations;
using Core.Exceptions;

namespace Core.Settings
{
    /// <summary>
    /// RandNetStat application settings organization and manipulation interface.
    /// </summary>
    public static class RandNetStatSettings
    {
        static private String defaultDirectory =
            Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\xRandNet";

        static private Configuration config;

        static private StorageType storageType = StorageType.XMLStorage;
        static private string xmlStorageDirectory;
        static private string excelStorageDirectory;
        //static private StorageProvider storage;
        //static private string connectionString;

        static RandNetStatSettings()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            try
            {
                storageType = (StorageType)Enum.Parse(typeof(StorageType),
                    config.AppSettings.Settings["StorageType"].Value);
                xmlStorageDirectory = config.AppSettings.Settings["XMLStorageDirectory"].Value;
                excelStorageDirectory = config.AppSettings.Settings["ExcelStorageDirectory"].Value;                
                //connectionString = config.ConnectionStrings.ConnectionStrings[config.AppSettings.Settings["SQLProvider"].Value].ConnectionString;
            }
            catch
            {
                throw new CoreException("The structure of Configuration file is not correct.");
            }
        }

        static public StorageType StorageType
        {
            get
            {
                if (config.AppSettings.Settings["StorageType"].Value != storageType.ToString())
                    storageType = (StorageType)Enum.Parse(typeof(StorageType),
                        config.AppSettings.Settings["StorageType"].Value);
                return storageType;
            }
            set
            {
                storageType = value;
                config.AppSettings.Settings["StorageType"].Value = storageType.ToString();
            }
        }

        static public string XMLStorageDirectory
        {
            get
            {
                return (xmlStorageDirectory == "") ? defaultDirectory + "\\Results" : xmlStorageDirectory;
            }
            set
            {
                if (value.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    xmlStorageDirectory = value;
                }
                else
                {
                    xmlStorageDirectory = value + Path.DirectorySeparatorChar;
                }

                if (Directory.Exists(xmlStorageDirectory) == false)
                {
                    Directory.CreateDirectory(xmlStorageDirectory);
                }

                config.AppSettings.Settings["XMLStorageDirectory"].Value = xmlStorageDirectory;
            }
        }

        static public string ExcelStorageDirectory
        {
            get
            {
                return (excelStorageDirectory == "") ? defaultDirectory + "\\Results" : excelStorageDirectory;
            }
            set
            {
                if (value.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    excelStorageDirectory = value;
                }
                else
                {
                    excelStorageDirectory = value + Path.DirectorySeparatorChar;
                }

                if (Directory.Exists(excelStorageDirectory) == false)
                {
                    Directory.CreateDirectory(excelStorageDirectory);
                }

                config.AppSettings.Settings["ExcelStorageDirectory"].Value = excelStorageDirectory;
            }
        }

        /*static public string ConnectionString 
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
                config.ConnectionStrings.ConnectionStrings[config.AppSettings.Settings["SQLProvider"].Value].ConnectionString
                    = connectionString;
            }
        }*/

        /// <summary>
        /// Refreshes app.config file content.
        /// </summary>
        static public void Refresh()
        {
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            //ConfigurationManager.RefreshSection("connectionStrings");
        }
    }
}
