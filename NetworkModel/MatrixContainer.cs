using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.Model;

namespace NetworkModel
{
    public class MatrixContainer : AbstractContainer
    {
        private List<BitArray> graph;
        private SortedDictionary<double, double> degrees;

        public MatrixContainer()
        {
            graph = new List<BitArray>();
            degrees = new SortedDictionary<double, double>();
        }

        public override UInt32 Size
        {
            get { return (UInt32)graph.Count + 1; }
            set
            {
                int size = (int)value;
                graph.Clear();
                degrees.Clear();
                for (int i = 0; i < size - 1; ++i)
                {
                    graph.Add(new BitArray(size - i - 1));
                }
            }
        }

        public SortedDictionary<Double, Double> Degrees
        {
            get { return degrees; }
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
            bool[,] matrix = new bool[Size, Size];

            for (int i = 0; i < Size; ++i)
                for (int j = 0; j < Size; ++j)
                    matrix[i, j] = AreConnected(i, j);

            return matrix;
        }

        public override List<KeyValuePair<int, int>> GetNeighbourship()
        {
            List<KeyValuePair<int, int>> n = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < graph.Count; ++i)
                for(int j = 0; j < graph[i].Count; ++j)
                    if(graph[i][j])
                        n.Add(new KeyValuePair<int, int>(i, i + j + 1));

            return n;
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
            if (i < j)
                graph[i][j - i - 1] = true;
            else
                graph[j][i - j - 1] = true;

            AddDegreeToVertex(i);
            AddDegreeToVertex(j);
        }

        private void AddDegreeToVertex(int i)
        {
            if (degrees.ContainsKey(i))
                ++degrees[i];
            else
                degrees.Add(i, 1);
        }

        public bool AreConnected(int i, int j)
        {
            if (i == j)
                return false;
            return i < j ? graph[i][j - i - 1] : graph[j][i - j - 1];
        }

        public int GetVertexDegree(int i)
        {
            return (int)(degrees.ContainsKey(i) ? degrees[i] : 0);
        }

        public List<int> GetAdjacentEdges(int i)
        {
            List<int> r = new List<int>();
            for (int j = 0; j < graph[i].Count; ++j)
                if (graph[i][j])
                    r.Add(i + j + 1);
            return r;
        }

        public int CalculateNumberOfEdges()
        {
            double r = 0;
            foreach (KeyValuePair<Double, Double> d in degrees)
                r += d.Value;
            return (int)(r / 2);
        }

        private void SetDataToDictionary(int index, ArrayList neighbourshipOfIVertex)
        {
            for (int j = 0; j < Size; j++)
                if ((bool)neighbourshipOfIVertex[j] == true && index != j)
                    AddConnection(index, j);
        }
    }
}
