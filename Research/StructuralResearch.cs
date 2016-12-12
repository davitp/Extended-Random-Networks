using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Threading;

using Microsoft.Practices.EnterpriseLibrary.Logging;

using Core;
using Core.Attributes;
using Core.Enumerations;
using Core.Exceptions;
using Core.Model;
using Core.Utility;
using Core.Settings;
using Core.Events;

namespace Research
{
    /// <summary>
    /// Structural research implementation.
    /// </summary>
    [AvailableModelType(ModelType.ER)]
    [AvailableModelType(ModelType.BA)]
    [AvailableModelType(ModelType.WS)]
    [AvailableGenerationType(GenerationType.Static)]
    //[AvailableGenerationType(GenerationType.Random)]
    [RequiredResearchParameter(ResearchParameter.InputPath)]
    //[RequiredResearchParameter(ResearchParameter.Algorithm)]
    [AvailableAnalyzeOption(
        AnalyzeOption.AvgClusteringCoefficient |
        AnalyzeOption.AvgDegree |
        AnalyzeOption.AvgPathLength |
        AnalyzeOption.ClusteringCoefficientDistribution |
        AnalyzeOption.ClusteringCoefficientPerVertex |
        AnalyzeOption.CompleteComponentDistribution |
        AnalyzeOption.ConnectedComponentDistribution |
        AnalyzeOption.SubtreeDistribution |
        AnalyzeOption.CycleDistribution |
        AnalyzeOption.Cycles3 |
        AnalyzeOption.Cycles4 |
        AnalyzeOption.Cycles5 |
        AnalyzeOption.DegreeDistribution |
        AnalyzeOption.Diameter |
        AnalyzeOption.DistanceDistribution |
        AnalyzeOption.EigenDistanceDistribution |
        AnalyzeOption.EigenValues |
        AnalyzeOption.TriangleByVertexDistribution)]// |
        //AnalyzeOption.DegreeCentrality |
        //AnalyzeOption.BetweennessCentrality |
        //AnalyzeOption.ClosenessCentrality)]
    public class StructuralResearch : AbstractResearch
    {
        private List<AbstractResearch> subResearches = null;
        private int currentResearchIndex = -1;

        public override void StartResearch()
        {
            ValidateResearchParameters();

            StatusInfo = new ResearchStatusInfo(ResearchStatus.Running, 0);
            Logger.Write("Research ID - " + ResearchID.ToString() +
                ". Research - " + ResearchName + ". STARTED STRUCTURAL RESEARCH.");

            MatrixPath mp = ((MatrixPath)ResearchParameterValues[ResearchParameter.InputPath]);
            List<MatrixPath> matrixes = new List<MatrixPath>();

            Debug.Assert((File.GetAttributes(mp.Path) & FileAttributes.Directory) == FileAttributes.Directory);
            Debug.Assert(Directory.GetFiles(mp.Path, "*.txt").Count() == 1);
            MatrixPath m = new MatrixPath();
            m.Path = Directory.GetFiles(mp.Path, "*.txt")[0];
            m.Size = mp.Size;
            matrixes.Add(m);
            MatrixInfoToRead mr = FileManager.Read(m.Path, m.Size);

            string storageString = Storage.StorageString;
            if (Storage.GetStorageType() != StorageType.SQLStorage)
            {
                storageString += ResearchName;
                if (!Directory.Exists(storageString))
                    Directory.CreateDirectory(storageString);
            }

            foreach (string fn in Directory.GetFiles(mp.Path, "*.sm"))
            {
                int[] s;
                FileManager.ReadSubgraphMatrix(fn, out s);
                MatrixInfoToWrite tmp = new MatrixInfoToWrite();
                tmp.Matrix = CreateMatrixForSubgraph(mr.Matrix, s);
                
                // Create temporary .txt files for each matrix
                string tmpFileName = storageString + "\\" + Path.GetFileNameWithoutExtension(fn);
                FileManager.Write(tmp, tmpFileName);

                MatrixPath sm = new MatrixPath();
                sm.Path = tmpFileName + ".txt";
                matrixes.Add(sm);
            }

            ResearchType rt = ResearchType.Basic;   // subresearch type is not supported and is always Basic

            subResearches = new List<AbstractResearch>();
            foreach (MatrixPath p in matrixes)
            {
                AbstractResearch r = AbstractResearch.CreateResearchByType(rt);
                r.ResearchName = ResearchName + "_" + Path.GetFileNameWithoutExtension(p.Path);
                r.GenerationType = GenerationType.Static;
                r.ModelType = ModelType;

                Debug.Assert(r.GenerationParameterValues.ContainsKey(GenerationParameter.AdjacencyMatrix));
                r.GenerationParameterValues[GenerationParameter.AdjacencyMatrix] = p;

                r.AnalyzeOption = AnalyzeOption;

                r.Storage = AbstractResultStorage.CreateStorage(Storage.GetStorageType(), storageString);
                r.OnUpdateResearchStatus += method;

                subResearches.Add(r);
            }

            if (subResearches.Count() != 0)
            {
                ++currentResearchIndex;
                subResearches[currentResearchIndex].StartResearch();
            }
        }

        private void method(object sender, ResearchEventArgs e)
        {
            ResearchStatus rs = GetSubresearchStatus(e.ResearchID);
            if (rs == ResearchStatus.Completed)
            {
                Interlocked.Increment(ref currentResearchIndex);
                if (currentResearchIndex >= subResearches.Count())
                    StatusInfo = new ResearchStatusInfo(ResearchStatus.Completed, (uint)currentResearchIndex);
                else
                {
                    StatusInfo = new ResearchStatusInfo(ResearchStatus.Running, StatusInfo.CompletedStepsCount + 1);
                    subResearches[currentResearchIndex].StartResearch();
                }
            }
        }

        private ResearchStatus GetSubresearchStatus(Guid id)
        {
            foreach (AbstractResearch r in subResearches)
            {
                if (r.ResearchID == id)
                    return r.StatusInfo.Status;
            }
            Debug.Assert(false);
            return ResearchStatus.NotStarted;
        }

        public override void StopResearch()
        {
            if (subResearches != null)
            {
                foreach (AbstractResearch r in subResearches)
                    r.StopResearch();

                StatusInfo = new ResearchStatusInfo(ResearchStatus.Stopped, StatusInfo.CompletedStepsCount);

                Logger.Write("Research ID - " + ResearchID.ToString() +
                    ". Research - " + ResearchName + ". STOPPED STRUCTURAL RESEARCH.");
            }
        }

        public override ResearchType GetResearchType()
        {
            return ResearchType.Structural;
        }

        public override int GetProcessStepsCount()
        {
            Debug.Assert(realizationCount == 1);
            if (processStepCount == -1)
            {
                MatrixPath mp = ((MatrixPath)ResearchParameterValues[ResearchParameter.InputPath]);
                processStepCount = Directory.GetFiles(mp.Path, "*.txt").Count() +
                    Directory.GetFiles(mp.Path, "*.sm").Count();
            }
            return processStepCount;
        }

        protected override void ValidateResearchParameters()
        {
            if (!ResearchParameterValues.ContainsKey(ResearchParameter.InputPath) ||
                ResearchParameterValues[ResearchParameter.InputPath] == null ||
                ((MatrixPath)ResearchParameterValues[ResearchParameter.InputPath]).Path == "")
            {
                Logger.Write("Research - " + ResearchName + ". Invalid research parameters.");
                throw new InvalidResearchParameters();
            }

            MatrixPath mp = ((MatrixPath)ResearchParameterValues[ResearchParameter.InputPath]);
            if ((File.GetAttributes(mp.Path) & FileAttributes.Directory) != FileAttributes.Directory)
            {
                Logger.Write("Research - " + ResearchName + ". Invalid research parameters." +
                    " Directory should be specified.");
                throw new InvalidResearchParameters();
            }

            if (Directory.GetFiles(mp.Path, "*.txt").Count() != 1)
            {
                Logger.Write("Research - " + ResearchName + ". Invalid research parameters." +
                    " Directory should contain only 1 .txt file.");
                throw new InvalidResearchParameters();
            }

            Logger.Write("Research - " + ResearchName + ". Validated research parameters.");
        }

        protected override void FillParameters(AbstractEnsembleManager m) { }

        private bool[,] CreateMatrixForSubgraph(ArrayList inMatrix, int[] subgraph)
        {
            bool[,] matrix = new bool[subgraph.Count(), subgraph.Count()];
            for (int j = 0; j < subgraph.Count(); ++j)
                for (int k = 0; k < subgraph.Count(); ++k)
                    matrix[j, k] = (bool)((ArrayList)inMatrix[subgraph[j]])[subgraph[k]];

            return matrix;
        }
    }
}
