using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Model
{
    /// <summary>
    /// Interface for random network container.
    /// </summary>
    public interface INetworkContainer
    {
        /// <summary>
        /// The size of the graph (number if vertices).
        /// </summary>
        UInt32 Size { get; set; }

        // Строится граф на основе матрицы смежности.
        /// <summary>
        /// Constucts a graph on the base of a adjacency matrix.
        /// </summary>
        /// <param name="matrix">Adjacency matrix.</param>
        void SetMatrix(ArrayList matrix);

        /// <summary>
        /// Constructs adjacency matrix for the graph.
        /// </summary>
        /// <returns>Adjacency matrix.</returns>
        bool[,] GetMatrix();
        
        /// <summary>
        /// Constructs neighbourships for the graph.
        /// </summary>
        /// <returns>Neighbourship pairs.</returns>
        List<KeyValuePair<int, int>> GetNeighbourship();
    }
}
