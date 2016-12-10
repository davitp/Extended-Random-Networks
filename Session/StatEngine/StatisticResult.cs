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
        public double ThickeningValue;

        public StatisticsOption(ApproximationType at, ThickeningType tt, Double v)
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
                foreach (Double k in d.Keys)
                {
                    double value = Math.Round(d[k] * tr.RealizationCount / RealizationCountSum, 4);
                    if (temp.ContainsKey(k))
                        temp[k] += value;
                    else
                        temp.Add(k, value);
                }
            }
            return temp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eIndex"></param>
        /// <param name="opt"></param>
        /// <param name="sopt"></param>
        /// <returns></returns>
        public object ApplyStatisticsOption(int eIndex, AnalyzeOption opt, StatisticsOption sopt)
        {
            return null;
        }

        /*/// <summary>
        /// 
        /// </summary>
        /// <param name="opt"></param>
        /// <param name="cOpt"></param>
        /// <returns></returns>
        /// Does ResearchResult contain that option???
        public SortedDictionary<double, double> GetDistributionResult(AnalyzeOption opt, CalculationOption cOpt)
        {
            if (!distributedOptionsResult.Keys.Contains(opt) ||
                cOpt != distributedOptionsResult[opt].Key)
                CalculateAndSetDistributionOption(opt, cOpt);
            return distributedOptionsResult[opt].Value;
        }

        private void CalculateAndSetGlobalOption(AnalyzeOption opt)
        {
            double value = 0;
            // calculate
            globalOptionsResult.Add(opt, value);
        }

        private void CalculateAndSetDistributionOption(AnalyzeOption opt, CalculationOption cOpt)
        {
            SortedDictionary<double, double> res = new SortedDictionary<double, double>();

            AnalyzeOptionInfo[] info = (AnalyzeOptionInfo[])opt.GetType().GetField(opt.ToString()).GetCustomAttributes(typeof(AnalyzeOptionInfo), false);
            Type t = info[0].EnsembleResultType;
            if (t.Equals(typeof(SortedDictionary<UInt32, Double>)))
            {
                SortedDictionary<UInt32, Double> r = researchResult.EnsembleResults[0].Result[opt] as SortedDictionary<UInt32, Double>;
                res = ApplyThickening<UInt32>(r, cOpt);
            }
            else if (t.Equals(typeof(SortedDictionary<Double, Double>)))
            {
                SortedDictionary<Double, Double> r = researchResult.EnsembleResults[0].Result[opt] as SortedDictionary<Double, Double>;
                res = ApplyThickening<Double>(r, cOpt);
            }
            else if (t.Equals(typeof(SortedDictionary<UInt32, Double>)))
            {
                SortedDictionary<UInt32, Double> r = researchResult.EnsembleResults[0].Result[opt] as SortedDictionary<UInt32, Double>;
                res = ApplyThickening<UInt32>(r, cOpt);
            }

            if (distributedOptionsResult.Keys.Contains(opt))
                distributedOptionsResult.Add(opt, new CalculationResult(cOpt, res));
            else
                distributedOptionsResult[opt] = new CalculationResult(cOpt, res);
        }

        private SortedDictionary<double, double> ApplyThickening<T>(SortedDictionary<T, double> r, CalculationOption cOpt)
        {
            int t = cOpt.thickeningValue;
            if (cOpt.thickeningType == ThickeningType.Percent)
                t = (int)(r.Values.Count() * cOpt.thickeningValue / 100.0);

            SortedDictionary<double, double> res = new SortedDictionary<double, double>();
            double[] array = r.Values.ToArray();

            int k = 1, step = t;
            double sum = 0;
            for (int i = 0; i < array.Count(); ++i)
            {
                if (k <= t)
                {
                    sum += array[i];
                    ++k;
                }
                else
                {
                    res.Add(step, sum / t);
                    sum = array[i];
                    k = 2;
                    step += t;
                }
            }

            res.Add(array.Count(), sum / ((array.Count() % t == 0) ? t : array.Count() % t));
            return res;
        }*/
    }
}
