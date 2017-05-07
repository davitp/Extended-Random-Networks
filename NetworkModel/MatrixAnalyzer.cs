using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Collections;
using System.Diagnostics;

using Core;
using Core.Exceptions;
using Core.Enumerations;
using Core.Model;
using NetworkModel.Engine.Eigenvalues;
using NetworkModel.Engine.Cycles;
using RandomNumberGeneration;

namespace NetworkModel
{
    /// <summary>
    /// Implementation of non hierarchic network's analyzer.
    /// </summary>
    public class MatrixAnalyzer : AbstractNetworkAnalyzer
    {
        private MatrixContainer container;

        public MatrixAnalyzer(AbstractNetwork n) : base(n) { }

        public override INetworkContainer Container
        {
            get { return container; }
            set { container = (MatrixContainer)value; }
        }

        protected override Double CalculateEdgesCountOfNetwork()
        {
            return (Double)container.CalculateNumberOfEdges();
        }

        protected override Double CalculateAveragePath()
        {
            if (!calledPaths)
                CountEssentialOptions();

            return averagePathLength;
        }

        protected override Double CalculateDiameter()
        {
            if (!calledPaths)
                CountEssentialOptions();

            return (Double)diameter;
        }

        protected override Double CalculateAverageDegree()
        {
            return AverageDegree();
        }

        protected override Double CalculateAverageClusteringCoefficient()
        {
            double cycles3 = (double)CalculateCycles3(), sum = 0, degree = 0;
            for (int i = 0; i < container.Size; ++i)
            {
                degree = container.GetVertexDegree(i);
                sum += degree * (degree - 1);
            }

            return 6 * cycles3 / sum;
        }

        protected override Double CalculateCycles3()
        {
            if (!calledPaths)
                CountEssentialOptions();

            double cycles3 = 0;
            for (int i = 0; i < container.Size; ++i)
            {
                if (edgesBetweenNeighbours[i] != -1)
                    cycles3 += edgesBetweenNeighbours[i];
            }

            if (cycles3 > 0 && cycles3 < 3)
                cycles3 = 1;
            else
                cycles3 /= 3;

            return cycles3;
        }

        protected override Double CalculateCycles4()
        {
            long count = 0;
            for (int i = 0; i < container.Size; i++)
                count += Get4OrderCyclesOfNode(i);

            return count / 4;
        }

        protected override SortedDictionary<Double, Double> CalculateDegreeDistribution()
        {
            return DegreeDistribution();
        }

        protected override SortedDictionary<Double, Double> CalculateClusteringCoefficientDistribution()
        {
            if (!calledCoeffs)
                CountCoefficients();

            SortedDictionary<Double, Double> m_iclusteringCoefficient =
                new SortedDictionary<Double, Double>();

            for (int i = 0; i < container.Size; ++i)
            {
                double result = coefficients[(uint)i];
                if (m_iclusteringCoefficient.Keys.Contains(result))
                    m_iclusteringCoefficient[coefficients[i]]++;
                else
                    m_iclusteringCoefficient[coefficients[i]] = 1;
            }

            return m_iclusteringCoefficient;
        }

        protected override SortedDictionary<Double, Double> CalculateClusteringCoefficientPerVertex()
        {
            if (!calledCoeffs)
                CountCoefficients();

            return coefficients;
        }

        protected override SortedDictionary<Double, Double> CalculateConnectedComponentDistribution()
        {
            var connectedSubGraphDic = new SortedDictionary<Double, Double>();
            Queue<int> q = new Queue<int>();
            var nodes = new Node[container.Size];
            for (int i = 0; i < nodes.Length; i++)
                nodes[i] = new Node();
            var list = new List<int>();

            for (int i = 0; i < container.Size; i++)
            {
                UInt32 order = 0;
                q.Enqueue(i);
                while (q.Count != 0)
                {
                    var item = q.Dequeue();
                    if (nodes[item].lenght != 2)
                    {
                        if (nodes[item].lenght == -1)
                        {
                            order++;
                        }
                        list = container.GetAdjacentEdges(item);
                        nodes[item].lenght = 2;

                        for (int j = 0; j < list.Count; j++)
                        {
                            if (nodes[list[j]].lenght == -1)
                            {
                                nodes[list[j]].lenght = 1;
                                order++;
                                q.Enqueue(list[j]);
                            }

                        }
                    }
                }

                if (order != 0)
                {
                    if (connectedSubGraphDic.ContainsKey(order))
                        connectedSubGraphDic[order]++;
                    else
                        connectedSubGraphDic.Add(order, 1);
                }
            }
            return connectedSubGraphDic;
        }

        protected override SortedDictionary<Double, Double> CalculateDistanceDistribution()
        {
            if (!calledPaths)
                CountEssentialOptions();

            return distanceDistribution;
        }

        protected override SortedDictionary<Double, Double> CalculateTriangleByVertexDistribution()
        {
            if (!calledPaths)
                CountEssentialOptions();

            var trianglesDistribution = new SortedDictionary<Double, Double>();
            for (int i = 0; i < container.Size; ++i)
            {
                var countTringle = edgesBetweenNeighbours[i];
                if (trianglesDistribution.ContainsKey(countTringle))
                {
                    trianglesDistribution[countTringle]++;
                }
                else
                {
                    trianglesDistribution.Add(countTringle, 1);
                }
            }

            return trianglesDistribution;
        }

        protected override double CalculateDr()
        {
            uint Size = container.Size;
            List<List<int>> matrix = new List<List<int>>();
            List<List<int>> lstNr = new List<List<int>>();


            for (int i = 0; i < Size; ++i)
            {
                lstNr.Add(container.GetAdjacentEdges(i));
            }


            for (int i = 0; i < Size; ++i)
            {
                List<int> temp = new List<int>();
                temp.Add(lstNr[i].Count);

                List<int> tmplst = new List<int>();
                List<int> cur = lstNr[i];
                List<int> prev = new List<int>();
                prev.Add(i);

                foreach (int k in cur)
                {
                    tmplst.Add(k);
                }

                int count = temp.Last();
                while (true)
                {
                    List<int> curlst = new List<int>();
                    foreach (int t in tmplst)
                        curlst.Add(t);

                    foreach (int p in curlst.OrderBy(x => x))
                    {
                        count += NeighbourshipCount(p, prev);
                        prev.Add(p);
                        ReFillList(p, tmplst);
                    }

                    foreach (int q in curlst)
                        tmplst.Remove(q);

                    if (count <= temp.Last())
                        break;
                    temp.Add(count);
                }

                matrix.Add(temp);
            }

            int maxC = matrix[0].Count;

            foreach (List<int> lst in matrix)
                if (lst.Count > maxC)
                    maxC = lst.Count;

            for (int i = 0; i < matrix.Count; ++i)
            {
                if (matrix[i].Count < maxC)
                {
                    int temp = matrix[i].Last();
                    while (matrix[i].Count != maxC)
                        matrix[i].Add(temp);
                }
            }

            List<double> NrAVG = new List<double>();

            foreach (List<int> i in matrix)
                NrAVG.Add(CalculateNrAvg(i));


            return CalculateNrAvg(NrAVG);
        }

        private int NeighbourshipCount(int i, List<int> lst)
        {
            List<int> temp = new List<int>();

            for (int j = 1; j < container.Size; ++j)
            {
                if (container.AreConnected(i, j) && !temp.Contains(j))
                {
                    temp.Add(j);
                }
            }

            if (lst != null)
                foreach (int item in lst)
                {
                    if (temp.Contains(item))
                    {
                        temp.Remove(item);
                    }
                }

            return temp.Count;
        }

        private void ReFillList(int i, List<int> lst)
        {
            for (int j = 0; j < container.Size; ++j)
            {
                if (i != j && container.AreConnected(i, j) && !lst.Contains(j))
                {
                    lst.Add(j);
                }
            }

        }

        /*protected override SortedDictionary<Double, Double> CalculateEigenVectorCentrality()
        {
            return base.CalculateEigenVectorCentrality();
        }*/

        #region Utilities

        private double CalculateNrAvg(List<int> lst)
        {
            if (lst.Count != 0)
            {
                double sum = 0;
                foreach (int item in lst)
                {
                    sum += item;
                }

                return sum / lst.Count;
            }

            return 0;
        }

        private double CalculateNrAvg(List<double> lst)
        {
            if (lst.Count != 0)
            {
                double sum = 0;
                foreach (int item in lst)
                {
                    sum += item;
                }

                return sum / lst.Count;
            }

            return 0;
        }

        private bool calledPaths = false;
        private double averagePathLength;
        private uint diameter;
        private SortedDictionary<Double, Double> distanceDistribution =
            new SortedDictionary<Double, Double>();
        private List<double> edgesBetweenNeighbours = new List<double>();

        private bool calledCoeffs = false;
        private SortedDictionary<Double, Double> coefficients =
            new SortedDictionary<Double, Double>();

        private bool calledEigens = false;
        private List<double> eigenValues = new List<double>();

        private class Node
        {
            public int ancestor = -1;
            public int lenght = -1;
            public int m_4Cycles = 0;
            public Node() { }
        }

        private void BFS(int i, Node[] nodes)
        {
            nodes[i].lenght = 0;
            nodes[i].ancestor = 0;
            bool b = true;
            Queue<int> q = new Queue<int>();
            q.Enqueue(i);
            int u;
            if (edgesBetweenNeighbours[i] == -1)
                edgesBetweenNeighbours[i] = 0;
            else
                b = false;

            while (q.Count != 0)
            {
                u = q.Dequeue();
                List<int> l = container.GetAdjacentEdges(u);
                for (int j = 0; j < l.Count; ++j)
                    if (nodes[l[j]].lenght == -1)
                    {
                        nodes[l[j]].lenght = nodes[u].lenght + 1;
                        nodes[l[j]].ancestor = u;
                        q.Enqueue(l[j]);
                    }
                    else
                    {
                        if (nodes[u].lenght == 1 && nodes[l[j]].lenght == 1 && b)
                        {
                            ++edgesBetweenNeighbours[i];
                        }
                    }
            }
            if (b)
                edgesBetweenNeighbours[i] /= 2;
        }

        // Выполняет подсчет сразу 3 свойств - средняя длина пути, диаметр и пути между вершинами.
        // Нужно вызвать перед получением этих свойств не изнутри.
        private void CountEssentialOptions()
        {
            if (edgesBetweenNeighbours.Count == 0)
            {
                for (int i = 0; i < container.Size; ++i)
                    edgesBetweenNeighbours.Add(-1);
            }

            double avg = 0;
            uint d = 0, uWay = 0;
            int k = 0;

            for (int i = 0; i < container.Size; ++i)
            {
                for (int j = i + 1; j < container.Size; ++j)
                {
                    int way = MinimumWay(i, j);
                    if (way == -1)
                        continue;
                    else
                        uWay = (uint)way;

                    if (distanceDistribution.ContainsKey(uWay))
                        ++distanceDistribution[uWay];
                    else
                        distanceDistribution.Add(uWay, 1);

                    if (uWay > d)
                        d = uWay;

                    avg += uWay;
                    ++k;
                }
            }

            Node[] nodes = new Node[container.Size];
            for (int t = 0; t < container.Size; ++t)
                nodes[t] = new Node();

            BFS((int)container.Size - 1, nodes);
            avg /= k;

            averagePathLength = avg;
            diameter = d;
            calledPaths = true;
        }

        // Возвращает длину минимальной пути между данными вершинами.
        private int MinimumWay(int i, int j)
        {
            if (i == j)
                return 0;

            Node[] nodes = new Node[container.Size];
            for (int k = 0; k < container.Size; ++k)
                nodes[k] = new Node();

            BFS(i, nodes);

            return nodes[j].lenght;
        }

        private void CountCoefficients()
        {
            if (!calledPaths)
                CountEssentialOptions();

            double clusteringCoefficient = 0;
            int iEdgeCountForFullness = 0, iNeighbourCount = 0;
            double iclusteringCoefficient = 0;

            for (int i = 0; i < container.Size; ++i)
            {
                iNeighbourCount = container.GetVertexDegree(i);
                if (iNeighbourCount != 0)
                {
                    iEdgeCountForFullness = (iNeighbourCount == 1) ? 1 : iNeighbourCount * (iNeighbourCount - 1) / 2;
                    iclusteringCoefficient = (edgesBetweenNeighbours[i]) / iEdgeCountForFullness;
                    coefficients.Add(i, Math.Round(iclusteringCoefficient, 3));
                    clusteringCoefficient += iclusteringCoefficient;
                }
                else
                    coefficients.Add(i, 0);
            }

            clusteringCoefficient /= container.Size;

            calledCoeffs = true;
        }

        // Возвращает число циклов 4, которые содержат данную вершину.
        private int CalculatCycles4(int i)
        {
            Node[] nodes = new Node[container.Size];
            for (int k = 0; k < container.Size; ++k)
                nodes[k] = new Node();
            int cyclesOfOrderi4 = 0;
            nodes[i].lenght = 0;
            nodes[i].ancestor = 0;
            Queue<int> q = new Queue<int>();
            q.Enqueue(i);
            int u;

            while (q.Count != 0)
            {
                u = q.Dequeue();
                List<int> l = container.GetAdjacentEdges(u);
                for (int j = 0; j < l.Count; ++j)
                    if (nodes[l[j]].lenght == -1)
                    {
                        nodes[l[j]].lenght = nodes[u].lenght + 1;
                        nodes[l[j]].ancestor = u;
                        q.Enqueue(l[j]);
                    }
                    else
                    {
                        if (nodes[u].lenght == 2 && nodes[l[j]].lenght == 1 && nodes[u].ancestor != l[j])
                        {
                            SortedList<int, int> cycles4I = new SortedList<int, int>();
                            cyclesOfOrderi4++;
                        }
                    }
            }

            return cyclesOfOrderi4;
        }

        // Возвращает распределение степеней.
        private SortedDictionary<Double, Double> DegreeDistribution()
        {
            return container.Degrees;
        }

        private long Get4OrderCyclesOfNode(int j)
        {
            /*List<int> neigboursList = container.Neighbourship[j];
            List<int> neigboursList1 = new List<int>();
            List<int> neigboursList2 = new List<int>();*/
            long count = 0;
            /*for (int i = 0; i < neigboursList.Count; i++)
            {
                neigboursList1 = container.Neighbourship[neigboursList[i]];
                for (int t = 0; t < neigboursList1.Count; t++)
                {
                    if (j != neigboursList1[t])
                    {
                        neigboursList2 = container.Neighbourship[neigboursList1[t]];
                        for (int k = 0; k < neigboursList2.Count; k++)
                            if (container.AreConnected(neigboursList2[k], j) && neigboursList2[k] != neigboursList1[t] && neigboursList2[k] != neigboursList[i])
                                count++;
                    }
                }
            }*/

            return count / 2;
        }

        /// <summary>
        /// Calculates average degree of the network.
        /// </summary>
        /// <returns>Average degree.</returns>
        public double AverageDegree()
        {
            return container.CalculateNumberOfEdges() * 2 / (double)container.Size;
        }

        #endregion
    }
}