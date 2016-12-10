using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics;

using Microsoft.Practices.EnterpriseLibrary.Logging;

using Core;
using Core.Attributes;
using Core.Enumerations;
using Core.Events;
using Core.Exceptions;

namespace Research
{
    /// <summary>
    /// Percolation research implementation.
    /// </summary>
    [AvailableModelType(ModelType.ER)]
    [AvailableModelType(ModelType.RegularHierarchic)]
    [AvailableGenerationType(GenerationType.Random)]
    [AvailableGenerationType(GenerationType.Static)]
    [RequiredResearchParameter(ResearchParameter.ProbabilityMax)]
    [RequiredResearchParameter(ResearchParameter.ProbabilityDelta)]
    [AvailableAnalyzeOption(
        AnalyzeOption.AvgClusteringCoefficient |
        AnalyzeOption.AvgDegree |
        AnalyzeOption.AvgPathLength |
        AnalyzeOption.ClusteringCoefficientDistribution |
        AnalyzeOption.ClusteringCoefficientPerVertex |
        AnalyzeOption.CompleteComponentDistribution |
        AnalyzeOption.ConnectedComponentDistribution |
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
    public class PercolationResearch : AbstractResearch
    {
        private bool isCanceled = false;

        private GenerationParameter probabilityParameter;
        private Double minProbability;
        private Double currentProbability;
        private Double maxProbability;
        private Double delta;

        /// <summary>
        /// Creates multiple EnsembleManagers, running sequentially.
        /// </summary>
        public override void StartResearch()
        {
            ValidateResearchParameters();

            Debug.Assert(GenerationParameterValues.ContainsKey(GenerationParameter.Probability) ||
                GenerationParameterValues.ContainsKey(GenerationParameter.Mu));
            probabilityParameter = GenerationParameterValues.ContainsKey(GenerationParameter.Probability) ?
                GenerationParameter.Probability : GenerationParameter.Mu;

            minProbability = Convert.ToDouble(GenerationParameterValues[probabilityParameter]);
            currentProbability = minProbability;
            maxProbability = Convert.ToDouble(ResearchParameterValues[ResearchParameter.ProbabilityMax]);
            delta = Convert.ToDouble(ResearchParameterValues[ResearchParameter.ProbabilityDelta]);

            StatusInfo = new ResearchStatusInfo(ResearchStatus.Running, 0);
            Logger.Write("Research ID - " + ResearchID.ToString() +
                ". Research - " + ResearchName + ". STARTED PERCOLATION RESEARCH.");

            StartCurrentEnsemble();
        }

        public override void StopResearch()
        {
            isCanceled = true;
            currentManager.Cancel();
            StatusInfo = new ResearchStatusInfo(ResearchStatus.Stopped, StatusInfo.CompletedStepsCount);
            Logger.Write("Research ID - " + ResearchID.ToString() +
                ". Research - " + ResearchName + ". STOPPED PERCOLATION RESEARCH.");
        }

        public override ResearchType GetResearchType()
        {
            return ResearchType.Percolation;
        }

        public override int GetProcessStepsCount()
        {
            /*if (processStepCount == -1)
            {
                // TODO
            }
            return processStepCount;*/
            return 10000;
        }

        protected override void ValidateResearchParameters()
        {
            if (!ResearchParameterValues.ContainsKey(ResearchParameter.ProbabilityDelta) ||
                !ResearchParameterValues.ContainsKey(ResearchParameter.ProbabilityMax))
            {
                Logger.Write("Research - " + ResearchName + ". Invalid research parameters.");
                throw new InvalidResearchParameters();
            }

            Logger.Write("Research - " + ResearchName + ". Validated research parameters.");
        }

        private void RunCompleted(IAsyncResult res)
        {
            if (isCanceled)
                ResearchParameterValues[ResearchParameter.ProbabilityMax] = currentProbability;
            else
                result.EnsembleResults.Add(currentManager.Result);

            currentProbability += delta;
            StartCurrentEnsemble();
        }

        private void StartCurrentEnsemble()
        {
            if (currentProbability <= maxProbability && !isCanceled)
            {
                base.CreateEnsembleManager();
                ManagerRunner r = new ManagerRunner(currentManager.Run);
                r.BeginInvoke(new AsyncCallback(RunCompleted), null);
            }
            else
            {
                base.SaveResearch();
            }
        }

        protected override void FillParameters(AbstractEnsembleManager m)
        {
            Dictionary<GenerationParameter, object> g = new Dictionary<GenerationParameter, object>();
            foreach (GenerationParameter p in base.GenerationParameterValues.Keys)
            {
                if (p == probabilityParameter)
                    g.Add(p, currentProbability);
                else
                    g.Add(p, base.GenerationParameterValues[p]);
            }

            m.GenerationParameterValues = g;
        }
    }
}
