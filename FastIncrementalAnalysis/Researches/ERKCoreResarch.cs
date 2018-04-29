using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Enumerations;
using ERModel;

namespace FastIncrementalAnalysis.Researches
{
    public class ERKCoreResarch : IKCoreResearch
    {
        private SortedDictionary<int, DependencyAnalysisDefinition> analysis;

        private int initialVertexes;
        private int realizations;
        private int option;

        public ERKCoreResarch()
        {
            this.InitializeParams();
            this.analysis =  new SortedDictionary<int, DependencyAnalysisDefinition>
            {
                {1, new DependencyAnalysisDefinition{Parameter = GenerationParameter.Probability, Description = "Dependency of the K-Core topology from probability of ER Generation.", Activator = this.ProbabilityDependencyAnalysis} }
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

            return 1;
        }

        private void InitializeParams()
        {
            Console.Write("Please enter the number of vertecies (Default = 24): ");

            if (!int.TryParse(Console.ReadLine(), out this.initialVertexes))
            {
                this.initialVertexes = 24;
            }

            Console.Write("Please enter the number of realization (Default = 1): ");

            if (!int.TryParse(Console.ReadLine(), out this.realizations))
            {
                this.realizations = 1;
            }
        }

        private void ProbabilityDependencyAnalysis()
        {
            Bounds<double, double> bounds = this.GetProbabilityBounds();

            Dictionary<double, ICollection<double>> allResults = new Dictionary<double, ICollection<double>>();
            for (var start = bounds.InitialValue; start < bounds.MaxValue; start += bounds.Step)
            {
                var networks = new ERNetwork[this.realizations];
                var results = new double[this.realizations];

                allResults[start] = results;

                var prob = start;
                Parallel.For(0, this.realizations, new ParallelOptions{MaxDegreeOfParallelism = 8}, i =>
                {
                    var param = new Dictionary<GenerationParameter, object>
                    {
                        {GenerationParameter.Probability, prob},
                        {GenerationParameter.Vertices, this.initialVertexes }
                    };

                    networks[i] = (ERNetwork)AbstractNetwork.CreateNetworkByType(ModelType.ER, $"Prob_{prob}_Run_{i}",
                        ResearchType.Basic, GenerationType.Random, new Dictionary<ResearchParameter, object> { }, param, AnalyzeOption.Degeneracy);

                    networks[i].Generate();

                    networks[i].Analyze();

                    results[i] = (double)networks[i].NetworkResult.Result[AnalyzeOption.Degeneracy];
                });

               
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

            step = step.Equals(0.0) ? Math.Min(step, 0.1) : 0.1;

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