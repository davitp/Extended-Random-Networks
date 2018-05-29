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
        /// <param name="minDegree">The minimum degree</param>
        /// <param name="degeneracy">The degeneracy</param>
        /// <param name="descriptors">The descriptors</param>
        public DegeneracyResult(SortedDictionary<int, double> collapseSequence, int minDegree, int degeneracy, Dictionary<int, CoreDescriptor> descriptors)
        {
            this.MinDegree = minDegree;
            this.CoreDescritpros = descriptors;
            this.CollapseSequence = collapseSequence;
            this.Degeneracy = degeneracy;
        }

        /// <summary>
        /// Vertex coreness
        /// </summary>
        public SortedDictionary<int, double> CollapseSequence { get; private set; }

        /// <summary>
        /// Minimum degree
        /// </summary>
        public int MinDegree { get; private set; }

        /// <summary>
        /// The degeneracy
        /// </summary>
        public int Degeneracy { get; private set; }

        public Dictionary<int, CoreDescriptor> CoreDescritpros { get; private set; }
    }
}