using System;
using FastIncrementalAnalysis.Researches;

namespace FastIncrementalAnalysis
{
    public class ResearchDefinition
    {
        public string ResearchName { get; set; }

        public Func<IKCoreResearch> ResearchCreator { get; set; }
    }
}