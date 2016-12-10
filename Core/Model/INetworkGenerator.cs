using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.Enumerations;
using Core.Utility;

namespace Core.Model
{
    /// <summary>
    /// Interface for random network generator.
    /// </summary>
    public interface INetworkGenerator
    {
        /// <summary>
        /// Generated network's container.
        /// </summary>
        INetworkContainer Container { get; set; }

        /// <summary>
        /// Generates random network with specified generation parameters.
        /// </summary>
        /// <param name="genParam">Generation parameter values.</param>
        void RandomGeneration(Dictionary<GenerationParameter, object> genParam);

        /// <summary>
        /// Generates network from adjacency matrix.
        /// </summary>
        /// <param name="matrix"></param>
        void StaticGeneration(MatrixInfoToRead matrixInfo);
    }
}
