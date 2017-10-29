using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Numerics;
using System.Diagnostics;

using Core;
using Core.Enumerations;
using Core.Attributes;
using Core.Result;
using Core.Utility;
using Core.Exceptions;

namespace Storage
{
    /// <summary>
    /// Implementation of XML result storage.
    /// </summary>
    public class XMLResultStorage : AbstractResultStorage
    {
        private XmlWriter writer;
        private XmlReader reader;

        private SortedDictionary<Guid, string> existingFileNames; 

        public XMLResultStorage(string str) : base(str) 
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
            return StorageType.XMLStorage;
        }

        public override void Save(ResearchResult result)
        {
            string fileName = storageStr + result.ResearchName;
            if (File.Exists(fileName + ".xml"))
                fileName += result.ResearchID;

            using (writer = XmlWriter.Create(fileName + ".xml"))
            {
                writer.WriteStartDocument(true);
                writer.WriteStartElement("Research");

                SaveResearchInfo(result.ResearchID, result.ResearchName, 
                    result.ResearchType, result.ModelType, result.RealizationCount, 
                    result.Date, result.Size, result.Edges);
                SaveResearchParameters(result.ResearchParameterValues);
                SaveGenerationParameters(result.GenerationParameterValues);

                writer.WriteStartElement("Ensembles");
                for (int i = 0; i < result.EnsembleResults.Count; ++i)
                {
                    SaveEnsembleResult(result.ResearchName, result.EnsembleResults[i], i);
                }
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }

        public override void Delete(Guid researchID)
        {
            if (existingFileNames != null && existingFileNames.Keys.Contains(researchID))
            {
                File.Delete(existingFileNames[researchID]);
                existingFileNames.Remove(researchID);
            }
            else
            {
                string fileNameToDelete = FileNameByGuid(researchID);
                if(fileNameToDelete != null)
                    File.Delete(fileNameToDelete);
            }   
        }

        public override List<ResearchResult> LoadAllResearchInfo()
        {
            existingFileNames = new SortedDictionary<Guid, string>();
            List<ResearchResult> researchInfos = new List<ResearchResult>();

            ResearchResult DoubleResearchInfo = null;
            foreach (string fileName in Directory.GetFiles(storageStr, "*.xml",
                SearchOption.TopDirectoryOnly))
            {
                DoubleResearchInfo = new ResearchResult();
                using (reader = XmlReader.Create(fileName))
                {
                    try
                    {
                        while (reader.Read() && 
                            (reader.NodeType != XmlNodeType.Element || 
                            reader.Name == "Research")) { }

                        LoadResearchInfo(DoubleResearchInfo);
                        LoadResearchParameters(DoubleResearchInfo);
                        LoadGenerationParameters(DoubleResearchInfo);

                        researchInfos.Add(DoubleResearchInfo);
                        existingFileNames.Add(DoubleResearchInfo.ResearchID, fileName);
                    }
                    catch (SystemException ex)
                    {
                        continue;
                    }
                }
            }

            return researchInfos;
        }

        public override ResearchResult Load(Guid researchID)
        {
            ResearchResult r = null;

            string fileNameToLoad = null;
            if (existingFileNames != null && existingFileNames.Keys.Contains(researchID))
                fileNameToLoad = existingFileNames[researchID];
            else
                fileNameToLoad = FileNameByGuid(researchID);

            if (fileNameToLoad != null)
            {
                r = new ResearchResult();
                using (reader = XmlReader.Create(fileNameToLoad))
                {
                    while (reader.Read() &&
                        (reader.NodeType != XmlNodeType.Element ||
                        reader.Name == "Research")) { }

                    LoadResearchInfo(r);
                    LoadResearchParameters(r);
                    LoadGenerationParameters(r);
                    LoadEnsembleResults(r);
                }
            }

            return r;
        }

        public override ResearchResult Load(string name)
        {
            ResearchResult r = null;

            if (name != null)
            {
                r = new ResearchResult();
                using (reader = XmlReader.Create(name))
                {
                    while (reader.Read() &&
                        (reader.NodeType != XmlNodeType.Element ||
                        reader.Name == "Research")) { }

                    LoadResearchInfo(r);
                    LoadResearchParameters(r);
                    LoadGenerationParameters(r);
                    LoadEnsembleResults(r);
                }
            }

            return r;
        }

        #region Utilities

        #region Save

        private void SaveResearchInfo(Guid researchID,
            string researchName,
            ResearchType rType,
            ModelType mType,
            int realizationCount,
            DateTime date,
            UInt32 size,
            Double edges)
        {
            writer.WriteElementString("ResearchID", researchID.ToString());
            writer.WriteElementString("ResearchName", researchName);
            writer.WriteElementString("ResearchType", rType.ToString());
            writer.WriteElementString("ModelType", mType.ToString());
            writer.WriteElementString("RealizationCount", realizationCount.ToString());
            writer.WriteElementString("Date", date.ToString());
            writer.WriteElementString("Size", size.ToString());
            writer.WriteElementString("Edges", edges.ToString());
        }

        private void SaveResearchParameters(Dictionary<ResearchParameter, object> p)
        {
            writer.WriteStartElement("ResearchParameterValues");
            foreach (ResearchParameter rp in p.Keys)
            {
                if (p[rp] != null)
                {
                    writer.WriteStartElement("ResearchParameter");
                    writer.WriteAttributeString("name", rp.ToString());
                    writer.WriteAttributeString("value", p[rp].ToString());
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
        }

        private void SaveGenerationParameters(Dictionary<GenerationParameter, object> p)
        {
            writer.WriteStartElement("GenerationParameterValues");
            foreach (GenerationParameter gp in p.Keys)
            {
                if (p[gp] == null)
                    continue;
                writer.WriteStartElement("GenerationParameter");
                if (gp == GenerationParameter.AdjacencyMatrix)
                {
                    writer.WriteAttributeString("name", "FileName");
                    writer.WriteAttributeString("value", ((MatrixPath)p[gp]).Path);
                }
                else
                {
                    writer.WriteAttributeString("name", gp.ToString());
                    writer.WriteAttributeString("value", p[gp].ToString());
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        private void SaveEnsembleResult(string researchName, EnsembleResult e, int id)
        {
            writer.WriteStartElement("Ensemble");
            writer.WriteAttributeString("id", id.ToString());

            foreach (AnalyzeOption opt in e.Result.Keys)
            {
                AnalyzeOptionInfo info = ((AnalyzeOptionInfo[])opt.GetType().GetField(opt.ToString()).GetCustomAttributes(typeof(AnalyzeOptionInfo), false))[0];
                OptionType optionType = info.OptionType;

                switch (optionType)
                {
                    case OptionType.Global:
                        writer.WriteElementString(opt.ToString(), e.Result[opt].ToString());
                        break;
                    case OptionType.ValueList:
                    case OptionType.Centrality:
                        writer.WriteStartElement(opt.ToString());
                        SaveValueList(e.Result[opt]);
                        writer.WriteEndElement();
                        break;
                    case OptionType.Distribution:
                        writer.WriteStartElement(opt.ToString());
                        SaveDistribution(info, e.Result[opt]);
                        writer.WriteEndElement();
                        break;
                    case OptionType.Trajectory:
                        writer.WriteStartElement(opt.ToString());
                        SaveTrajectoryAndFile(researchName, info, e.Result[opt]);
                        writer.WriteEndElement();
                        break;
                    default:
                        break;
                }
            }

            writer.WriteEndElement();
        }

        private void SaveValueList(Object value)
        {
            Debug.Assert(value is List<Double>);
            List<Double> l = value as List<Double>;
            foreach (Double d in l)
                writer.WriteElementString("Value", d.ToString());
        }

        private void SaveDistribution(AnalyzeOptionInfo info, Object value)
        {
            Debug.Assert(value is SortedDictionary<Double, Double>);
            SortedDictionary<Double, Double> l = value as SortedDictionary<Double, Double>;
            foreach (KeyValuePair<Double, Double> d in l)
            {
                writer.WriteStartElement("pair");
                writer.WriteAttributeString(info.XAxisName, d.Key.ToString());
                writer.WriteAttributeString(info.YAxisName, d.Value.ToString());
                writer.WriteEndElement();
            }
        }

        private void SaveTrajectoryAndFile(string researchName, AnalyzeOptionInfo info, Object value)
        {
            string fileName = storageStr + researchName + "_" + info.FullName + ".txt";

            writer.WriteElementString("FileName", fileName);
            /*writer.WriteStartElement(info.FullName);
            writer.WriteAttributeString("FileName", fileName);
            writer.WriteEndElement();*/

            Debug.Assert(value is SortedDictionary<Double, Double>);
            SortedDictionary<Double, Double> l = value as SortedDictionary<Double, Double>;
            using (StreamWriter file = new StreamWriter(fileName))
            {
                foreach (KeyValuePair<Double, Double> d in l)
                    file.WriteLine(d.Key.ToString() + " " + d.Value.ToString());
            }
        }

        #endregion

        #region Load

        private void LoadResearchInfo(ResearchResult r)
        {
            if (reader.Name == "ResearchID")
                r.ResearchID = new Guid(reader.ReadElementString());
            if (reader.Name == "ResearchName")
                r.ResearchName = reader.ReadElementString();
            if (reader.Name == "ResearchType")
                r.ResearchType = (ResearchType)Enum.Parse(typeof(ResearchType), reader.ReadElementString());
            if (reader.Name == "ModelType")
                r.ModelType = (ModelType)Enum.Parse(typeof(ModelType), reader.ReadElementString());
            if (reader.Name == "RealizationCount")
                r.RealizationCount = Int32.Parse(reader.ReadElementString());
            if (reader.Name == "Date")
                r.Date = DateTime.Parse(reader.ReadElementString());
            if (reader.Name == "Size")
                r.Size = UInt32.Parse(reader.ReadElementString());
            if (reader.Name == "Edges")
                r.Edges = Double.Parse(reader.ReadElementString());
        }

        private void LoadResearchParameters(ResearchResult r)
        {
            while (reader.Read())
            {
                if (reader.Name == "ResearchParameter")
                {
                    reader.MoveToAttribute("name");
                    ResearchParameter rp = (ResearchParameter)Enum.Parse(typeof(ResearchParameter), reader.ReadContentAsString());

                    reader.MoveToAttribute("value");
                    ResearchParameterInfo rpInfo = (ResearchParameterInfo)(rp.GetType().GetField(rp.ToString()).GetCustomAttributes(typeof(ResearchParameterInfo), false)[0]);
                    if (rpInfo.Type.Equals(typeof(UInt32)))
                        r.ResearchParameterValues.Add(rp, UInt32.Parse(reader.Value));
                    else if (rpInfo.Type.Equals(typeof(Double)))
                        r.ResearchParameterValues.Add(rp, Double.Parse(reader.Value));
                    else if (rpInfo.Type.Equals(typeof(Boolean)))
                        r.ResearchParameterValues.Add(rp, Boolean.Parse(reader.Value));
                    else if (rpInfo.Type.Equals(typeof(ResearchType)))
                        r.ResearchParameterValues.Add(rp, reader.Value.ToString());
                    else if (rpInfo.Type.Equals(typeof(MatrixPath)))
                    {
                        MatrixPath mp = new MatrixPath();
                        mp.Path = reader.Value.ToString();
                        r.ResearchParameterValues.Add(rp, mp);
                    }
                    else
                    {
                        Debug.Assert(false);
                        //throw new InvalidResearchParameters();
                    }
                }
                else if (reader.Name == "GenerationParameterValues")
                    break;
            }
        }

        private void LoadGenerationParameters(ResearchResult r)
        {
            while (reader.Read())
            {
                if (reader.Name == "GenerationParameter")
                {
                    reader.MoveToAttribute("name");
                    string g = reader.ReadContentAsString();
                    GenerationParameter gp;
                    if (g == "FileName")
                        gp = GenerationParameter.AdjacencyMatrix;
                    else
                        gp = (GenerationParameter)Enum.Parse(typeof(GenerationParameter), g);

                    reader.MoveToAttribute("value");
                    GenerationParameterInfo gpInfo = (GenerationParameterInfo)(gp.GetType().GetField(gp.ToString()).GetCustomAttributes(typeof(GenerationParameterInfo), false)[0]);
                    if (gpInfo.Type.Equals(typeof(UInt32)))
                        r.GenerationParameterValues.Add(gp, UInt32.Parse(reader.Value));
                    else if (gpInfo.Type.Equals(typeof(Double)))
                        r.GenerationParameterValues.Add(gp, Double.Parse(reader.Value));
                    else if (gpInfo.Type.Equals(typeof(Boolean)))
                        r.GenerationParameterValues.Add(gp, Boolean.Parse(reader.Value));
                    else if (gpInfo.Type.Equals(typeof(MatrixPath)))
                    {
                        MatrixPath mp = new MatrixPath();
                        mp.Path = reader.Value.ToString();
                        r.GenerationParameterValues.Add(gp, mp);
                    }
                    else
                    {
                        Debug.Assert(false);
                        //throw new InvalidGenerationParameters();
                    }
                }
                if (reader.Name == "Ensembles")
                    break;
            }
        }

        private void LoadEnsembleResults(ResearchResult r)
        {
            while (reader.Read())
            {
                if (reader.Name == "Ensemble" && !reader.IsEmptyElement)
                {
                    EnsembleResult e = new EnsembleResult(r.Size);
                    //e.NetworkSize = r.Size;
                    e.EdgesCount = r.Edges;
                    e.Result = new Dictionary<AnalyzeOption, Object>();

                    reader.Read();
                    while (reader.NodeType != XmlNodeType.EndElement)
                    {
                        AnalyzeOption opt = (AnalyzeOption)Enum.Parse(typeof(AnalyzeOption), reader.Name);
                        AnalyzeOptionInfo optInfo = (AnalyzeOptionInfo)(opt.GetType().GetField(opt.ToString()).GetCustomAttributes(typeof(AnalyzeOptionInfo), false)[0]);
                        switch (optInfo.OptionType)
                        {
                            case OptionType.Global:
                                e.Result.Add(opt, reader.ReadElementContentAsDouble());
                                break;
                            case OptionType.ValueList:
                            case OptionType.Centrality:
                                e.Result.Add(opt, LoadValueList());
                                reader.Read();
                                break;
                            case OptionType.Distribution:
                            case OptionType.Trajectory:
                                e.Result.Add(opt, LoadDistribution());
                                reader.Read();
                                break;
                            default:
                                break;
                        }
                    }

                    r.EnsembleResults.Add(e);
                }
            }
        }

        private Object LoadValueList()
        {
            List<Double> valueList = new List<Double>();
            reader.Read();
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                valueList.Add(Double.Parse(reader.ReadElementString()));
            }
            return valueList;
        }

        private Object LoadDistribution()
        {
            SortedDictionary<Double, Double> d = new SortedDictionary<Double, Double>();
            double first, second;
            while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
            {
                reader.MoveToFirstAttribute();
                first = Double.Parse(reader.Value);
                reader.MoveToNextAttribute();
                second = Double.Parse(reader.Value);
                d.Add(first, second);
            }
            return d;
        }

        #endregion

        private string FileNameByGuid(Guid id)
        {
            foreach (string fileName in Directory.GetFiles(storageStr, "*.xml",
                SearchOption.TopDirectoryOnly))
            {
                using (reader = XmlReader.Create(fileName))
                {
                    try
                    {
                        while (reader.Read() &&
                            (reader.NodeType != XmlNodeType.Element ||
                            reader.Name == "Research")) { }

                        if (reader.Name == "ResearchID")
                        {
                            if (id == Guid.Parse(reader.ReadElementString()))
                            {
                                return fileName;
                            }
                        }
                    }
                    catch (SystemException)
                    {
                        continue;
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
