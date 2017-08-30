using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Globalization;

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
    /// Activation research implementation.
    /// </summary>
    [AvailableModelType(ModelType.ER)]
    [AvailableModelType(ModelType.BA)]
    [AvailableModelType(ModelType.WS)]
    [AvailableModelType(ModelType.HMN)]
    [RequiredResearchParameter(ResearchParameter.InitialActivationProbability)]
    [RequiredResearchParameter(ResearchParameter.ActiveMu)]
    [RequiredResearchParameter(ResearchParameter.Lambda)]
    [RequiredResearchParameter(ResearchParameter.ActivationStepCount)]
    [RequiredResearchParameter(ResearchParameter.TracingStepIncrement)]
    [AvailableGenerationType(GenerationType.Random)]
    [AvailableGenerationType(GenerationType.Static)]
    [AvailableAnalyzeOption(AnalyzeOption.ActivePartA
        | AnalyzeOption.ActivePartB)]
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

        public override ResearchType GetResearchType() => ResearchType.Activation;

        public override int GetProcessStepsCount()
        {
            Debug.Assert(ResearchParameterValues.ContainsKey(ResearchParameter.ActivationStepCount));
            Debug.Assert(ResearchParameterValues.ContainsKey(ResearchParameter.TracingStepIncrement));
            if (processStepCount == -1)
            {
                int ev = (int)Convert.ToUInt32(ResearchParameterValues[ResearchParameter.ActivationStepCount]);
                int tev = (int)Convert.ToUInt32(ResearchParameterValues[ResearchParameter.TracingStepIncrement]);
                int cc = tev != 0 ? ev / tev : 0;
                int t = (TracingPath != "") ? 2 : 1;
                processStepCount = realizationCount * (t + (ev + 1) * GetAnalyzeOptionsCount() + cc) + 1;
            }
            return processStepCount;
        }

        private void RunCompleted(IAsyncResult res)
        {
            realizationCount = currentManager.RealizationsDone;
            result.EnsembleResults.Add(currentManager.Result);
            SaveResearch();
        }

        protected override void ValidateResearchParameters()
        {
            if (!ResearchParameterValues.ContainsKey(ResearchParameter.ActivationStepCount) ||
                !ResearchParameterValues.ContainsKey(ResearchParameter.Lambda) || 
                !ResearchParameterValues.ContainsKey(ResearchParameter.ActiveMu) ||
                !ResearchParameterValues.ContainsKey(ResearchParameter.TracingStepIncrement) ||
                !ResearchParameterValues.ContainsKey(ResearchParameter.InitialActivationProbability))
            {
                Logger.Write("Research - " + ResearchName + ". Invalid research parameters.");
                throw new InvalidResearchParameters();
            }

            UInt32 Time = Convert.ToUInt32(ResearchParameterValues[ResearchParameter.ActivationStepCount]);
            Double Lambda = Double.Parse(ResearchParameterValues[ResearchParameter.Lambda].ToString(), CultureInfo.InvariantCulture);
            Double Mu = Double.Parse(ResearchParameterValues[ResearchParameter.ActiveMu].ToString(), CultureInfo.InvariantCulture);
            Double IP = Double.Parse(ResearchParameterValues[ResearchParameter.InitialActivationProbability].ToString(), CultureInfo.InvariantCulture);

            if (Time <= 0 || Lambda < 0 || Mu < 0|| IP < 0 || IP > 1)
            {
                Logger.Write("Research - " + ResearchName + ". Invalid research parameters.");
                throw new InvalidResearchParameters();
            }
            
            Logger.Write("Research - " + ResearchName + ". Validated research parameters.");
        }

        protected override void FillParameters(AbstractEnsembleManager m)
        {
            m.ResearchParamaterValues = ResearchParameterValues;
            m.GenerationParameterValues = GenerationParameterValues;
        }
    }
}
