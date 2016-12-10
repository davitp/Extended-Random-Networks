using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Practices.EnterpriseLibrary.Logging;

using Core;
using Core.Enumerations;
using Core.Attributes;
using Core.Events;
using Core.Exceptions;

namespace Research
{
    /// <summary>
    /// Evolution research implementation.
    /// </summary>
    [AvailableModelType(ModelType.ER)]
    [AvailableGenerationType(GenerationType.Random)]
    [AvailableGenerationType(GenerationType.Static)]
    [RequiredResearchParameter(ResearchParameter.EvolutionStepCount)]
    [RequiredResearchParameter(ResearchParameter.Nu)]
    [RequiredResearchParameter(ResearchParameter.PermanentDistribution)]
    [RequiredResearchParameter(ResearchParameter.TracingStepIncrement)]
    [AvailableAnalyzeOption(AnalyzeOption.Cycles3Trajectory)]
    public class EvolutionResearch : AbstractResearch
    {
        /// <summary>
        /// Creates single EnsembleManagers, runs in background thread.
        /// </summary>
        public override void StartResearch()
        {
            ValidateResearchParameters();

            CreateEnsembleManager();
            StatusInfo = new ResearchStatusInfo(ResearchStatus.Running, 0);
            Logger.Write("Research ID - " + ResearchID.ToString() +
                ". Research - " + ResearchName + ". STARTED EVOLUTION RESEARCH.");

            ManagerRunner r = new ManagerRunner(currentManager.Run);
            r.BeginInvoke(new AsyncCallback(RunCompleted), null);
        }

        public override void StopResearch()
        {
            currentManager.Cancel();
            StatusInfo = new ResearchStatusInfo(ResearchStatus.Stopped, StatusInfo.CompletedStepsCount);
            Logger.Write("Research ID - " + ResearchID.ToString() +
                ". Research - " + ResearchName + ". STOPPED EVOLUTION RESEARCH.");
        }

        public override ResearchType GetResearchType()
        {
            return ResearchType.Evolution;
        }

        public override int GetProcessStepsCount()
        {
            Debug.Assert(ResearchParameterValues.ContainsKey(ResearchParameter.EvolutionStepCount));
            Debug.Assert(ResearchParameterValues.ContainsKey(ResearchParameter.TracingStepIncrement));
            if (processStepCount == -1)
            {
                int ev = (int)Convert.ToUInt32(ResearchParameterValues[ResearchParameter.EvolutionStepCount]);
                int tev = (int)Convert.ToUInt32(ResearchParameterValues[ResearchParameter.TracingStepIncrement]);
                int cc = tev != 0 ? ev / tev : 0;
                int t = (TracingPath != "") ? 2 : 1;
                processStepCount = realizationCount * (t + (ev + 1) * GetAnalyzeOptionsCount() + cc) + 1;
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
            m.ResearchParamaterValues = ResearchParameterValues;
            m.GenerationParameterValues = GenerationParameterValues;
        }
    }
}
