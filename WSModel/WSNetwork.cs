﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core;
using Core.Attributes;
using Core.Enumerations;
using NetworkModel;

namespace WSModel
{
    /// <summary>
    /// Implementation of random network of Watts-Strogatz's model.
    /// </summary>
    [RequiredGenerationParameter(GenerationParameter.AdjacencyMatrix)]
    [RequiredGenerationParameter(GenerationParameter.Vertices)]
    [RequiredGenerationParameter(GenerationParameter.Edges)]
    [RequiredGenerationParameter(GenerationParameter.Probability)]
    [RequiredGenerationParameter(GenerationParameter.StepCount)]
    [AvailableAnalyzeOption(AnalyzeOption.AvgClusteringCoefficient
        | AnalyzeOption.AvgDegree
        | AnalyzeOption.AvgPathLength
        | AnalyzeOption.ClusteringCoefficientDistribution
        | AnalyzeOption.ClusteringCoefficientPerVertex
        | AnalyzeOption.ConnectedComponentDistribution
        | AnalyzeOption.Cycles3
        | AnalyzeOption.Cycles4
        | AnalyzeOption.DegreeDistribution
        | AnalyzeOption.Diameter
        | AnalyzeOption.DistanceDistribution
        | AnalyzeOption.EigenDistanceDistribution
        | AnalyzeOption.EigenValues
        | AnalyzeOption.LaplacianEigenValues
        | AnalyzeOption.TriangleByVertexDistribution
        | AnalyzeOption.Dr
        | AnalyzeOption.ModelA_OR_StdTime_All
        | AnalyzeOption.ModelA_OR_ExtTime_All
        | AnalyzeOption.ModelA_OR_StdTime_Passives
        | AnalyzeOption.ModelA_OR_ExtTime_Passives
        | AnalyzeOption.ModelA_AND_StdTime_All
        | AnalyzeOption.ModelA_AND_ExtTime_All
        | AnalyzeOption.ModelA_AND_StdTime_Passives
        | AnalyzeOption.ModelA_AND_ExtTime_Passives
        | AnalyzeOption.DegreeCentrality
        | AnalyzeOption.ClosenessCentrality
        | AnalyzeOption.BetweennessCentrality
        | AnalyzeOption.Degeneracy
        | AnalyzeOption.CCS
        )]
    public class WSNetwork : AbstractNetwork
    {
        public WSNetwork(String rName,
            ResearchType rType,
            GenerationType gType,
            Dictionary<ResearchParameter, object> rParams,
            Dictionary<GenerationParameter, object> genParams,
            AnalyzeOption analyzeOpts, ContainerMode mode) : base(rName, rType, gType, rParams, genParams, analyzeOpts, mode)
        {
            networkGenerator = new WSNetworkGenerator(mode);
            networkAnalyzer = new NonHierarchicAnalyzer(this);
        }
    }
}
