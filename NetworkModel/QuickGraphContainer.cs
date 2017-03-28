using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuickGraph;

using Core.Model;

namespace NetworkModel
{
    /// <summary>
    /// 
    /// </summary>
    public class QuickGraphContainer : AbstractContainer 
    {
        private UndirectedGraph<int, UndirectedEdge<int>> graph;

        public QuickGraphContainer()
        {
            graph = new UndirectedGraph<int, UndirectedEdge<int>>(false);
        }

        public override UInt32 Size
        {
            get { return (UInt32)graph.VertexCount; }
            set
            {
                graph.Clear();
                for (int i = 0; i < value; ++i)
                    graph.AddVertex(i);
            }
        }

        public override void SetMatrix(ArrayList matrix)
        {
            Size = (uint)matrix.Count;
            ArrayList neighbourshipOfVertex = new ArrayList();
            for (int i = 0; i < matrix.Count; i++)
            {
                neighbourshipOfVertex = (ArrayList)matrix[i];
                SetDataToDictionary(i, neighbourshipOfVertex);
            }
        }

        public override bool[,] GetMatrix()
        {
            bool[,] matrix = new bool[graph.VertexCount, graph.VertexCount];

            for (int i = 0; i < graph.VertexCount; ++i)
                for (int j = 0; j < graph.VertexCount; ++j)
                    matrix[i, j] = AreConnected(i, j);

            return matrix;
        }

        public override List<KeyValuePair<int, int>> GetNeighbourship()
        {
            List<KeyValuePair<int, int>> n = new List<KeyValuePair<int, int>>();
            foreach(UndirectedEdge<int> e in graph.Edges)
                n.Add(new KeyValuePair<int, int>(e.Source, e.Target));

            return n;
        }

        /// <summary>
        /// Adds a new vertex with no connections.
        /// </summary>
        public void AddVertex()
        {
            graph.AddVertex(graph.VertexCount + 1);
        }

        /// <summary>
        /// Adds a connection between specified vertices.
        /// </summary>
        /// <param name="i">First vertex number.</param>
        /// <param name="j">Second vertex number.</param>
        public void AddConnection(int i, int j)
        {
            if (i == j || AreConnected(i, j))
                return;
            UndirectedEdge<int> e = (i < j) ? new UndirectedEdge<int>(i, j) : new UndirectedEdge<int>(j, i);
            graph.AddEdge(e);
        }

        public bool AreConnected(int i, int j)
        {
            if (i == j)
                return false;
            return i < j ? graph.ContainsEdge(i, j) : graph.ContainsEdge(j, i);
        }

        public int GetVertexDegree(int i)
        {
            return graph.AdjacentDegree(i);
        }

        public IEnumerable<UndirectedEdge<int>> GetAdjacentEdges(int i)
        {
            return graph.AdjacentEdges(i);
        }

        public int CalculateNumberOfEdges()
        {
            return graph.EdgeCount;
        }

        public UndirectedGraph<int, UndirectedEdge<int>> GetContainer()
        {
            return graph;
        }

        private void SetDataToDictionary(int index, ArrayList neighbourshipOfIVertex)
        {
            for (int j = 0; j < graph.VertexCount; j++)
                if ((bool)neighbourshipOfIVertex[j] == true && index != j)
                    AddConnection(index, j);
        }
    }
}
