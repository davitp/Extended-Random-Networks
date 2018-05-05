using System.Collections.Generic;

namespace NetworkModel
{
    /// <summary>
    /// The degeneracy result
    /// </summary>
    public class DegeneracyResult
    {
        /// <summary>
        /// The degeneracy result
        /// </summary>
        /// <param name="collapseSequence">Core Collapse sequence</param>
        /// <param name="degeneracy">The degeneracy</param>
        public DegeneracyResult(SortedDictionary<int, double> collapseSequence, int degeneracy)
        {
            this.CollapseSequence = collapseSequence;
            this.Degeneracy = degeneracy;
        }

        /// <summary>
        /// Vertex coreness
        /// </summary>
        public SortedDictionary<int, double> CollapseSequence { get; private set; }

        /// <summary>
        /// The degeneracy
        /// </summary>
        public int Degeneracy { get; private set; }
    }
}