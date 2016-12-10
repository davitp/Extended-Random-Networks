using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.Enumerations;

namespace Core.Attributes
{
    /// <summary>
    /// Attribute for types derived from AbstractResearch and AbstractNetworkModel types.
    /// Options - flag, which consists all Analyze Options, which are available for current type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AvailableAnalyzeOption : Attribute
    {
        public AvailableAnalyzeOption(AnalyzeOption options)
        {
            Options = options;
        }

        public AnalyzeOption Options { get; private set; }
    }
}
