using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.Attributes;

namespace Core.Enumerations
{
    /// <summary>
    /// Enumaration, used for indicating Research types.
    /// Uses Attribute ResearchTypeInfo for storing metadata about every Research.
    /// </summary>
    public enum ResearchType
    {
        [ResearchTypeInfo("Basic research", 
            "The basic analysis for random networks (Double ensemble).", 
            "Research.BasicResearch, Research")]
        Basic,

        [ResearchTypeInfo("Evolution research", 
            "Analysis the evolution of trajectories for random networks (several ensembles).", 
            "Research.EvolutionResearch, Research")]
        Evolution,

        [ResearchTypeInfo("Percolation research", 
            "Analysis of percolation probability (critical probability) for random network (several ensembles).", 
            "Research.PercolationResearch, Research")]
        Percolation,

        [ResearchTypeInfo("Collection of researches",
            "",
            "Research.CollectionResearch, Research")]
        Collection,

        [ResearchTypeInfo("Structural research",
            "",
            "Research.StructuralResearch, Research")]
        Structural,

        [ResearchTypeInfo("Activation research",
            "",
            "Research.ActivationResearch, Research")]
        Activation
    }
}