using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core;
using Core.Attributes;
using Core.Enumerations;
using NetworkModel;

namespace IMModel
{
    [RequiredGenerationParameter(GenerationParameter.Vertices)]
    [RequiredGenerationParameter(GenerationParameter.Probability)]
    [RequiredGenerationParameter(GenerationParameter.Alpha)]
    [RequiredGenerationParameter(GenerationParameter.ZeroLevelNodesCount)]
    [RequiredGenerationParameter(GenerationParameter.BlocksCount)]
    [AvailableAnalyzeOption(
          AnalyzeOption.AvgClusteringCoefficient
        | AnalyzeOption.AvgDegree
        | AnalyzeOption.AvgPathLength
        | AnalyzeOption.ClusteringCoefficientDistribution
        | AnalyzeOption.ClusteringCoefficientPerVertex
        | AnalyzeOption.ConnectedComponentDistribution
        | AnalyzeOption.CompleteComponentDistribution
        | AnalyzeOption.SubtreeDistribution
        | AnalyzeOption.CycleDistribution
        | AnalyzeOption.Cycles3
        | AnalyzeOption.Cycles4
        | AnalyzeOption.DegreeDistribution
        | AnalyzeOption.Diameter
        | AnalyzeOption.DistanceDistribution
        | AnalyzeOption.EigenDistanceDistribution
        | AnalyzeOption.EigenValues
        | AnalyzeOption.TriangleByVertexDistribution
        )]
    public class IMNetwork : AbstractNetwork
    {
        public IMNetwork(String rName,
            GenerationType gType,
            Dictionary<ResearchParameter, object> rParams,
            Dictionary<GenerationParameter, object> genParams,
            AnalyzeOption analyzeOpts) : base(rName, gType, rParams, genParams, analyzeOpts)
        {
            networkGenerator = new IMNetworkGenerator();
            networkAnalyzer = new NonHierarchicAnalyzer(this);
        }
    }
}
