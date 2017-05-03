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
using Core.Exceptions;

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
    [RequiredResearchParameter(ResearchParameter.ActiveMu)]
    [RequiredResearchParameter(ResearchParameter.Lambda)]
    [RequiredResearchParameter(ResearchParameter.ActivationStepCount)]
    //[AvailableGenerationType(GenerationType.Random)]  // TODO think about
    [AvailableGenerationType(GenerationType.Static)]
    [AvailableAnalyzeOption(AnalyzeOption.ActivePart)]
    //[AvailableAnalyzeOption(AnalyzeOption.ActivePart1)]
    public class ActivationResearch : AbstractResearch
    {
        /// <summary>
        /// 
        /// </summary>
        public override void StartResearch()
        {
            ValidateResearchParameters();

            CreateEnsembleManager();
            StatusInfo = new ResearchStatusInfo(ResearchStatus.Running, 0);
            Logger.Write("Research ID - " + ResearchID.ToString() +
                ". Research - " + ResearchName + ". STARTED ACTIVATION RESEARCH.");

            ManagerRunner r = new ManagerRunner(currentManager.Run);
            r.BeginInvoke(new AsyncCallback(RunCompleted), null);
        }

        public override void StopResearch()
        {
            currentManager.Cancel();
            StatusInfo = new ResearchStatusInfo(ResearchStatus.Stopped, StatusInfo.CompletedStepsCount);
            Logger.Write("Research ID - " + ResearchID.ToString() +
                ". Research - " + ResearchName + ". STOPPED ACTIVATION RESEARCH.");
        }

        public override ResearchType GetResearchType()
        {
            return ResearchType.Activation;
        }

        public override int GetProcessStepsCount()
        {
            // TODO
            return 1;
        }

        protected override void ValidateResearchParameters()
        {
            if (!ResearchParameterValues.ContainsKey(ResearchParameter.Lambda) ||
                !ResearchParameterValues.ContainsKey(ResearchParameter.ActiveMu) ||
                !ResearchParameterValues.ContainsKey(ResearchParameter.ActivationStepCount)
                )
            {
                Logger.Write("Research - " + ResearchName + ". Invalid research parameters.");
                throw new InvalidResearchParameters();
            }

            Double l = ((Double)ResearchParameterValues[ResearchParameter.Lambda]);
            Double m = ((Double)ResearchParameterValues[ResearchParameter.ActiveMu]);
            UInt32 t = ((UInt32)ResearchParameterValues[ResearchParameter.ActivationStepCount]);

            if (l < 0 || l > 1 || m < 0 || m > 1 || t == 0)
            {
                Logger.Write("Research - " + ResearchName + ". Invalid research parameters.");
                throw new InvalidResearchParameters();
            }

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
