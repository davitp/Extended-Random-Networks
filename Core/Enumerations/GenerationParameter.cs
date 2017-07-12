using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.Utility;
using Core.Model;
using Core.Attributes;

namespace Core.Enumerations
{
    /// <summary>
    /// Enumeration of generation parameters that should be used for generating of a random network.
    /// Uses Attribute GenerationParameterInfo for storing metadata about every generation parameter.
    /// This metadata is used mainly for validating and getting user-friendly information.
    /// </summary>
    public enum GenerationParameter
    {
        [GenerationParameterInfo("Number of vertices", 
            "The initial number of vertices in the network.", 
            typeof(UInt32), "24")]
        Vertices,

        [GenerationParameterInfo("Connectivity probability", 
            "The probability of existance of a connection between the nodes in the network.", 
            typeof(Double), "0.1")]
        Probability,

        [GenerationParameterInfo("Permanent network", 
            "Defines if the initial network is permanent for each generation step.", 
            typeof(Boolean), "false")]
        PermanentNetwork,

        [GenerationParameterInfo("Number of edges", 
            "The initial number of edges in the network.", 
            typeof(UInt32), "1")]
        Edges,

        [GenerationParameterInfo("Step count", 
            "The number of generation steps to get each network in the ensemble.", 
            typeof(UInt32), "1")]
        StepCount,

        [GenerationParameterInfo("Branching index", 
            "The branching index of the block-hierarchical network.", 
            typeof(UInt32), "2")]
        BranchingIndex,

        [GenerationParameterInfo("Γ - Level", 
            "The level of the block-hierarchical network.", 
            typeof(UInt32), "1")]
        Level,

        [GenerationParameterInfo("μ", 
            "The density parameter of the block-hierarchical network.", 
            typeof(Double), "0.1")]
        Mu,

        [GenerationParameterInfo("M0",
            "Count of nodes on zero level.",
            typeof(UInt32), "3")]
        ZeroLevelNodesCount,

        [GenerationParameterInfo("α",
            "Random parameter of IM network",
            typeof(Double), "0.1")]
        Alpha,

        [GenerationParameterInfo("b",
            "Count of blocks.",
            typeof(UInt32), "2")]
        BlocksCount,

        [GenerationParameterInfo("Make Connected",
            "",
            typeof(Boolean), "true")]
        MakeConnected,

        [GenerationParameterInfo("Adjacency matrix",
            "Adjacency matrix and branches of the networks.",
            typeof(MatrixPath), null)]
        AdjacencyMatrix
    }
}
