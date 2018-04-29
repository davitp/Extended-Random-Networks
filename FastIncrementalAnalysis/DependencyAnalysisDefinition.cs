using System;
using Core.Enumerations;

namespace FastIncrementalAnalysis
{
    public class DependencyAnalysisDefinition
    {
        public GenerationParameter Parameter { get; set; }

        public string Description { get; set; }

        public Action Activator { get; set; }
    }
}