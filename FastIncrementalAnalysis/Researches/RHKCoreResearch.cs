using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Enumerations;
using ERModel;
using Microsoft.Win32;
using RegularHierarchicModel;

namespace FastIncrementalAnalysis.Researches
{
    public class RHKCoreResearch : IKCoreResearch
    {
        private readonly SortedDictionary<int, DependencyAnalysisDefinition> analysis;

        private int branchingIndex;
        private int level;

        private int realizations;
        private int option;

        public RHKCoreResearch()
        {
            this.InitializeParams();
            this.analysis =  new SortedDictionary<int, DependencyAnalysisDefinition>
            {
                {1, new DependencyAnalysisDefinition{Parameter = GenerationParameter.Mu, Description = "Dependency of the K-Core topology from Mu parameter in Regular Hierarchical Network.", Activator = this.MuDependencyAnalysis} }
            };
            
        }

        private int AskForOption()
        {
            foreach (var analytics in this.analysis)
            {
                Console.WriteLine($"[{analytics.Key}] {analytics.Value.Description}");
            }

            Console.WriteLine();
            Console.Write("Choose option to start analyze (Default = 1): ");

            int res;

            if (!int.TryParse(Console.ReadLine(), out res))
            {
                res = 1;
            }

            return res;
        }

        private void InitializeParams()
        {
            Console.Write("Please enter the branching index (Default = 2): ");

            if (!int.TryParse(Console.ReadLine(), out this.branchingIndex))
            {
                this.branchingIndex = 2;
            }

            Console.Write("Please enter the number of levels (Default = 2): ");

            if (!int.TryParse(Console.ReadLine(), out this.level))
            {
                this.level = 2;
            }

            Console.Write("Please enter the number of realization (Default = 1): ");

            if (!int.TryParse(Console.ReadLine(), out this.realizations))
            {
                this.realizations = 1;
            }
        }

        private void MuDependencyAnalysis()
        {
            Bounds<double, double> bounds = this.GetMuBounds();

            Dictionary<double, ICollection<double>> allResults = new Dictionary<double, ICollection<double>>();
            for (var start = bounds.InitialValue; start <= bounds.MaxValue; start += bounds.Step)
            {
                var networks = new RegularHierarchicNetwork[this.realizations];
                var results = new double[this.realizations];

                allResults[start] = results;

                var mu = start;
                Parallel.For(0, this.realizations, new ParallelOptions{MaxDegreeOfParallelism = 8}, i =>
                {
                    var param = new Dictionary<GenerationParameter, object>
                    {
                        {GenerationParameter.Mu, mu},
                        {GenerationParameter.Level, this.level},
                        {GenerationParameter.BranchingIndex, this.branchingIndex }
                    };

                    networks[i] = (RegularHierarchicNetwork)AbstractNetwork.CreateNetworkByType(ModelType.RegularHierarchic, $"MU_{mu}_Run_{i}",
                        ResearchType.Basic, GenerationType.Random, new Dictionary<ResearchParameter, object> { }, param, AnalyzeOption.Degeneracy, ContainerMode.Fast);

                    networks[i].Generate();

                    networks[i].Analyze();

                    results[i] = (double)networks[i].NetworkResult.Result[AnalyzeOption.Degeneracy];
                });
            }

            this.SaveMuResearchResult(allResults);
            Console.WriteLine("Research is completed. You can start other experiment or close app.");
            Console.WriteLine();

        }

        /// <summary>
        /// Save result to CSV file
        /// </summary>
        /// <param name="allResults">The result set</param>
        private void SaveMuResearchResult(Dictionary<double, ICollection<double>> allResults)
        {
            var aggregated = new SortedDictionary<double, double>(allResults.ToDictionary(kv => kv.Key, kv => kv.Value.Average()));

            // Displays a SaveFileDialog so the user can save the Image  
            // assigned to Button2.  
            var saveFileDialog1 = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                Title = "Save a resulting CSV file"
            };
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.  
            if (saveFileDialog1.FileName == "") return;
            
            // Saves the Image via a FileStream created by the OpenFile method.  
            using (var fs = (FileStream) saveFileDialog1.OpenFile())
            {

                using (var writer = new StreamWriter(fs))
                {
                    writer.Write("Mu, Avg" + Environment.NewLine);

                    foreach (var r in aggregated)
                    {
                        writer.Write("{0},{1}{2}", r.Key, r.Value, Environment.NewLine);
                    }

                    writer.Flush();
                }
            }
        }
    

        private Bounds<double, double> GetMuBounds()
        {
            double min;
            double max;
            double step;

            Console.Write("Enter the Mu parameter for research (Default = 0.1): ");

            if (!double.TryParse(Console.ReadLine(), out min))
            {
                min = 0.1;
            }

            min = Math.Max(min, 0);

            Console.Write("Enter the step to increment (Default = 0.1): ");

            if (!double.TryParse(Console.ReadLine(), out step))
            {
                step = 0.1;
            }

            step = step.Equals(0.0) ? Math.Min(step, 0.1) : 0.1;

            Console.Write("Enter the maximum value (Default = 0.6): ");

            if (!double.TryParse(Console.ReadLine(), out max))
            {
                max = 0.6;
            }

            max = Math.Min(max, 1);

            return new Bounds<double, double>
            {
                InitialValue = min,
                Step = step,
                MaxValue = max
            };
        }

        public void Run()
        {
            this.option = this.AskForOption();

            if (!this.analysis.ContainsKey(this.option))
            {
                this.option = this.analysis.First().Key;
            }

            this.analysis[this.option].Activator();
        }
    }
}