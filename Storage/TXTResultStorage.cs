using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

using Core;
using Core.Enumerations;
using Core.Attributes;
using Core.Result;
using Core.Utility;

namespace Storage
{
    /// <summary>
    /// Implementation of txt result storage. 
    /// <remark>Making folder with .txt files for global properties and  for each option saparetely.</remark>
    /// </summary>
    class TXTResultStorage : AbstractResultStorage
    {
        public TXTResultStorage(string str)
            : base(str)
        {
            if (!storageStr.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                storageStr += Path.DirectorySeparatorChar;
            }
            if (!Directory.Exists(storageStr))
            {
                Directory.CreateDirectory(storageStr);
            }
        }

        public override StorageType GetStorageType()
        {
            return StorageType.TXTStorage;
        }

        public override void Save(ResearchResult result)
        {
            string dirName = storageStr + result.ResearchName;
            if (Directory.Exists(dirName))
                dirName += result.ResearchID;

            Directory.CreateDirectory(dirName);

            SaveGeneralInfo(dirName, result.ResearchID, result.ResearchName,
                    result.ResearchType, result.ModelType, result.RealizationCount,
                    result.Size, result.Edges, result.ResearchParameterValues,
                    result.GenerationParameterValues);

            for (int i = 0; i < result.EnsembleResults.Count; ++i)
            {
                SaveEnsembleResult(dirName, result.ResearchName, result.EnsembleResults[i], i);
            }
        }

        public override void Delete(Guid researchID)
        {
            // TODO implementation
        }

        public override List<ResearchResult> LoadAllResearchInfo()
        {
            return null;
            // TODO implementation
        }

        public override ResearchResult Load(Guid researchID)
        {
            // TODO implementation
            return null;
        }

        public override ResearchResult Load(string name)
        {
            throw new NotImplementedException();
        }

        #region Utilities

        #region Save

        private void SaveGeneralInfo(string dirName,
            Guid researchID,
            string researchName,
            ResearchType rType,
            ModelType mType,
            int realizationCount,
            UInt32 size,
            Double edges,
            Dictionary<ResearchParameter, object> rp,
            Dictionary<GenerationParameter, object> gp)
        {
            string fileName = dirName + Path.DirectorySeparatorChar + "general";
            using (StreamWriter w = new StreamWriter(fileName + ".txt"))
            {
                w.WriteLine("Research Info");
                w.WriteLine("ResearchID - " + researchID);
                w.WriteLine("ResearchName - " + researchName);
                w.WriteLine("ResearchType - " + rType);
                w.WriteLine("ModelType - " + mType);
                w.WriteLine("RealizationCount - " + realizationCount);
                w.WriteLine("Date - " + DateTime.Now);
                w.WriteLine("Size - " + size);
                w.WriteLine("Edges - " + edges);
            }

            SaveResearchParameters(fileName, rp);
            SaveGenerationParameters(fileName, gp);
        }

        private void SaveResearchParameters(string fileName, Dictionary<ResearchParameter, object> p)
        {
            if (p.Count != 0)
            {
                using (StreamWriter w = new StreamWriter(fileName + ".txt", true))
                {
                    w.WriteLine();
                    w.WriteLine("Research Parameters");
                    foreach (ResearchParameter rp in p.Keys)
                    {
                        if (p[rp] != null)
                        {
                            w.WriteLine(rp.ToString() + " " + p[rp].ToString());
                        }
                    }
                }
            }
        }

        private void SaveGenerationParameters(string fileName, Dictionary<GenerationParameter, object> p)
        {
            using (StreamWriter w = new StreamWriter(fileName + ".txt", true))
            {
                w.WriteLine();
                w.WriteLine("Generation Parameters");
                foreach (GenerationParameter gp in p.Keys)
                {
                    if (p[gp] == null)
                        continue;
                    if (gp == GenerationParameter.AdjacencyMatrix)
                    {
                        w.WriteLine("FileName - " + ((MatrixPath)p[gp]).Path);
                    }
                    else
                    {
                        w.WriteLine(gp.ToString() + " " + p[gp].ToString());
                    }
                }
            }
        }

        private void SaveEnsembleResult(string dirName, string researchName, EnsembleResult e, int id)
        {
            foreach (AnalyzeOption opt in e.Result.Keys)
            {
                AnalyzeOptionInfo info = ((AnalyzeOptionInfo[])opt.GetType().GetField(opt.ToString()).GetCustomAttributes(typeof(AnalyzeOptionInfo), false))[0];
                OptionType optionType = info.OptionType;

                switch (optionType)
                {
                    case OptionType.Global:
                        SaveToGlobalFile(dirName, id, info, e.Result[opt]);
                        break;
                    case OptionType.ValueList:
                    case OptionType.Centrality:
                        SaveValueListFile(dirName, id, info, e.Result[opt]);
                        break;
                    case OptionType.Distribution:
                        SaveDistributionFile(dirName, id, info, e.Result[opt]);
                        break;
                    case OptionType.Trajectory:
                        SaveTrajectoryFile(dirName, researchName, id, info, e.Result[opt]);
                        break;
                    default:
                        break;
                }
            }
        }

        private void SaveToGlobalFile(string dirName, int id, AnalyzeOptionInfo info, Object value)
        {
            string fileName = dirName + Path.DirectorySeparatorChar + id.ToString() + "_globals";
            using (StreamWriter w = new StreamWriter(fileName + ".txt", true))
            {
                w.WriteLine(info.FullName + " " + value.ToString());
            }
        }

        private void SaveValueListFile(string dirName, int id, AnalyzeOptionInfo info, Object value)
        {
            string fileName = dirName + Path.DirectorySeparatorChar + id.ToString() + "_" + info.FullName;
            using (StreamWriter w = new StreamWriter(fileName + ".txt"))
            {
                Debug.Assert(value is List<Double>);
                List<Double> l = value as List<Double>;
                foreach (Double d in l)
                {
                    w.WriteLine(d);
                }
            }
        }

        private void SaveCentralityFile(string dirName, int id, AnalyzeOptionInfo info, Object value)
        {
            throw new NotImplementedException();
        }

        private void SaveDistributionFile(string dirName, int id, AnalyzeOptionInfo info, Object value)
        {
            string fileName = dirName + Path.DirectorySeparatorChar + id.ToString() + "_" + info.FullName;
            using (StreamWriter w = new StreamWriter(fileName + ".txt"))
            {
                Debug.Assert(value is SortedDictionary<Double, Double>);
                SortedDictionary<Double, Double> l = value as SortedDictionary<Double, Double>;
                foreach (KeyValuePair<Double, Double> d in l)
                {
                    w.WriteLine(d.Key + " " + d.Value);
                }
            }
        }

        private void SaveTrajectoryFile(string dirName, string researchName, int id, AnalyzeOptionInfo info, Object value)
        {
            string fileName = dirName + Path.DirectorySeparatorChar + id.ToString() + "_" + info.FullName;
            using (StreamWriter w = new StreamWriter(fileName + ".txt"))
            {
                Debug.Assert(value is SortedDictionary<Double, Double>);
                SortedDictionary<Double, Double> l = value as SortedDictionary<Double, Double>;
                foreach (KeyValuePair<Double, Double> d in l)
                    w.WriteLine(d.Key + " " + d.Value);
            }                
        }

        #endregion

        #region Load
        #endregion
        
        #endregion
    }
}