using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BAModel;
using Core;
using Core.Enumerations;
using ERModel;
using Microsoft.Win32;
using NetworkModel;
using QuickGraph;

namespace FastIncrementalAnalysis.Researches
{
    public class BAKCoreResearch : IKCoreResearch
    {
        private readonly SortedDictionary<int, DependencyAnalysisDefinition> analysis;
        private readonly int parallel;
        private int option;

        public BAKCoreResearch(int parallel)
        {
            this.parallel = parallel;
            this.analysis =  new SortedDictionary<int, DependencyAnalysisDefinition>
            {
                {1, new DependencyAnalysisDefinition{Parameter = GenerationParameter.Probability, Description = "Dependency of the K-Core topology from probability of BA Generation.", Activator = this.ProbabilityDependencyAnalysis} },
                {2, new DependencyAnalysisDefinition{Parameter = GenerationParameter.Vertices, Description = "Dependency of the active state from the external field strength for ER Generation.", Activator = this.ActivationAnalysis} }

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




        private void ActivationAnalysis()
        {
            Console.Write("Please enter the initial number of vertecies (Default = 500): ");

            int initialVertexes;

            if (!int.TryParse(Console.ReadLine(), out initialVertexes))
            {
                initialVertexes = 500;
            }

            Console.Write("Please enter the number of generation steps (Default = 500): ");
            int steps;
            if (!int.TryParse(Console.ReadLine(), out steps))
            {
                steps = 500;
            }

            Console.Write("Number of edges per step (Default = 10): ");
            int edges;
            if (!int.TryParse(Console.ReadLine(), out edges))
            {
                edges = 10;
            }

            Console.Write("Please enter generation probability (Default = 0.01): ");

            double probability;
            if (!double.TryParse(Console.ReadLine(), out probability))
            {
                probability = 0.01;
            }

            double delta;
            Console.Write("Please enter the delta value (Default = 0.001): ");

            if (!double.TryParse(Console.ReadLine(), out delta))
            {
                delta = 0.001;
            }


            var param = new Dictionary<GenerationParameter, object>
            {
                {GenerationParameter.Probability, probability},
                {GenerationParameter.Vertices, initialVertexes },
                {GenerationParameter.Edges, edges },
                {GenerationParameter.StepCount, steps }
            };

            var network = (BANetwork)AbstractNetwork.CreateNetworkByType(ModelType.BA, $"Prob_{probability}_",
                ResearchType.Basic, GenerationType.Random, new Dictionary<ResearchParameter, object> { }, param, AnalyzeOption.None, ContainerMode.Fast);
   
            network.Generate();
            network.Analyze();
            var quick = ((IQuickGraphConverter)network.GetAnalyzer()).ToQuickGraph();

            var decomposition = quick.CoreDecomposition();

            var energySequence = new List<double>();
            var activePortions = new List<double>();
            var uValues = new List<double>();

            for (var k = 1; k <= decomposition.Degeneracy + 2; ++k)
            {
                var uValue = -delta - k;
                uValues.Add(Math.Abs(uValue));
                var kSize = decomposition.CoreDescritpros.ContainsKey(k) ? decomposition.CoreDescritpros[k] : new CoreDescriptor(k, 0, 0);

                var vertexPortion = kSize.VertexCount * 1.0 / quick.VertexCount;
                activePortions.Add(vertexPortion);

                var energy = -1.0 * kSize.EdgeCount / quick.VertexCount + (k - 1 + delta) * vertexPortion;

                energySequence.Add(energy);
            }

            this.SaveActivationResearchResult(quick, decomposition, uValues, activePortions, energySequence);
        }

        private void SaveActivationResearchResult(UndirectedGraph<int, Edge<int>> quick, DegeneracyResult decomposition, List<double> uValues, List<double> activePortions, List<double> energySequence)
        {
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
            using (var fs = (FileStream)saveFileDialog1.OpenFile())
            {
                using (var writer = new StreamWriter(fs))
                {
                    writer.Write("K,U,Mk,Ek" + Environment.NewLine);

                    for (var k = 0; k < uValues.Count; ++k)
                    {
                        writer.Write("{0},{1},{2},{3}{4}", k + 1, uValues[k], activePortions[k], energySequence[k], Environment.NewLine);
                    }

                    writer.Flush();
                }
            }
        }

        private void ProbabilityDependencyAnalysis()
        {
            Console.Write("Please enter the initial number of vertecies (Default = 500): ");

            int initialVertexes;

            if (!int.TryParse(Console.ReadLine(), out initialVertexes))
            {
                initialVertexes = 500;
            }

            Console.Write("Please enter the number of generation steps (Default = 500): ");
            int steps;
            if (!int.TryParse(Console.ReadLine(), out steps))
            {
                steps = 500;
            }

            Console.Write("Number of edges per step (Default = 10): ");
            int edges;
            if (!int.TryParse(Console.ReadLine(), out edges))
            {
                edges = 10;
            }

            Console.Write("Please enter the number of realization (Default = 1): ");

            int realizations;
            if (!int.TryParse(Console.ReadLine(), out realizations))
            {
                realizations = 1;
            }
            Bounds<double, double> bounds = this.GetProbabilityBounds();

            Dictionary<double, ICollection<double>> allResults = new Dictionary<double, ICollection<double>>();
            for (var start = bounds.InitialValue; start <= bounds.MaxValue; start += bounds.Step)
            {
                var networks = new BANetwork[realizations];
                var results = new double[realizations];

                allResults[start] = results;

                var prob = start;
                Parallel.For(0, realizations, new ParallelOptions{MaxDegreeOfParallelism = this.parallel}, i =>
                {
                    var param = new Dictionary<GenerationParameter, object>
                    {
                        {GenerationParameter.Probability, prob},
                        {GenerationParameter.Vertices, initialVertexes },
                        {GenerationParameter.Edges, edges },
                        {GenerationParameter.StepCount, steps }
                    };

                    networks[i] = (BANetwork)AbstractNetwork.CreateNetworkByType(ModelType.BA, $"Prob_{prob}_Run_{i}",
                        ResearchType.Basic, GenerationType.Random, new Dictionary<ResearchParameter, object>(), param, AnalyzeOption.Degeneracy, ContainerMode.Fast);
                    
                    networks[i].Generate();

                    networks[i].Analyze();

                    results[i] = (double)networks[i].NetworkResult.Result[AnalyzeOption.Degeneracy];
                });
            }

            this.SaveProbResearchResult(allResults);
            Console.WriteLine("Research is completed. You can start other experiment or close app.");
            Console.WriteLine();

        }

        /// <summary>
        /// Save result to CSV file
        /// </summary>
        /// <param name="allResults">The result set</param>
        private void SaveProbResearchResult(Dictionary<double, ICollection<double>> allResults)
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
                    writer.Write("Probability, Avg" + Environment.NewLine);

                    foreach (var r in aggregated)
                    {
                        writer.Write("{0},{1}{2}", r.Key, r.Value, Environment.NewLine);
                    }

                    writer.Flush();
                }
            }
        }
    

        private Bounds<double, double> GetProbabilityBounds()
        {
            double min;
            double max;
            double step;

            Console.Write("Enter the probability for research (Default = 0.1): ");

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