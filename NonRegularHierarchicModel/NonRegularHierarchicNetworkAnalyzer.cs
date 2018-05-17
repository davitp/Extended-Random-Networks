using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

using Core;
using Core.Model;
using NetworkModel;
using NetworkModel.HierarchicEngine;
using QuickGraph;

namespace NonRegularHierarchicModel
{
    /// <summary>
    /// Implementation of non regularly branching block-hierarchic network's analyzer.
    /// </summary>
    class NonRegularHierarchicNetworkAnalyzer : AbstractNetworkAnalyzer
    {
        /// <summary>
        /// Container with network of specified model (non regular block-hierarchic).
        /// </summary>
        private NonRegularHierarchicNetworkContainer container;

        public NonRegularHierarchicNetworkAnalyzer(AbstractNetwork n) : base(n) { }

        public override INetworkContainer Container
        {
            get { return container; }
            set { container = (NonRegularHierarchicNetworkContainer)value; }
        }

        protected override Double CalculateEdgesCountOfNetwork()
        {
            // TODO maybe non recursive calculation is available?
            return (Double)container.CalculateNumberOfEdges(0, 0);
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
            return container.CalculateNumberOfEdges(0, 0) * 2 / (Double)container.Size;
        }

        protected override Double CalculateAverageClusteringCoefficient()
        {
            double cycles3 = Count3Cycle(0, 0)[0], sum = 0, degree = 0;
            for (int i = 0; i < container.Size; ++i)
            {
                degree = VertexDegree(i, 0);
                sum += degree * (degree - 1);
            }

            return 6 * cycles3 / sum;
        }

        protected override Double CalculateCycles3()
        {
            return (long)Count3Cycle(0, 0)[0];
        }

        protected override Double CalculateCycles4()
        {
            return (long)Count4Cycle(0, 0)[0];
        }


        protected UndirectedGraph<int, Edge<int>> ToQuickGraph()
        {
            if (this.quickGraph != null) return this.quickGraph;

            var g = new UndirectedGraph<int, Edge<int>>();

            for (var i = 0; i < this.container.Size; ++i)
            {
                g.AddVertex(i);
            }

            foreach (var kv in this.container.GetNeighbourship())
            {
                if (!g.ContainsEdge(kv.Key, kv.Value))
                {
                    g.AddEdge(new Edge<int>(kv.Key, kv.Value));
                }

            }

            this.quickGraph = g;
            return g;
        }


        protected override double CalculateDegeneracy()
        {
            if (this.degeneracy == null)
            {
                this.degeneracy = this.ToQuickGraph().CoreDecomposition();
            }
            return degeneracy.Degeneracy;
        }

        protected override List<double> CalculateCoreCollapseSequence()
        {

            if (this.degeneracy == null)
            {
                this.degeneracy = this.ToQuickGraph().CoreDecomposition();
            }
            return degeneracy.CollapseSequence.Values.ToList();
        }

        protected override SortedDictionary<Double, Double> CalculateDegreeDistribution()
        {
            return DegreeDistributionInCluster(0, 0);
        }

        protected override SortedDictionary<Double, Double> CalculateClusteringCoefficientDistribution()
        {
            SortedDictionary<Double, Double> result = new SortedDictionary<Double, Double>();

            for (int i = 0; i < container.Size; ++i)
            {
                double dresult = Math.Round(ClusterringCoefficientOfVertex(i), 3);
                if (result.Keys.Contains(dresult))
                    ++result[dresult];
                else
                    result.Add(dresult, 1);
            }

            return result;
        }

        // TODO optimize
        protected override SortedDictionary<Double, Double> CalculateClusteringCoefficientPerVertex()
        {
            SortedDictionary<Double, Double> result = new SortedDictionary<Double, Double>();

            for (int i = 0; i < container.Size; ++i)
                result.Add(i, Math.Round(ClusterringCoefficientOfVertex(i), 3));

            return result;
        }

        protected override SortedDictionary<Double, Double> CalculateConnectedComponentDistribution()
        {
            return ConnectedSubgraphsInCluster(0, 0);
        }

        protected override SortedDictionary<Double, Double> CalculateDistanceDistribution()
        {
            if (!calledPaths)
                CountEssentialOptions();

            return distanceDistribution;
        }

        protected override SortedDictionary<Double, Double> CalculateTriangleByVertexDistribution()
        {
            SortedDictionary<Double, Double> result = new SortedDictionary<Double, Double>();
            for (int i = 0; i < container.Size; ++i)
            {
                Double triangleCountOfVertex = Count3CycleOfVertex(i, 0)[0];
                if (result.Keys.Contains(triangleCountOfVertex))
                    ++result[triangleCountOfVertex];
                else
                    result.Add(triangleCountOfVertex, 1);
            }

            return result;
        }

        #region Utilities

        bool calledPaths = false;
        private Double averagePathLength;
        private uint diameter;
        private SortedDictionary<Double, Double> distanceDistribution =
            new SortedDictionary<Double, Double>();

        private UndirectedGraph<int, Edge<int>> quickGraph;
        private DegeneracyResult degeneracy;

        /// <summary>
        /// A method that is used to count distance distribution, average path length and diameter.
        /// </summary>
        private void CountEssentialOptions()
        {
            double avg = 0;
            uint d = 0, uWay = 0;
            int countOfWays = 0;

            for (int i = 0; i < container.Size; ++i)
            {
                for (int j = i + 1; j < container.Size; ++j)
                {
                    int way = container.CalculateMinimalPathLength(i, j);
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
                    ++countOfWays;
                }
            }

            averagePathLength = avg / countOfWays;
            diameter = d;
            calledPaths = true;
        }

        /// <summary>
        /// Calculates the distribution of degrees of vertices that belong to the given cluster.
        /// </summary>
        /// <param name="numberNode">The index of cluster.</param>
        /// <param name="currentLevel">The level of cluster.</param>
        /// <returns>Distribution of degrees of vertices that belong to the given cluster.</returns>
        private SortedDictionary<Double, Double> DegreeDistributionInCluster(int numberNode, 
            int currentLevel)
        {
            if (currentLevel == container.Level)
            {
                SortedDictionary<Double, Double> returned = new SortedDictionary<Double, Double>();
                returned[0] = 1;
                return returned;
            }
            else
            {
                BitArray node = container.TreeNode(currentLevel, numberNode);

                SortedDictionary<Double, Double> arraysReturned = new SortedDictionary<Double, Double>();
                SortedDictionary<Double, Double> array = new SortedDictionary<Double, Double>();
                int branchSize = (int)container.Branches[currentLevel][numberNode];
                int branchStartPnt = container.FindBranches(currentLevel, numberNode);

                for (int i = 0; i < branchSize; ++i)
                {
                    array = DegreeDistributionInCluster(branchStartPnt + i, currentLevel + 1);
                    int countAjacentsNodes = container.CountConnectedBlocks(node, branchSize, i);
                    foreach (KeyValuePair<Double, Double> kvt in array)
                    {
                        Double key = kvt.Key;
                        for (int j = 0; j < branchSize; ++j)
                        {
                            int counter = 0;
                            if (container.AreConnectedTwoBlocks(node, branchSize, i, j))
                            {
                                key += container.CountLeaves(currentLevel + 1, 
                                    branchStartPnt + j);
                                ++counter;
                            }
                            if (counter == countAjacentsNodes)
                            {
                                break;
                            }
                        }

                        if (arraysReturned.ContainsKey(key))
                        {
                            arraysReturned[key] += kvt.Value;
                        }
                        else
                        {
                            arraysReturned.Add(key, kvt.Value);
                        }
                    }
                }

                return arraysReturned;
            }
        }

        /// <summary>
        /// Calculates the number of cycles of 3 order in the given cluster.
        /// </summary>
        /// <param name="level">The level of cluster.</param>
        /// <param name="numberNode">The index of cluster.</param>
        /// <returns>Number of cycles of 3 order in the given cluster.</returns>
        private SortedDictionary<Double, Double> Count3Cycle(int level, int numberNode)
        {
            SortedDictionary<Double, Double> retArray = new SortedDictionary<Double, Double>();
            retArray[0] = 0; // count cycles
            retArray[1] = 0; // count edges
            retArray[2] = 0; // count leaves

            if (level == container.Level)
            {
                retArray[2] = 1;
                return retArray;
            }
            else
            {
                uint branchSize = container.Branches[level][numberNode];
                int branchStart = container.FindBranches(level, numberNode);
                BitArray node = container.TreeNode(level, numberNode);
                double[] countEdge = new double[branchSize];
                double[] countLeaves = new double[branchSize];

                for (int i = 0; i < branchSize; ++i)
                {
                    SortedDictionary<Double, Double> arr = new SortedDictionary<Double, Double>();
                    arr = Count3Cycle(level + 1, i + branchStart);
                    countEdge[i] = arr[1];
                    countLeaves[i] = arr[2];
                    retArray[0] += arr[0];
                    retArray[1] += arr[1];
                    retArray[2] += arr[2];
                }

                for (int i = 0; i < branchSize; ++i)
                {
                    for (int j = i + 1; j < branchSize; ++j)
                    {
                        if (container.AreConnectedTwoBlocks(node, (int)branchSize, i, j))
                        {
                            retArray[0] += countLeaves[i] * countEdge[j] +
                                countLeaves[j] * countEdge[i];

                            retArray[1] += countLeaves[i] * countLeaves[j];

                            for (int k = j + 1; k < branchSize; ++k)
                            {
                                if (container.AreConnectedTwoBlocks(node, (int)branchSize, i, k)
                                    && container.AreConnectedTwoBlocks(node, (int)branchSize, j, k))
                                {
                                    retArray[0] += countLeaves[i] * countLeaves[j] * countLeaves[k];
                                }
                            }
                        }
                    }
                }
                return retArray;
            }
        }

        /// <summary>
        /// Calculates the number of cycles of 4 order in the given cluster.
        /// </summary>
        /// <param name="level">The level of cluster.</param>
        /// <param name="numberNode">The index of cluster.</param>
        /// <returns>Number of cycles of 4 order in the given cluster.</returns>
        private SortedDictionary<Double, Double> Count4Cycle(int level, int nodeNumber)
        {
            SortedDictionary<Double, Double> retArray = new SortedDictionary<Double, Double>();
            retArray[0] = 0; // число циклов порядка 4
            retArray[1] = 0; // число путей длиной 1 (ребер)
            retArray[2] = 0; // число путей длиной 2
            retArray[3] = 0; // count leaves

            if (level == container.Level)
            {
                retArray[3] = 1;
                return retArray;
            }
            else
            {
                SortedDictionary<Double, Double> array = new SortedDictionary<Double, Double>();

                int branchSize = (int)container.Branches[level][nodeNumber];
                int branchStart = container.FindBranches(level, nodeNumber);
                BitArray node = container.TreeNode(level, nodeNumber);

                double[] countEdge = new double[branchSize];
                double[] countLeaves = new double[branchSize];
                double[] countDoubleEdge = new double[branchSize];

                for (int i = 0; i < branchSize; ++i)
                {
                    array = Count4Cycle(level + 1, i + branchStart);
                    retArray[0] += array[0];
                    retArray[1] += array[1];
                    retArray[2] += array[2];
                    retArray[3] += array[3];
                    countEdge[i] = array[1];
                    countDoubleEdge[i] = array[2];
                    countLeaves[i] = array[3];
                }

                for (int i = 0; i < branchSize; ++i)
                {
                    for (int j = i + 1; j < branchSize; ++j)
                    {
                        if (container.AreConnectedTwoBlocks(node, branchSize, i, j))
                        {
                            retArray[0] += countDoubleEdge[i] * countLeaves[j] 
                                + countDoubleEdge[j] * countLeaves[i];
                            retArray[0] += 2 * countEdge[i] * countEdge[j];

                            retArray[0] += countLeaves[i] * (countLeaves[i] - 1) *
                                countLeaves[j] * (countLeaves[j] - 1) / 4;

                            retArray[1] += countLeaves[i] * countLeaves[j];

                            retArray[2] += 2 * (countEdge[i] * countLeaves[j] +
                                countEdge[j] * countLeaves[i]);
                            retArray[2] += (countLeaves[i] * (countLeaves[i] - 1) * countLeaves[j] +
                                countLeaves[j] * (countLeaves[j] - 1) * countLeaves[i]) / 2;
                        }

                        for (int k = j + 1; k < branchSize; ++k)
                        {
                            int countDouble = 0;
                            if (container.AreConnectedTwoBlocks(node, branchSize, i, j) &&
                                container.AreConnectedTwoBlocks(node, branchSize, j, k) &&
                                container.AreConnectedTwoBlocks(node, branchSize, i, k))
                            {
                                retArray[0] += 2 * (countEdge[i] * countLeaves[j] * countLeaves[k] +
                                    countEdge[j] * countLeaves[i] * countLeaves[k] +
                                    countEdge[k] * countLeaves[i] * countLeaves[j]);
                            }

                            if (container.AreConnectedTwoBlocks(node, branchSize, i, j) &&
                                container.AreConnectedTwoBlocks(node, branchSize, j, k))
                            {
                                retArray[0] += countLeaves[i] * countLeaves[k] * 
                                    (countLeaves[j] * (countLeaves[j] - 1) / 2);

                                ++countDouble;
                            }

                            if (container.AreConnectedTwoBlocks(node, branchSize, i, k) &&
                                container.AreConnectedTwoBlocks(node, branchSize, j, k))
                            {
                                retArray[0] += countLeaves[i] * countLeaves[j] *
                                    (countLeaves[k] * (countLeaves[k] - 1) / 2);

                                ++countDouble;
                            }

                            if (container.AreConnectedTwoBlocks(node, branchSize, i, j) &&
                                container.AreConnectedTwoBlocks(node, branchSize, i, k))
                            {
                                retArray[0] += countLeaves[j] * countLeaves[k] *
                                    (countLeaves[i] * (countLeaves[i] - 1) / 2);

                                ++countDouble;                                
                            }

                            retArray[2] += countDouble * countLeaves[i] * countLeaves[j] * countLeaves[k];

                            for (int l = k + 1; l < branchSize; ++l)
                            {
                                int count4clusters = 0;
                                if (container.AreConnectedTwoBlocks(node, branchSize, i, j) &&
                                    container.AreConnectedTwoBlocks(node, branchSize, j, k) &&
                                    container.AreConnectedTwoBlocks(node, branchSize, k, l) &&
                                    container.AreConnectedTwoBlocks(node, branchSize, i, l))
                                {
                                    ++count4clusters;
                                }
                                if (container.AreConnectedTwoBlocks(node, branchSize, i, j) &&
                                    container.AreConnectedTwoBlocks(node, branchSize, j, l) &&
                                    container.AreConnectedTwoBlocks(node, branchSize, k, l) &&
                                    container.AreConnectedTwoBlocks(node, branchSize, i, k))
                                {
                                    ++count4clusters;
                                }
                                if (container.AreConnectedTwoBlocks(node, branchSize, i, l) &&
                                    container.AreConnectedTwoBlocks(node, branchSize, j, l) &&
                                    container.AreConnectedTwoBlocks(node, branchSize, j, k) &&
                                    container.AreConnectedTwoBlocks(node, branchSize, i, k))
                                {
                                    ++count4clusters;
                                }
                                retArray[0] += count4clusters * countLeaves[i] * countLeaves[j] * countLeaves[k] *
                                        countLeaves[l];
                            }
                        }
                    }
                }

                return retArray;
            }
        }

        /// <summary>
        /// Calculates the clusterring coefficient of the given vertex.
        /// </summary>
        /// <param name="vertexNumber">The index of vertex.</param>
        /// <returns>Clusterring coefficient.</returns>
        private Double ClusterringCoefficientOfVertex(int vertexNumber)
        {
            SortedDictionary<Double, Double> result = Count3CycleOfVertex(vertexNumber, 0);
            double count3CyclesOfVertex = result[0];
            double degree = result[1];
            if (degree == 0 || degree == 1)
                return 0;
            else
                return (2 * count3CyclesOfVertex) / (degree * (degree - 1));
        }

        /// <summary>
        /// Calculates the number of triangles of that contain the given vertex on a cluster of given level.
        /// </summary>
        /// <param name="vertexNumber">The index of vertex.</param>
        /// <param name="level">The level of cluster.</param>
        /// <returns>Number of triangles.</returns>
        private SortedDictionary<Double, Double> Count3CycleOfVertex(int vertexNumber, int level)
        {
            SortedDictionary<Double, Double> result = new SortedDictionary<Double, Double>();
            result[0] = 0;  // число циклов 3 прикрепленных к данному узлу
            result[1] = 0;  // степень узла в данном подграфе
            result[2] = 0;  // индекс узла в данном подграфе

            if (level == container.Level)
            {
                result[2] = vertexNumber;
                return result;
            }
            else
            {
                SortedDictionary<Double, Double> previousResult = Count3CycleOfVertex(vertexNumber, level + 1);
                int vertexIndex = (int)previousResult[2];
                int numberNode = container.TreeIndexStep(vertexIndex, level);
                int branchStart = container.FindBranches(level, numberNode);
                int branchSize = (int)container.Branches[level][numberNode];
                int currentVertexIndex = vertexIndex - branchStart;
                BitArray node = container.TreeNode(level, numberNode);

                result[0] += previousResult[0];
                result[1] += previousResult[1];
                result[2] = numberNode;
                double degree = previousResult[1];
                for (int i = 0; i < branchSize; ++i)
                {
                    if (container.AreConnectedTwoBlocks(node, branchSize, currentVertexIndex, i))
                    {
                        result[1] += container.CountLeaves(level + 1, i + branchStart);
                        result[0] += container.CalculateNumberOfEdges(level + 1, i + branchStart);
                        result[0] += container.CountLeaves(level + 1, i + branchStart) * degree;
                        for (int j = i + 1; j < branchSize; ++j)
                        {
                            if (container.AreConnectedTwoBlocks(node, branchSize, i, j) &&
                                container.AreConnectedTwoBlocks(node, branchSize, j, currentVertexIndex))
                            {
                                result[0] += container.CountLeaves(level + 1, i + branchStart) *
                                    container.CountLeaves(level + 1, j + branchStart);
                            }
                        }
                    }
                } 

                return result;
            }
        }

        /// <summary>
        /// Calculates the degree of the given vertex on a cluster of the given level.
        /// </summary>
        /// <param name="vertexNumber">The index of vertex.</param>
        /// <param name="level">The level of cluster.</param>
        /// <returns>Degree of the vertex.</returns>
        private Double VertexDegree(int vertexNumber, int level)
        {
            if (level == container.Level)
            {
                return 0;
            }
            else
            {
                double result = 0;
                int nodeNumber = container.TreeIndex(vertexNumber, level);
                int branchStart = container.FindBranches(level, nodeNumber);
                int branchSize = (int)container.Branches[level][nodeNumber];
                int vertexIndex = container.TreeIndex(vertexNumber, level + 1) - branchStart;
                BitArray node = container.TreeNode(level, nodeNumber);
                for (int i = 0; i < branchSize; ++i)
                {
                    if (i != vertexIndex)
                    {
                        if (container.AreConnectedTwoBlocks(node, branchSize, i, vertexIndex))
                        {
                            result += container.CountLeaves(level + 1, i + branchStart);
                        }
                    }
                }

                return result + VertexDegree(vertexNumber, level + 1);
            }
        }

        public int bIndex { get; set; }

        /// <summary>
        /// Retrives the connected subgraphs of the given cluster.
        /// </summary>
        /// <param name="level">The level of cluster.</param>
        /// <param name="numberNode">The index of cluster.</param>
        /// <returns>Connected components.</returns>
        private SortedDictionary<Double, Double> ConnectedSubgraphsInCluster(int level, int numberNode)
        {
            SortedDictionary<Double, Double> retArray = new SortedDictionary<Double, Double>();

            if (level == container.Level)
            {
                retArray[1] = 1;
                return retArray;
            }

            int branchSize = (int)container.Branches[level][numberNode];
            int branchStart = container.FindBranches(level, numberNode);
            BitArray node = container.TreeNode(level, numberNode);

            bool haveOne = false;
            for (int i = 0; i < branchSize; ++i)
            {
                if (container.CountConnectedBlocks(node, branchSize, i) == 0)
                {
                    SortedDictionary<Double, Double> array = ConnectedSubgraphsInCluster(level + 1, 
                        branchStart + i);

                    foreach (KeyValuePair<Double, Double> kvt in array)
                    {
                        if (retArray.Keys.Contains(kvt.Key))
                            retArray[kvt.Key] += kvt.Value;
                        else
                            retArray.Add(kvt.Key, kvt.Value);
                    }
                }
                else
                {
                    haveOne = true;
                }
            }

            if (haveOne)
            {
                EngineForConnectedComp engForConnectedComponent = new EngineForConnectedComp();
                Dictionary<int, ArrayList> nodeMadrixList = container.NodeAdjacencyLists(node, branchSize);

                Dictionary<int, ArrayList> connComp = engForConnectedComponent.GetConnSGraph(nodeMadrixList, branchSize);
                Double countConnElements = 0;
                for (int i = 0; i < connComp.Count; ++i)
                {
                    if (connComp[i].Count > 1)
                    {
                        countConnElements = 0;
                        for (int j = 0; j < connComp[i].Count; ++j)
                        {
                            countConnElements += container.CountLeaves(level + 1, 
                                branchStart + (int)connComp[i][j]);
                        }

                        if (retArray.Keys.Contains(countConnElements))
                        {
                            retArray[countConnElements] += 1;
                        }
                        else
                        {
                            retArray.Add(countConnElements, 1);
                        }
                    }
                }
            }

            return retArray;
        }

        #endregion
    }
}
