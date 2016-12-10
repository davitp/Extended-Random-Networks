using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Attributes
{
    /// <summary>
    /// Enumeration used for inticating the logical type of statistical property.
    /// </summary>
    public enum OptionType
    {
        Global,
        ValueList,
        Distribution,
        Trajectory,
        Centrality
    }

    /// <summary>
    /// Mapping which shows the type of data for each OptionType.
    /// </summary>
    public static class OptionTypeToTypeMapping
    {
        public static Dictionary<OptionType, Type> Mapping { get; private set; }

        static OptionTypeToTypeMapping()
        {
            Mapping = new Dictionary<OptionType, Type>();

            Mapping.Add(OptionType.Global, typeof(Double));
            Mapping.Add(OptionType.ValueList, typeof(List<Double>));
            Mapping.Add(OptionType.Centrality, typeof(List<Double>));
            Mapping.Add(OptionType.Distribution, typeof(SortedDictionary<Double, Double>));
            Mapping.Add(OptionType.Trajectory, typeof(SortedDictionary<Double, Double>));
        }
    };

    /// <summary>
    /// Attribute for AnalyzeOption (enum).
    /// FullName - user-friendly name for an Analyze Option.
    /// Description - extended information about an Analyze Option.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class AnalyzeOptionInfo : Attribute
    {
        public AnalyzeOptionInfo(string fullName, 
            string description,
            OptionType optionType)
        {
            FullName = fullName;
            Description = description;
            OptionType = optionType;
        }

        public AnalyzeOptionInfo(string fullName,
            string description,
            OptionType optionType,
            string xAxisName,
            string yAxisName)
            : this(fullName, description, optionType)
        {
            XAxisName = xAxisName;
            YAxisName = yAxisName;
        }

        public string FullName { get; private set; }
        public string Description { get; private set; }
        public OptionType OptionType { get; private set; }
        public string XAxisName { get; private set; }
        public string YAxisName { get; private set; }
    }
}
