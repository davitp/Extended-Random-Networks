using System;
using System.Collections.Generic;
using FastIncrementalAnalysis.Researches;

namespace FastIncrementalAnalysis
{
    class Program
    {
        private static readonly int MaxRuns = 10000;
        private static readonly int MaxParallel = 8;

        private static readonly SortedDictionary<int, ResearchDefinition> Researches = new SortedDictionary<int, ResearchDefinition>
        {
            {1, new ResearchDefinition{ResearchName = "ER Model", ResearchCreator = () => new ERKCoreResearch(MaxParallel)} },
            {2, new ResearchDefinition{ResearchName = "Regular Hierarchic Model", ResearchCreator = () => new RHKCoreResearch(MaxParallel)} },
            {3, new ResearchDefinition{ResearchName = "BA Model", ResearchCreator = () => new BAKCoreResearch(MaxParallel)} },
            {4, new ResearchDefinition{ResearchName = "HMN Model", ResearchCreator = () => new HMNKCoreResearch(MaxParallel)} },
            {5, new ResearchDefinition{ResearchName = "Static Graphs", ResearchCreator = () => new StataicKCoreResearch(MaxParallel)}}
        };

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to K-Core analysis app...");
            Console.WriteLine();

            var run = 0;

            while (run++ < MaxRuns)
            {
                var research = Researches[AskForNetwork()].ResearchCreator();
                research.Run();

            }

            Console.ReadLine();
        }

        /// <summary>
        /// Ask for the network type
        /// </summary>
        /// <returns></returns>
        private static int AskForNetwork()
        {
            while (true)
            {
                foreach (var res in Researches)
                {
                    Console.WriteLine($"[{res.Key}] {res.Value.ResearchName}");
                }

                Console.Write("Please choose the network type to start analysis (Default = 1): ");

                int result;
                var read = Console.ReadLine();
                Console.WriteLine();

                if (int.TryParse(read, out result))
                {
                    return result > Researches.Count || result < 1 ? 1 : result;
                }

                return 1;
            }
        }
        
    }
}
