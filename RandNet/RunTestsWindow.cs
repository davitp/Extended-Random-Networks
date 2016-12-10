using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using Core;
using Core.Attributes;
using Core.Enumerations;
using Core.Result;
using Core.Utility;
using Session;

namespace RandNet
{
    public partial class RunTestsWindow : Form
    {
        public ModelType ModelType { get; set; }

        private static string parent = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
        private static string matrixesPath = parent + "\\Tests\\matrixes";
        private static string erPath = parent + "\\Tests\\er_tests";
        private static string regularHierarchicPath = parent + "\\Tests\\regular_hierarchic_tests";
        private static string nonRegularHierarchicPath = parent + "\\Tests\\non_regular_hierarchic_tests";

        private static string[] matrixFiles = { "h1_0.txt", "h2_0.txt", "h3_0.txt", "h4_0.txt", "h5_0.txt", "h6_0.txt",
                                              "h7_0.txt", "h8_0.txt", "h9_0.txt", "h10_0.txt", "h11_0.txt", "h12_0.txt",
                                              "h13_0.txt", "h14_0.txt", "h15_0.txt", "h16_0.txt", "h17_0.txt", "h18_0.txt",
                                              "h19_0.txt", "h20_0.txt", "h21_0.txt", "h22_0.txt", "h23_0.txt", "h24_0.txt",
                                              "h25_0.txt"};
        private static string[] erFiles = { "e1.xml", "e2.xml", "e3.xml", "e4.xml", "e9.xml", "e10.xml", "e11.xml", "e12.xml",
                                          "e21.xml" };//, "e23.xml"};
        private static string[] regularHierarchicFiles = { "h1.xml", "h2.xml", "h3.xml", "h4.xml", "h5.xml", "h6.xml", "h7.xml",
                                                         "h8.xml", "h9.xml", "h10.xml", "h11.xml", "h12.xml", "h13.xml", "h14.xml",
                                                         "h15.xml", "h16.xml", "h17.xml", "h18.xml", "h19.xml", "h20.xml", "h21.xml",
                                                         "h22.xml", "h23.xml", "h24.xml", "h25.xml"};
        private static string[] nonRegularHierarchicFiles = { "nh1.xml", "nh2.xml", "nh3.xml", "nh4.xml", "nh5.xml", "nh6.xml", "nh7.xml",
                                                         "nh8.xml", "nh9.xml", "nh10.xml", "nh11.xml", "nh12.xml", "nh13.xml", "nh14.xml",
                                                         "nh15.xml", "nh16.xml", "nh17.xml", "nh18.xml", "nh19.xml", "nh20.xml", "nh21.xml",
                                                         "nh22.xml", "nh23.xml", "nh24.xml", "nh25.xml"};

        private static AbstractResultStorage erStorage = AbstractResultStorage.CreateStorage(StorageType.XMLStorage, erPath);
        private static AbstractResultStorage regularHierarchicStorage =
            AbstractResultStorage.CreateStorage(StorageType.XMLStorage, regularHierarchicPath);
        private static AbstractResultStorage nonRegularHierarchicStorage =
            AbstractResultStorage.CreateStorage(StorageType.XMLStorage, nonRegularHierarchicPath);

        public RunTestsWindow()
        {
            ModelType = ModelType.ER;
            InitializeComponent();
        }

        #region Event Handlers

        private void RunTestsWindow_Load(object sender, EventArgs e)
        {
            Text = ModelType.ToString() + " tests";
        }

        private void start_Click(object sender, EventArgs e)
        {
            start.Enabled = false;
            close.Enabled = false;
            switch (ModelType)
            {
                case ModelType.RegularHierarchic:
                    RunRegularHierarchicTests();
                    break;
                case ModelType.NonRegularHierarchic:
                    RunNonRegularHierarchicTests();
                    break;
                default:
                    RunERTests();
                    break;
            }
        }

        #endregion

        #region Utilities

        private void RunERTests()
        {
            RunTest(0, 0, 0, 0);
            RunTest(1, 1, 1, 1);
            RunTest(2, 2, 2, 2);
            RunTest(3, 3, 3, 3);

            RunTest(8, 4, 8, 8);
            RunTest(9, 5, 9, 9);
            RunTest(10, 6, 10, 10);
            RunTest(11, 7, 11, 11);

            RunTest(20, 8, 20, 20);
            //RunTest(22, 9, 22, 22);
            MessageBox.Show("Finished running Classic tests.");
            close.Enabled = true;
        }

        private void RunRegularHierarchicTests()
        {
            RunHierarchicTests();
            MessageBox.Show("Finished running Regular Hierarchic tests.");
            close.Enabled = true;
        }

        private void RunNonRegularHierarchicTests()
        {
            RunHierarchicTests();
            MessageBox.Show("Finished running Non Regular Hierarchic tests.");
            close.Enabled = true;
        }

        private void RunHierarchicTests()
        {
            RunTest(0, 0, 0, 0);
            RunTest(1, 1, 1, 1);
            RunTest(2, 2, 2, 2);
            RunTest(3, 3, 3, 3);

            RunTest(4, 25, 4, 4);
            RunTest(5, 25, 5, 5);
            RunTest(6, 25, 6, 6);
            RunTest(7, 25, 7, 7);

            RunTest(8, 4, 8, 8);
            RunTest(9, 5, 9, 9);
            RunTest(10, 6, 10, 10);
            RunTest(11, 7, 11, 11);

            RunTest(12, 25, 12, 12);
            RunTest(13, 25, 13, 13);
            RunTest(14, 25, 14, 14);
            RunTest(15, 25, 15, 15);
            RunTest(16, 25, 16, 16);
            RunTest(17, 25, 17, 17);
            RunTest(18, 25, 18, 18);
            RunTest(19, 25, 19, 19);

            RunTest(20, 8, 20, 20);

            RunTest(21, 25, 21, 21);
            RunTest(22, 25, 22, 22);
            RunTest(23, 25, 23, 23); // RunTest(22, 9, 22, 22);
            RunTest(24, 25, 24, 24);
        }

        private void RunTest(uint matrixIndex, uint erIndex, uint hIndex, uint nhIndex)
        {
            Debug.Assert(matrixIndex < matrixFiles.Count());

            MatrixPath m = new MatrixPath();
            m.Path = matrixesPath + "\\" + matrixFiles[matrixIndex];

            Dictionary<GenerationParameter, object> gp = new Dictionary<GenerationParameter, object>();
            gp.Add(GenerationParameter.AdjacencyMatrix, m);
            AnalyzeOption opts = SessionManager.GetAvailableAnalyzeOptions(ResearchType.Basic, ModelType);
            opts &= ~AnalyzeOption.EigenValues;
            opts &= ~AnalyzeOption.EigenDistanceDistribution;
            opts &= ~AnalyzeOption.CycleDistribution;
            AbstractNetwork n = AbstractNetwork.CreateNetworkByType(ModelType, "",
                GenerationType.Static, 
                new Dictionary<ResearchParameter,object>(),
                gp,
                opts);
            n.Generate();
            n.Analyze();
            RealizationResult rn = n.NetworkResult;

            ResearchResult er = null;
            if(erIndex < erFiles.Count())
                er = erStorage.Load(erPath + "\\" + erFiles[erIndex]);

            Debug.Assert(hIndex < regularHierarchicFiles.Count());
            ResearchResult hr = regularHierarchicStorage.Load(regularHierarchicPath + "\\" + regularHierarchicFiles[hIndex]);

            Debug.Assert(nhIndex < nonRegularHierarchicFiles.Count());
            ResearchResult nhr = nonRegularHierarchicStorage.Load(nonRegularHierarchicPath + "\\" + nonRegularHierarchicFiles[nhIndex]);

            string s;
            foreach (AnalyzeOption o in rn.Result.Keys)
            {
                if (er != null)
                {
                    s = Check(o, rn, er) ? "Passed" : "Failed";
                    optionsTable.Rows.Add(o.ToString(), er.ResearchName, s);
                    optionsTable.FirstDisplayedScrollingRowIndex = optionsTable.RowCount - 1;
                    Application.DoEvents();
                }

                s = Check(o, rn, hr) ? "Passed" : "Failed";
                optionsTable.Rows.Add(o.ToString(), hr.ResearchName, s);
                optionsTable.FirstDisplayedScrollingRowIndex = optionsTable.RowCount - 1;
                Application.DoEvents();

                s = Check(o, rn, nhr) ? "Passed" : "Failed";
                optionsTable.Rows.Add(o.ToString(), nhr.ResearchName, s);
                optionsTable.FirstDisplayedScrollingRowIndex = optionsTable.RowCount - 1;
                Application.DoEvents();
            }
        }

        private bool Check(AnalyzeOption o, RealizationResult rn, ResearchResult rr)
        {
            Debug.Assert(rn.Result.ContainsKey(o));
            Debug.Assert(rn.Result[o] != null);
            Debug.Assert(rr.EnsembleResults.Count() > 0);
            if (!rr.EnsembleResults[0].Result.ContainsKey(o))
                return true;
            Debug.Assert(rr.EnsembleResults[0].Result[o] != null);

            AnalyzeOptionInfo[] info = (AnalyzeOptionInfo[])o.GetType().GetField(o.ToString()).GetCustomAttributes(typeof(AnalyzeOptionInfo), false);
            OptionType ot = info[0].OptionType;
            switch (ot)
            {
                case OptionType.Global:
                    Double v1 = Convert.ToDouble(rn.Result[o]);
                    Double v2 = Convert.ToDouble(rr.EnsembleResults[0].Result[o]);
                    return Math.Round(v1, 4) == v2;
                case OptionType.Distribution:
                case OptionType.Trajectory:
                    Debug.Assert(rn.Result[o] is SortedDictionary<Double, Double>);
                    Debug.Assert(rr.EnsembleResults[0].Result[o] is SortedDictionary<Double, Double>);
                    SortedDictionary<Double, Double> d1 = rn.Result[o] as SortedDictionary<Double, Double>;
                    SortedDictionary<Double, Double> d2 = rr.EnsembleResults[0].Result[o] as SortedDictionary<Double, Double>;
                    return CheckDictionary(d1, d2);
                default:
                    Debug.Assert(false);
                    break;
            }
            
            return true;
        }

        private bool CheckDictionary(SortedDictionary<Double, Double> d1, SortedDictionary<Double, Double> d2)
        {
            if (d1.Count() != d2.Count())
                return false;

            foreach (Double k in d1.Keys)
            {
                if (!d2.ContainsKey(k))
                    return false;
                if (Math.Round(d1[k], 4) != d2[k])
                    return false;
            }

            return true;
        }

        #endregion
    }
}
