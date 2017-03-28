using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.Attributes;
using Core.Utility;

namespace Core.Enumerations
{
    /// <summary>
    /// Enumeration of research parameters that should be used for Research run.
    /// Uses Attribute ResearchParameterInfo for storing metadata about every research parameter.
    /// This metadata is used mainly for validating and getting user-friendly information.
    /// </summary>
    public enum ResearchParameter
    {
        [ResearchParameterInfo("Count of evolution steps",
            "Count of evolution steps",
            typeof(UInt32), "1")]
        EvolutionStepCount,

        [ResearchParameterInfo("Nu",
            "Nu",
            typeof(Double), "0.1")]
        Nu,

        [ResearchParameterInfo("Tracing step increment",
            "Tracing step increment",
            typeof(UInt32), "0")]
        TracingStepIncrement,

        [ResearchParameterInfo("Distribution is permanent",
            "Distribution is permanent",
            typeof(Boolean), "false")]
        PermanentDistribution,

        [ResearchParameterInfo("Maximum probability",
            "Maximal value of probability",
            typeof(Double), "1.0")]
        ProbabilityMax,

        [ResearchParameterInfo("Delta of probability",
            "Delta of probability change",
            typeof(Double), "0.01")]
        ProbabilityDelta,

        [ResearchParameterInfo("Mu",
            "Mu",
            typeof(Double), "0.1")]
        Mu,

        [ResearchParameterInfo("Lambda",
            "Lambda",
            typeof(Double), "0.1")]
        Lambda,

        [ResearchParameterInfo("Count of activation steps",
            "Count of activation steps",
            typeof(UInt32), "10")]
        ActivationStepCount,

        [ResearchParameterInfo("Research type",
            "Research type for each item",
            typeof(ResearchType), "BasicResearch")]
        ResearchItemType,

        [ResearchParameterInfo("Input path",
            "Path for input data",
            typeof(MatrixPath), null)]
        InputPath
    }
}
