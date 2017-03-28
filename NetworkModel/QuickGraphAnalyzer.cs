using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.ShortestPath;

using Core;
using Core.Model;

namespace NetworkModel
{
    class CommonInformation
    {
        public CommonInformation()
        {
            Edges = new List<double>();
        }

        public List<Double> Edges;
    }

    class PathInformation
    {
        public PathInformation()
        {
            AveragePathLength = 0;
            Diameter = 0;
            DistanceDistribution = new SortedDictionary<double, double>();
        }

        public Double AveragePathLength;
        public Double Diameter;
        public SortedDictionary<Double, Double> DistanceDistribution;
    }

    class ClusteringCoefficientInformation
    {
        public ClusteringCoefficientInformation()
        {
            Coefficients = new SortedDictionary<double, double>();
        }

        public SortedDictionary<Double, Double> Coefficients;
    }

    /// <summary>
    /// 
    /// </summary>
    public class QuickGraphAnalyzer : AbstractNetworkAnalyzer
    {
        private MatrixContainer container;

        private CommonInformation commonInformation;
        private PathInformation pathInformation;
        private ClusteringCoefficientInformation coefficientInformation;

        public QuickGraphAnalyzer(AbstractNetwork n) : base(n)
        {
            commonInformation = null;
            pathInformation = null;
            coefficientInformation = null;
        }

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
            if (pathInformation == null)
                CalculatePathInformation();

            Debug.Assert(pathInformation != null);
            return pathInformation.AveragePathLength;
        }

        protected override Double CalculateDiameter()
        {
            if (pathInformation == null)
                CalculatePathInformation();

            Debug.Assert(pathInformation != null);
            return pathInformation.Diameter;
        }

        protected override Double CalculateAverageDegree()
        {
            return AverageDegree();
        }

        protected override Double CalculateAverageClusteringCoefficient()
        {
            double cycles3 = CalculateCycles3(), sum = 0, degree = 0;
            for (int i = 0; i < container.Size; ++i)
            {
                degree = container.GetVertexDegree(i);
                sum += degree * (degree - 1);
            }

            return 6 * cycles3 / sum;
        }

        protected override Double CalculateCycles3()
        {
            if (commonInformation == null)
                CalculatePathInformation();

            double cycles3 = 0;
            for (int i = 0; i < container.Size; ++i)
            {
                if (commonInformation.Edges[i] != -1)
                    cycles3 += commonInformation.Edges[i];
            }

            if (cycles3 > 0 && cycles3 < 3)
                cycles3 = 1;
            else
                cycles3 /= 3;

            return cycles3;
        }

        protected override Double CalculateCycles4()
        {
            long result = 0;
            for (int i = 0; i < container.Size; ++i)
                result += CalculateCycles4ForVertex(i);

            return result / 4;
        }

        protected override SortedDictionary<Double, Double> CalculateDegreeDistribution()
        {
            return DegreeDistribution();
        }

        protected override SortedDictionary<Double, Double> CalculateClusteringCoefficientDistribution()
        {
            if (coefficientInformation == null)
                CalculateCoefficientInformation();

            SortedDictionary<Double, Double> result = new SortedDictionary<Double, Double>();

            for (int i = 0; i < container.Size; ++i)
            {
                double r = coefficientInformation.Coefficients[(uint)i];
                if (result.Keys.Contains(r))
                    result[coefficientInformation.Coefficients[i]]++;
                else
                    result[coefficientInformation.Coefficients[i]] = 1;
            }

            return result;
        }

        protected override SortedDictionary<Double, Double> CalculateClusteringCoefficientPerVertex()
        {
            if (coefficientInformation == null)
                CalculateCoefficientInformation();

            return coefficientInformation.Coefficients;
        }

        protected override SortedDictionary<Double, Double> CalculateDistanceDistribution()
        {
            if (pathInformation == null)
                CalculatePathInformation();

            Debug.Assert(pathInformation != null);
            return pathInformation.DistanceDistribution;
        }

        protected override SortedDictionary<Double, Double> CalculateTriangleByVertexDistribution()
        {
            if (pathInformation == null)
                CalculatePathInformation();

            SortedDictionary<Double, Double> result = new SortedDictionary<Double, Double>();
            for (int i = 0; i < container.Size; ++i)
            {
                Double tmp = commonInformation.Edges[i];
                if (result.ContainsKey(tmp))
                    ++result[tmp];
                else
                    result.Add(tmp, 1);
            }

            return result;
        }

        #region Utilities

        private double AverageDegree()
        {
            return container.CalculateNumberOfEdges() * 2 / (double)container.Size;
        }

        private SortedDictionary<Double, Double> DegreeDistribution()
        {
            SortedDictionary<Double, Double> degreeDistribution = new SortedDictionary<Double, Double>();
            for (uint i = 0; i < container.Size; ++i)
                degreeDistribution[i] = new Double();

            for (int i = 0; i < container.Size; ++i)
            {
                int degreeOfVertexI = container.GetVertexDegree(i);
                ++degreeDistribution[degreeOfVertexI];
            }

            for (uint i = 0; i < container.Size; ++i)
                if (degreeDistribution[i] == 0)
                    degreeDistribution.Remove(i);

            return degreeDistribution;
        }

        #region BFS

        private class Node
        {
            public int ancestor = -1;
            public int lenght = -1;
        }

        void BreadthFirstSearch(int i, Node[] nodes)
        {
            Debug.Assert(commonInformation != null);
            Debug.Assert(commonInformation.Edges.Count == container.Size);

            nodes[i].lenght = 0;
            nodes[i].ancestor = 0;

            bool b = true;
            if (commonInformation.Edges[i] == -1)
                commonInformation.Edges[i] = 0;
            else
                b = false;

            Queue<int> q = new Queue<int>();
            q.Enqueue(i);
            int u = 0;
            while (q.Count != 0)
            {
                u = q.Dequeue();
                foreach (int e in container.GetAdjacentEdges(u))
                {
                    if(nodes[e].lenght == -1)
                    {
                        nodes[e].lenght = nodes[u].lenght + 1;
                        nodes[e].ancestor = u;
                        q.Enqueue(e);
                    }
                    else if (nodes[u].lenght == 1 && nodes[e].lenght == 1 && b)
                        ++commonInformation.Edges[i];
                }
            }
            if (b)
                commonInformation.Edges[i] /= 2;
        }
        
        double MinimumWay(int i, int j)
        {
            if (i == j)
                return 0;

            Node[] nodes = new Node[container.Size];
            for (int k = 0; k < container.Size; ++k)
                nodes[k] = new Node();

            BreadthFirstSearch(i, nodes);

            return nodes[j].lenght;
        } 

        #endregion

        private void InitializeCommonInformation()
        {
            Debug.Assert(commonInformation == null);
            commonInformation = new CommonInformation();
            for (int i = 0; i < container.Size; ++i)
                commonInformation.Edges.Add(-1);
        }

        private void CalculateCommonInformation()
        {
            Debug.Assert(commonInformation != null);
            Debug.Assert(commonInformation.Edges.Count == container.Size);

            Node[] nodes = new Node[container.Size];
            for (int i = 0; i < container.Size; ++i)
                nodes[i] = new Node();

            BreadthFirstSearch((int)container.Size - 1, nodes);
        }

        private void CalculatePathInformation()
        {
            Debug.Assert(pathInformation == null);
            if (commonInformation == null)
                InitializeCommonInformation();
            pathInformation = new PathInformation();
            
            int k = 0;
            double current = 0;
            for (int i = 0; i < container.Size; ++i)
            {
                for (int j = i + 1; j < container.Size; ++j)
                {
                    double way = MinimumWay(i, j);
                    if (way == -1)
                        continue;
                    else
                        current = way;

                    pathInformation.AveragePathLength += current;

                    if (current > pathInformation.Diameter)
                        pathInformation.Diameter = current;

                    if (pathInformation.DistanceDistribution.ContainsKey(current))
                        ++pathInformation.DistanceDistribution[current];
                    else
                        pathInformation.DistanceDistribution.Add(current, 1);

                    ++k;
                }
            }
            pathInformation.AveragePathLength /= k;
            CalculateCommonInformation();
        }

        private void CalculateCoefficientInformation()
        {
            if (pathInformation == null)
                CalculatePathInformation();

            Debug.Assert(coefficientInformation == null);
            coefficientInformation = new ClusteringCoefficientInformation();
            
            int e = 0, n = 0;
            double current = 0;
            for (int i = 0; i < container.Size; ++i)
            {
                n = container.GetVertexDegree(i);
                if (n != 0)
                {
                    e = (n == 1) ? 1 : n * (n - 1) / 2;
                    current = (commonInformation.Edges[i]) / e;
                    coefficientInformation.Coefficients.Add(i, Math.Round(current, 3));
                }
                else
                    coefficientInformation.Coefficients.Add(i, 0);
            }
        }

        /*private void CalculatePathInformation()
        {
            Debug.Assert(pathInformation == null);
            pathInformation = new PathInformation();
            
            int k = 0;

            Func<UndirectedEdge<int>, double> edgeWeights = e => 1;
            TryFunc<int, IEnumerable<UndirectedEdge<int>>> paths = null;
            IEnumerable<UndirectedEdge<int>> path;
            for (int i = 0; i < container.Size; ++i)
            {
                paths = AlgorithmExtensions.ShortestPathsDijkstra<int, UndirectedEdge<int>>(container.GetContainer(),
                    edgeWeights, i);

                for (int j = i + 1; j < container.Size; ++j)
                {
                    Double current = 0;
                    if (paths(j, out path))
                        current = path.Count();
                    else
                        continue;

                    pathInformation.AveragePathLength += current;

                    if (current > pathInformation.Diameter)
                        pathInformation.Diameter = current;

                    if (pathInformation.DistanceDistribution.ContainsKey(current))
                        ++pathInformation.DistanceDistribution[current];
                    else
                        pathInformation.DistanceDistribution.Add(current, 1);

                    ++k;
                }
            }
            pathInformation.AveragePathLength /= k;
        }*/

        private long CalculateCycles4ForVertex(int v)
        {
            long result = 0;
            /*foreach (int e in container.GetAdjacentEdges(v))
            {
                foreach(int e1 in container.GetAdjacentEdges(e))
                {
                    if (v != e1)
                    {
                        foreach (int e2 in container.GetAdjacentEdges(e1))
                        {
                            if (container.AreConnected(e2, v) &&
                                e2 != e1)// && e != e)
                            {
                                ++result;
                            }
                        }
                    }
                }
            }*/

            return result / 2;
        }

        #endregion
    }
}