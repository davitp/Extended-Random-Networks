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
    [RequiredResearchParameter(ResearchParameter.Mu)]
    [RequiredResearchParameter(ResearchParameter.Lambda)]
    [RequiredResearchParameter(ResearchParameter.ActivationStepCount)]
    //[AvailableGenerationType(GenerationType.Random)]  // TODO think about
    [AvailableGenerationType(GenerationType.Static)]
    [AvailableAnalyzeOption(AnalyzeOption.ActivePart)]
    public class ActivationResearch : AbstractResearch
    {
        /// <summary>
        /// 
        /// </summary>
        public override void StartResearch()
        {
            ValidateResearchParameters();

            // TODO

            StatusInfo = new ResearchStatusInfo(ResearchStatus.Running, 0);
            Logger.Write("Research ID - " + ResearchID.ToString() +
                ". Research - " + ResearchName + ". STARTED ACTIVATION RESEARCH.");

            // TODO
        }

        public override void StopResearch()
        {
            // TODO
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
            return 0;
        }

        protected override void ValidateResearchParameters()
        {
            // TODO
            Logger.Write("Research - " + ResearchName + ". Validated research parameters.");
        }

        protected override void FillParameters(AbstractEnsembleManager m)
        {
            m.GenerationParameterValues = GenerationParameterValues;
        }
    }
}
