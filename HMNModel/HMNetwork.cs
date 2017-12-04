using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core;
using Core.Attributes;
using Core.Enumerations;
using NetworkModel;

namespace HMNModel
{
    [RequiredGenerationParameter(GenerationParameter.Vertices)]
    [RequiredGenerationParameter(GenerationParameter.Probability)]
    [RequiredGenerationParameter(GenerationParameter.Alpha)]
    [RequiredGenerationParameter(GenerationParameter.ZeroLevelNodesCount)]
    [RequiredGenerationParameter(GenerationParameter.BlocksCount)]
    [RequiredGenerationParameter(GenerationParameter.MakeConnected)]
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
        )]
    public class HMNetwork : AbstractNetwork
    {
        public HMNetwork(String rName,
            ResearchType rType,
            GenerationType gType,
            Dictionary<ResearchParameter, object> rParams,
            Dictionary<GenerationParameter, object> genParams,
            AnalyzeOption analyzeOpts) : base(rName, rType, gType, rParams, genParams, analyzeOpts)
        {
            networkGenerator = new HMNetworkGenerator();
            networkAnalyzer = new NonHierarchicAnalyzer(this);
        }
    }
}
