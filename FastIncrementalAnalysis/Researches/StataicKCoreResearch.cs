using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Enumerations;
using FastIncrementalAnalysis.Researches;
using NetworkModel;
using QuickGraph;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace FastIncrementalAnalysis
{
    public class StataicKCoreResearch : IKCoreResearch
    {
        private readonly int maxParallel;
        private readonly SortedDictionary<int, DependencyAnalysisDefinition> analysis;
        private readonly List<string> files;

        public StataicKCoreResearch(int maxParallel)
        {
            this.maxParallel = maxParallel;
            this.files = new List<string>();
            this.InitializeParams();
            this.analysis = new SortedDictionary<int, DependencyAnalysisDefinition>
            {
                {1, new DependencyAnalysisDefinition{Parameter = GenerationParameter.AdjacencyMatrix, Description = "Dynamics of Core Collapse Sequence", Activator = this.CCSAnalysis} }
            };

        }

        private void CCSAnalysis()
        {
            var results = new Tuple<string, List<double>>[this.files.Count];
            Console.WriteLine("Started analyzing {0} graphs... ", this.files.Count);
            Parallel.For(0, this.files.Count, new ParallelOptions {MaxDegreeOfParallelism = this.maxParallel}, i =>
            {
                var file = this.files[i];
                var result = this.AnalyzeDegeneracy(file).CollapseSequence.Values.ToList();
                Console.WriteLine("Completed graph {0} ... ", i);
                var name = Path.GetFileName(file) ?? file;
                
                results[i] = new Tuple<string, List<double>>(name, result);
                
            });

            this.SaveDynamicResult(results);
        }

        private void SaveDynamicResult(Tuple<string, List<double>>[] results)
        {
            var names = new List<string>();
            foreach (var item in results)
            {
                names.Add(item.Item1.Replace(",", "_"));
            }

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
                    writer.WriteLine(string.Join(",", names));

                    var maxRows = results.Select(r => r.Item2.Count).Max();

                    for (var r = 0; r < maxRows; ++r)
                    {
                        var row = new List<string>();
                        for (var c = 0; c < names.Count; ++c)
                        {
                            var column = results[c].Item2;
                            row.Add(r >= column.Count ? string.Empty : column[r].ToString("F"));
                        }

                        writer.WriteLine(string.Join(",", row));
                    }

                }
            }
         
           
        }


        private DegeneracyResult AnalyzeDegeneracy(string file)
        {
            var graph = new UndirectedGraph<int, Edge<int>>();

            this.rowByRow(file, row =>
            {
                var split = row.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries);

                int v;
                int u;

                if (split.Length != 2 || !int.TryParse(split[0], out v) || !int.TryParse(split[1], out u))
                {
                    return;
                }

                graph.AddVerticesAndEdge(new Edge<int>(u, v));
            });

            return graph.CoreDecomposition();
        }

        private void rowByRow(string file, Action<string> action)
        {
            using (var stream = new FileStream(file, FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        action(reader.ReadLine());
                    }
                }
            }
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
            Console.WriteLine("Please select graph files you want to analyze...");
            var dialog = new OpenFileDialog {Multiselect = true};

            var dr = dialog.ShowDialog();

            int incrementStep;

            Console.Write("Please enter the increment step for given files (Default = 1): ");

            if (!int.TryParse(Console.ReadLine(), out incrementStep))
            {
                incrementStep = 1;
            }

          

            if (dr != DialogResult.OK) return;

            var names = dialog.FileNames;
            // Read the files
            for (var i = 0; i < names.Length; i += incrementStep)
            {
                this.files.Add(names[i]);
            }
        }

        public void Run()
        {
            var option = this.AskForOption();

            if (!this.analysis.ContainsKey(option))
            {
                option = this.analysis.First().Key;
            }

            this.analysis[option].Activator();
        }
    }
}