using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

using Core.Enumerations;

namespace Core.Model
{
    /// <summary>
    /// Interface for random network analyzer.
    /// </summary>
    public interface INetworkAnalyzer
    {
        /// <summary>
        /// Generated network's container.
        /// </summary>
        INetworkContainer Container { get; set; }

        /// <summary>
        /// Calculates the count of edges of network.
        /// </summary>
        /// <returns>Count of edges.</returns>
        Double CalculateEdgesCount();

        /// <summary>
        /// Calculates specified analyze option.
        /// </summary>
        /// <param name="option">Analyze option</param>
        /// <returns>Calculated value.</returns>
        Object CalculateOption(AnalyzeOption option);
    }
}
