using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core;
using Core.Attributes;
using Core.Enumerations;
using NetworkModel;

namespace ERModel
{
    /// <summary>
    /// Implementation of random network of Erdős-Rényi model's model.
    /// </summary>
    [RequiredGenerationParameter(GenerationParameter.AdjacencyMatrix)]
    [RequiredGenerationParameter(GenerationParameter.Vertices)]
    [RequiredGenerationParameter(GenerationParameter.Probability)]
    [AvailableAnalyzeOption(AnalyzeOption.AvgClusteringCoefficient |
        AnalyzeOption.AvgDegree |
        AnalyzeOption.AvgPathLength |
        AnalyzeOption.ClusteringCoefficientDistribution |
        AnalyzeOption.ClusteringCoefficientPerVertex |
        AnalyzeOption.ConnectedComponentDistribution |
        AnalyzeOption.SubtreeDistribution |
        AnalyzeOption.CycleDistribution |
        AnalyzeOption.Cycles3 |
        AnalyzeOption.Cycles4 |
        AnalyzeOption.Cycles3Trajectory |
        AnalyzeOption.DegreeDistribution |
        AnalyzeOption.Diameter |
        AnalyzeOption.DistanceDistribution |
        AnalyzeOption.EigenDistanceDistribution |
        AnalyzeOption.EigenValues |
        AnalyzeOption.TriangleByVertexDistribution |
        AnalyzeOption.Dr)]// |
        //AnalyzeOption.DegreeCentrality |
        //AnalyzeOption.BetweennessCentrality |
        //AnalyzeOption.ClosenessCentrality)]
    public class ERNetwork : AbstractNetwork
    {
        public ERNetwork(String rName,
            GenerationType gType,
            Dictionary<ResearchParameter, object> rParams,
            Dictionary<GenerationParameter, object> genParams,
            AnalyzeOption analyzeOpts) : base(rName, gType, rParams, genParams, analyzeOpts)
        {
            networkGenerator = new ERNetworkGenerator();
            networkAnalyzer = new NonHierarchicAnalyzer(this);
        }
    }
}
