using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Enumerations;
using ERModel;
using FastIncrementalAnalysis;
using HMNModel;
using Microsoft.Win32;

namespace FastIncrementalAnalysis.Researches
{
    public class HMNKCoreResearch : IKCoreResearch
    {
        private readonly SortedDictionary<int, DependencyAnalysisDefinition> analysis;

        private int initialVertexes;
        private int blocks;
        private int alpha;
        private int zeroNodes;
        private int realizations;
        private int option;
        private int parallel;

        public HMNKCoreResearch(int parallel)
        {
            this.parallel = parallel;
            this.InitializeParams();
            this.analysis =  new SortedDictionary<int, DependencyAnalysisDefinition>
            {
                {1, new DependencyAnalysisDefinition{Parameter = GenerationParameter.Probability, Description = "Dependency of the K-Core topology from Mu (P = b^-Mu) of HMN Network.", Activator = this.MuDependencyAnalysis} }
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
            Console.Write("Please enter the number of vertecies (Default = 24): ");

            if (!int.TryParse(Console.ReadLine(), out this.initialVertexes))
            {
                this.initialVertexes = 24;
            }

            Console.Write("Please enter the number of blocks (Default = 2): ");

            if (!int.TryParse(Console.ReadLine(), out this.blocks))
            {
                this.blocks = 2;
            }

            Console.Write("Please enter the Alpha value (Default = 1): ");

            if (!int.TryParse(Console.ReadLine(), out this.alpha))
            {
                this.alpha = 1;
            }

            Console.Write("Please enter the number of zero level nodes (Default = 2): ");

            if (!int.TryParse(Console.ReadLine(), out this.zeroNodes))
            {
                this.zeroNodes = 2;
            }

            Console.Write("Please enter the number of realization (Default = 1): ");

            if (!int.TryParse(Console.ReadLine(), out this.realizations))
            {
                this.realizations = 1;
            }
        }

        private void MuDependencyAnalysis()
        {
            Console.WriteLine("Starting Mu dependency research considering P = b^mu");

            Bounds<double, double> bounds = this.GetMuBounds();

            Dictionary<double, ICollection<double>> allResults = new Dictionary<double, ICollection<double>>();
            for (var start = Math.Min(bounds.InitialValue, bounds.MaxValue); start <= Math.Max(bounds.InitialValue, bounds.MaxValue); start += bounds.Step)
            {
                var networks = new HMNetwork[this.realizations];
                var results = new double[this.realizations];

                var mu = start;
                var prob = Math.Pow(this.blocks, -mu);
                allResults[start] = results;

                Parallel.For(0, this.realizations, new ParallelOptions{MaxDegreeOfParallelism = this.parallel}, i =>
                {
                    var param = new Dictionary<GenerationParameter, object>
                    {
                        {GenerationParameter.Probability, prob},
                        {GenerationParameter.BlocksCount, this.blocks },
                        {GenerationParameter.MakeConnected, true },
                        {GenerationParameter.Alpha, this.alpha},
                        {GenerationParameter.ZeroLevelNodesCount, this.zeroNodes},
                        {GenerationParameter.Vertices, this.initialVertexes }
                    };

                    networks[i] = (HMNetwork)AbstractNetwork.CreateNetworkByType(ModelType.HMN, $"Mu_{prob}_Run_{i}",
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

            Console.Write("Enter the Mu for research (Default = 0.1): ");

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

            step = !step.Equals(0.0) ? Math.Min(step, 0.1) : 0.1;

            Console.Write("Enter the maximum value (Default = 1): ");

            if (!double.TryParse(Console.ReadLine(), out max))
            {
                max = 1.0;
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