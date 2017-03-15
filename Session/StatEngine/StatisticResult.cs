using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Core.Attributes;
using Core.Enumerations;
using Core.Result;
using Session;

/// !!!!!!!!!!Support for only basic research where ensemble count is 1!!!!!!!!!!!!
namespace Session.StatEngine
{
    /// <summary>
    /// 
    /// </summary>
    public struct StatisticsOption
    {
        public ApproximationType ApproximationType;
        public ThickeningType ThickeningType;
        public int ThickeningValue;

        public StatisticsOption(ApproximationType at, ThickeningType tt, int v)
        {
            ApproximationType = at;
            ThickeningType = tt;
            ThickeningValue = v;
        }
    };

    /// <summary>
    /// Represents the statistic analyze result.
    /// </summary>
    public class StatisticResult
    {
        private List<Guid> researches;

        public double RealizationCountSum { get; private set; }
        public double EdgesCountAvg { get; private set; }

        public List<EnsembleResult> EnsembleResultsAvg { get; private set; }

        public StatisticResult(List<Guid> r)
        {
            researches = r;
            EnsembleResultsAvg = new List<EnsembleResult>();

            CalculateInfo();
        }

        /// <summary>
        /// Calculates summary of realizations and the average value of edges.
        /// </summary>
        private void CalculateInfo()
        {
            ResearchResult tr = null;
            foreach (Guid id in researches)
            {
                tr = StatSessionManager.GetResearchResult(id);
                RealizationCountSum += tr.RealizationCount;
                EdgesCountAvg += tr.Edges * tr.RealizationCount;
            }
            EdgesCountAvg = Math.Round(EdgesCountAvg / RealizationCountSum, 4);
        }

        /// <summary>
        /// Calculates the average value of specified global analyze option.
        /// </summary>
        /// <param name="opt">Analyze option.</param>
        public void CalculateGlobalOption(AnalyzeOption opt)
        {
            if (EnsembleResultsAvg.Count != 0 && EnsembleResultsAvg[0].Result.ContainsKey(opt))
                return;

            ResearchResult tr = null;
            AnalyzeOptionInfo[] info = (AnalyzeOptionInfo[])opt.GetType().GetField(opt.ToString()).GetCustomAttributes(typeof(AnalyzeOptionInfo), false);
            Debug.Assert(info[0].OptionType == OptionType.Global);

            double avg = 0;
            foreach (Guid id in researches)
            {
                tr = StatSessionManager.GetResearchResult(id);
                Debug.Assert(tr.EnsembleResults.Count == 1);
                // TODO fix it
                if (!tr.EnsembleResults[0].Result.ContainsKey(opt))
                    continue;
                Debug.Assert(tr.EnsembleResults[0].Result[opt] != null);
                avg += Convert.ToDouble(tr.EnsembleResults[0].Result[opt]) * tr.RealizationCount;                
            }
            avg /= RealizationCountSum;
            EnsembleResult er = null;
            if (EnsembleResultsAvg.Count == 0)
            {
                er = new EnsembleResult(0);
                EnsembleResultsAvg.Add(er);
            }
            else
                er = EnsembleResultsAvg[0];
            er.Result.Add(opt, Math.Round(avg, 4));
        }
        
        /// <summary>
        /// Calculates the average distribution of specified distributed analyze option.
        /// </summary>
        /// <param name="opt">Analyze option.</param>
        public void CalculateDistributedOption(AnalyzeOption opt)
        {
            if (EnsembleResultsAvg.Count != 0 && EnsembleResultsAvg[0].Result.ContainsKey(opt))
                return;

            AnalyzeOptionInfo[] info = (AnalyzeOptionInfo[])opt.GetType().GetField(opt.ToString()).GetCustomAttributes(typeof(AnalyzeOptionInfo), false);
            Debug.Assert(info[0].OptionType == OptionType.Distribution);
            EnsembleResult er = null;
            if (EnsembleResultsAvg.Count == 0)
            {
                er = new EnsembleResult(0);
                EnsembleResultsAvg.Add(er);
            }
            else
                er = EnsembleResultsAvg[0];
            er.Result.Add(opt, CalculateDoubleAverage(opt));
        }

        private SortedDictionary<Double, Double> CalculateDoubleAverage(AnalyzeOption opt)
        {
            SortedDictionary<Double, Double> temp = new SortedDictionary<Double, Double>();
            ResearchResult tr = null;
            foreach (Guid id in researches)
            {
                tr = StatSessionManager.GetResearchResult(id);
                Debug.Assert(tr.EnsembleResults.Count == 1);
                // TODO fix it
                if (!tr.EnsembleResults[0].Result.ContainsKey(opt))
                    continue;
                Debug.Assert(tr.EnsembleResults[0].Result[opt] != null);

                Debug.Assert(tr.EnsembleResults[0].Result[opt] is SortedDictionary<Double, Double>);
                SortedDictionary<Double, Double> d = tr.EnsembleResults[0].Result[opt] as SortedDictionary<Double, Double>;
                foreach (KeyValuePair<Double, Double> k in d)
                {
                    double value = Math.Round(k.Value * tr.RealizationCount / RealizationCountSum, 4);
                    if (temp.ContainsKey(k.Key))
                        temp[k.Key] += value;
                    else
                        temp.Add(k.Key, value);
                }
            }
            return temp;
        }
    }
}
