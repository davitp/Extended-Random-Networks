using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Logging;

using Core;
using Core.Attributes;
using Core.Enumerations;
using Core.Model;
using Core.Events;

namespace Research
{
    /// <summary>
    /// Basic research implementation.
    /// </summary>
    [AvailableModelType(ModelType.ER)]
    [AvailableModelType(ModelType.BA)]
    [AvailableModelType(ModelType.WS)]
    [AvailableModelType(ModelType.RegularHierarchic)]
    [AvailableModelType(ModelType.NonRegularHierarchic)]
    [AvailableModelType(ModelType.IM)]
    [AvailableGenerationType(GenerationType.Random)]
    [AvailableGenerationType(GenerationType.Static)]
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
        AnalyzeOption.TriangleByVertexDistribution)] //|
        //AnalyzeOption.DegreeCentrality |
        //AnalyzeOption.BetweennessCentrality |
        //AnalyzeOption.ClosenessCentrality)]
    public class BasicResearch : AbstractResearch
    {
        /// <summary>
        /// Creates single EnsembleManager, runs in background thread.
        /// </summary>
        public override void StartResearch()
        {
            ValidateResearchParameters();

            CreateEnsembleManager();
            StatusInfo = new ResearchStatusInfo(ResearchStatus.Running, 0);
            Logger.Write("Research ID - " + ResearchID.ToString() +
                ". Research - " + ResearchName + ". STARTED BASIC RESEARCH.");

            ManagerRunner r = new ManagerRunner(currentManager.Run);
            r.BeginInvoke(new AsyncCallback(RunCompleted), null);
        }

        public override void StopResearch()
        {
            currentManager.Cancel();
            StatusInfo = new ResearchStatusInfo(ResearchStatus.Stopped, StatusInfo.CompletedStepsCount);

            Logger.Write("Research ID - " + ResearchID.ToString() +
                ". Research - " + ResearchName + ". STOPPED BASIC RESEARCH.");
        }

        public override ResearchType GetResearchType()
        {
            return ResearchType.Basic;
        }

        public override int GetProcessStepsCount()
        {
            if (processStepCount == -1)
            {
                int t = (TracingPath != "") ? 2 : 1;
                processStepCount = realizationCount * (t + GetAnalyzeOptionsCount()) + 1;
            }
            return processStepCount;
        }

        protected override void ValidateResearchParameters() 
        {
            Logger.Write("Research - " + ResearchName + ". Validated research parameters.");
        }

        private void RunCompleted(IAsyncResult res)
        {
            realizationCount = currentManager.RealizationsDone;
            result.EnsembleResults.Add(currentManager.Result);
            SaveResearch();
        }

        protected override void FillParameters(AbstractEnsembleManager m)
        {
            m.GenerationParameterValues = GenerationParameterValues;
        }        
    }
}
