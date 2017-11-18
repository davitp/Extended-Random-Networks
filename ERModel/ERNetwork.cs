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
        | AnalyzeOption.Cycles3Trajectory
        | AnalyzeOption.Dr
        | AnalyzeOption.ModelA_AND_StdTime_Actives
        | AnalyzeOption.ModelA_OR_ExtTime_Actives
        | AnalyzeOption.ModelA_OR_ExtTime_All
        | AnalyzeOption.ModelA_OR_StdTime_Actives
        | AnalyzeOption.ModelA_OR_StdTime_All
        | AnalyzeOption.DegreeCentrality
        | AnalyzeOption.ClosenessCentrality
        | AnalyzeOption.BetweennessCentrality
        )]
    public class ERNetwork : AbstractNetwork
    {
        public ERNetwork(String rName,
            ResearchType rType,
            GenerationType gType,
            Dictionary<ResearchParameter, object> rParams,
            Dictionary<GenerationParameter, object> genParams,
            AnalyzeOption analyzeOpts) : base(rName, rType, gType, rParams, genParams, analyzeOpts)
        {
            networkGenerator = new ERNetworkGenerator();
            networkAnalyzer = new NonHierarchicAnalyzer(this);
        }
    }
}
