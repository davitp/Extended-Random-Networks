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

namespace RegularHierarchicModel
{
    /// <summary>
    /// Implementation of regularly branching block-hierarchic network's analyzer.
    /// </summary>
    class RegularHierarchicNetworkAnalyzer : AbstractNetworkAnalyzer
    {
        /// <summary>
        /// Container with network of specified model (regular block-hierarchic).
        /// </summary>
        private RegularHierarchicNetworkContainer container;

        public RegularHierarchicNetworkAnalyzer(AbstractNetwork n) : base(n) { }

        public override INetworkContainer Container
        {
            get { return container; }
            set { container = (RegularHierarchicNetworkContainer)value; }
        }

        protected override Double CalculateEdgesCountOfNetwork()
        {
            // TODO maybe non recursive calculation is available?
            return container.CalculateNumberOfEdges(0, 0);
        }

        protected override Double CalculateAveragePath()
        {
            if (!calledPaths)
                CountEssentialOptions();

            return averagePathLength;

            // TODO !optimize!
            //long[] pathsInfo = GetSubgraphsPathInfo(0, 0);
            // !petq e bajanel chanaparhneri qanaki vra!
            //return 2 * (pathsInfo[0] + pathsInfo[2]) / ((double)container.Size *
            //    ((double)container.Size - 1));
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
            double cycles3 = Count3Cycle(0, 0)[0], sum = 0, degree = 0;
            for (int i = 0; i < container.Size; ++i)
            {
                degree = container.VertexDegree(i, 0);
                sum += degree * (degree - 1);
            }

            return 6 * cycles3 / sum;
        }

        protected override Double CalculateCycles3()
        {
            return Count3Cycle(0, 0)[0];
        }

        protected AdjacencyGraph<int, Edge<int>> ToQuickGraph()
        {
            var g = new AdjacencyGraph<int, Edge<int>>();

            for (var i = 0; i < this.container.Size; ++i)
            {
                g.AddVertex(i);
            }

            for (int i = 0; i < this.container.Size; ++i)
            {
                for (int j = i + 1; j < this.container.Size; ++j)
                    if (this.container[i, j] == 1)
                        g.AddEdge(new Edge<int>(i, j));
            }

            return g;
        }

        protected override double CalculateDegeneracy()
        {
           
            return this.ToQuickGraph().CoreDecomposition().Degeneracy;
        }

        protected override List<double> CalculateCoreCollapseSequence()
        {
            return this.ToQuickGraph().CoreDecomposition().CollapseSequence.Values.ToList();
        }

        protected override Double CalculateCycles4()
        {
            return Count4Cycle(0, 0)[0];
        }

        protected override Double CalculateCycles5()
        {
            return Count5Cycle(0, 0)[0];
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
        private double averagePathLength;
        private uint diameter;
        private SortedDictionary<Double, Double> distanceDistribution =
            new SortedDictionary<Double, Double>();

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

        // Возвращает распределение степеней.
        // Распределение степеней вычисляется в данном узле данного уровня.
        private SortedDictionary<Double, Double> DegreeDistributionInCluster(int numberNode, 
            int level)
        {
            if (level == container.Level)
            {
                SortedDictionary<Double, Double> returned = new SortedDictionary<Double, Double>();
                returned[0] = 1;
                return returned;
            }
            else
            {
                BitArray node = container.TreeNode(level, numberNode);

                SortedDictionary<Double, Double> arraysReturned = new SortedDictionary<Double, Double>();
                SortedDictionary<Double, Double> array = new SortedDictionary<Double, Double>();
                int powPK = Convert.ToInt32(Math.Pow(container.BranchingIndex, container.Level - level - 1));

                int si = (int)(numberNode * container.BranchingIndex);
                for (int i = si; i < container.BranchingIndex * (numberNode + 1); i++)
                {
                    int nodeNumberi = i - si;
                    array = DegreeDistributionInCluster(i, level + 1);
                    int countAjacentsThisnode = container.CountConnectedBlocks(node, nodeNumberi);
                    foreach (KeyValuePair<Double, Double> kvt in array)
                    {
                        Double key = kvt.Key + countAjacentsThisnode * powPK;
                        if (arraysReturned.ContainsKey(key))
                            arraysReturned[key] += kvt.Value;
                        else
                            arraysReturned.Add(key, kvt.Value);
                    }

                }
                return arraysReturned;
            }
        }

        // Возвращает информацию о пути подграфа (реализована рекурсивным образом).
        // Используется алгоритм Флойда для вычисления минимальных путей между вершинами графа.
        private long[] GetSubgraphsPathInfo(int level, long nodeNumber)
        {
            //resultArr's and tempinfo's 
            //1 element is current paths, that can't minimized, lengths sum
            //2 temp paths count, that have chance to be minimized
            //3 >2 paths' lengths sum
            long[] resultArr = { 0, 0, 0 };
            long[] tempInfo = { 0, 0, 0 };

            // Если это не лист дерева, то проход по всем дочерным узлам (рекурсивный вызов).
            if (level < container.Level - 1)
            {
                for (int i = 0; i < container.BranchingIndex; i++)
                {
                    tempInfo = GetSubgraphsPathInfo(level + 1, nodeNumber * container.BranchingIndex + i);

                    resultArr[0] += tempInfo[0];
                    if (container.NodeChildAdjacentsCount(level, nodeNumber, i) > 0)
                    {
                        resultArr[0] += tempInfo[1] * 2;
                    }
                    else
                    {
                        resultArr[1] += tempInfo[1];
                        resultArr[2] += tempInfo[2];
                    }
                }
            }

            // Получение суммы длин минимальных путей (и дополнительной информации) для данного узла.
            tempInfo = Engine.FloydMinPath(container.NodeMatrix(level, nodeNumber));

            double tempPow = Math.Pow(container.BranchingIndex, container.Level - level - 1);
            resultArr[0] += tempInfo[0] * Convert.ToInt64(Math.Pow(tempPow, 2));
            resultArr[1] += tempInfo[1] * Convert.ToInt64(Math.Pow(tempPow, 2));
            resultArr[2] += tempInfo[2] * Convert.ToInt64(Math.Pow(tempPow, 2));

            return resultArr;
        }

        private SortedDictionary<Double, Double> ConnectedSubgraphsInCluster(int numberNode, int level)
        {
            SortedDictionary<Double, Double> retArray = new SortedDictionary<Double, Double>();

            if (level == container.Level)
            {
                retArray[1] = 1;
                return retArray;
            }
            BitArray node = container.TreeNode(level, numberNode);

            bool haveOne = false;
            for (int i = 0; i < container.BranchingIndex; i++)
            {
                if (container.CountConnectedBlocks(node, i) == 0)
                {
                    SortedDictionary<Double, Double> array = 
                        ConnectedSubgraphsInCluster((int)(numberNode * container.BranchingIndex + i),
                        level + 1);

                    foreach (KeyValuePair<Double, Double> kvt in array)
                    {
                        if (retArray.Keys.Contains(kvt.Key))
                            retArray[kvt.Key] += kvt.Value;
                        else
                            retArray.Add(kvt.Key, kvt.Value);
                    }
                }
                else
                    haveOne = true;
            }

            if (haveOne)
            {
                int powPK = Convert.ToInt32(Math.Pow(container.BranchingIndex, container.Level - level - 1));
                EngineForConnectedComp engForConnectedComponent = new EngineForConnectedComp();
                ArrayList arrConnComp = engForConnectedComponent.GetCountConnSGraph(container.NodeAdjacencyLists(node),
                    (int)container.BranchingIndex);
                Double uKey = 0;
                for (int i = 0; i < arrConnComp.Count; i++)
                {
                    uKey = ((int)arrConnComp[i] * powPK);
                    if (retArray.Keys.Contains(uKey))
                        retArray[uKey] += 1;
                    else
                        retArray.Add(uKey, 1);
                }
            }

            return retArray;
        }

        // Возвращает число циклов порядка 3 в нулевом элементе SortedDictionary<int, double>.
        // Число циклов вычисляется в данном узле данного уровня.
        private SortedDictionary<Double, Double> Count3Cycle(int numberNode, int level)
        {
            SortedDictionary<Double, Double> retArray = new SortedDictionary<Double, Double>();
            retArray[0] = 0; // count cycles
            retArray[1] = 0; // count edges

            if (level == container.Level)
            {
                return retArray;
            }
            else
            {
                double countCycle = 0;
                double[] countEdge = new double[container.BranchingIndex];
                int countOne = 0;
                double powPK = Math.Pow(container.BranchingIndex, container.Level - level - 1);
                BitArray node = container.TreeNode(level, numberNode);

                int ni = (int)(numberNode * container.BranchingIndex);
                for (int i = ni; i < container.BranchingIndex * (numberNode + 1); i++)
                {
                    SortedDictionary<Double, Double> arr = new SortedDictionary<Double, Double>();
                    arr = Count3Cycle(i, level + 1);
                    countEdge[i - numberNode * container.BranchingIndex] = arr[1];
                    retArray[0] += arr[0];
                    retArray[1] += arr[1];
                }
                for (int i = 0; i < (container.BranchingIndex * (container.BranchingIndex - 1) / 2); i++)
                {
                    countOne += (node[i]) ? 1 : 0;
                }
                retArray[1] += countOne * powPK * powPK;


                for (int i = ni; i < container.BranchingIndex * (numberNode + 1); i++)
                {
                    for (int j = (i + 1); j < container.BranchingIndex * (numberNode + 1); j++)
                    {
                        if (container.AreConnectedTwoBlocks(node, i - ni, j - ni))
                        {
                            countCycle += (countEdge[i - numberNode * container.BranchingIndex] +
                                countEdge[j - numberNode * container.BranchingIndex]) * powPK;

                            for (int k = (j + 1); k < container.BranchingIndex * (numberNode + 1); k++)
                            {
                                if (container.AreConnectedTwoBlocks(node, j - ni, k - ni)
                                    && container.AreConnectedTwoBlocks(node, i - ni, k - ni))
                                    countCycle += powPK * powPK * powPK;
                            }
                        }
                    }
                }
                retArray[0] += countCycle;

                return retArray;
            }
        }

        // Возвращает число циклов порядка 4 в нулевом элементе SortedDictionary<int, double>.
        // Число циклов вычисляется в данном узле данного уровня.
        private SortedDictionary<Double, Double> Count4Cycle(int nodeNumber, int level)
        {
            SortedDictionary<Double, Double> arrayReturned = new SortedDictionary<Double, Double>();
            arrayReturned[0] = 0; // число циклов порядка 4
            arrayReturned[1] = 0; // число путей длиной 1 (ребер)
            arrayReturned[2] = 0; // число путей длиной 2

            if (level == container.Level)
            {
                return arrayReturned;
            }
            else
            {
                SortedDictionary<int, SortedDictionary<Double, Double>> array =
                    new SortedDictionary<int, SortedDictionary<Double, Double>>();
                int bIndex = (int)container.BranchingIndex;

                for (int i = nodeNumber * bIndex; i < (nodeNumber + 1) * bIndex; ++i)
                {
                    array[i] = Count4Cycle(i, level + 1);
                    arrayReturned[0] += array[i][0];
                    arrayReturned[1] += array[i][1];
                    arrayReturned[2] += array[i][2];
                }

                BitArray node = container.TreeNode(level, nodeNumber);
                double powPK = Math.Pow(container.BranchingIndex, container.Level - level - 1);

                for (int i = nodeNumber * bIndex; i < (nodeNumber + 1) * bIndex; ++i)
                {
                    for (int j = i + 1; j < (nodeNumber + 1) * bIndex; ++j)
                    {
                        if (container.AreConnectedTwoBlocks(node, i - nodeNumber * bIndex, j - nodeNumber * bIndex))
                        {
                            arrayReturned[0] += (array[i][2] + array[j][2]) * powPK;
                            arrayReturned[0] += 2 * array[i][1] * array[j][1];

                            arrayReturned[0] += Math.Pow(powPK * (powPK - 1) / 2, 2);

                            arrayReturned[1] += powPK * powPK;

                            arrayReturned[2] += 2 * powPK * (array[i][1] + array[j][1]);
                            arrayReturned[2] += powPK * powPK * (powPK - 1);
                        }

                        for (int k = j + 1; k < (nodeNumber + 1) * bIndex; ++k)
                        {
                            if (container.AreConnectedTwoBlocks(node, i - nodeNumber * bIndex, j - nodeNumber * bIndex) &&
                                container.AreConnectedTwoBlocks(node, j - nodeNumber * bIndex, k - nodeNumber * bIndex) &&
                                container.AreConnectedTwoBlocks(node, i - nodeNumber * bIndex, k - nodeNumber * bIndex))
                            {
                                arrayReturned[0] += 2 * (array[i][1] + array[j][1] + array[k][1]) * powPK * powPK;
                            }

                            if (container.AreConnectedTwoBlocks(node, i - nodeNumber * bIndex, j - nodeNumber * bIndex) &&
                                container.AreConnectedTwoBlocks(node, j - nodeNumber * bIndex, k - nodeNumber * bIndex))
                            {
                                arrayReturned[0] += powPK * powPK * powPK * (powPK - 1) / 2;

                                arrayReturned[2] += powPK * powPK * powPK;
                            }

                            if (container.AreConnectedTwoBlocks(node, i - nodeNumber * bIndex, k - nodeNumber * bIndex) &&
                                container.AreConnectedTwoBlocks(node, k - nodeNumber * bIndex, j - nodeNumber * bIndex))
                            {
                                arrayReturned[0] += powPK * powPK * powPK * (powPK - 1) / 2;

                                arrayReturned[2] += powPK * powPK * powPK;
                            }

                            if (container.AreConnectedTwoBlocks(node, i - nodeNumber * bIndex, j - nodeNumber * bIndex) &&
                                container.AreConnectedTwoBlocks(node, i - nodeNumber * bIndex, k - nodeNumber * bIndex))
                            {
                                arrayReturned[0] += powPK * powPK * powPK * (powPK - 1) / 2;

                                arrayReturned[2] += powPK * powPK * powPK;
                            }

                            for (int l = k + 1; l < (nodeNumber + 1) * bIndex; ++l)
                            {
                                bool b1 = container.AreConnectedTwoBlocks(node, i - nodeNumber * bIndex, j - nodeNumber * bIndex) &&
                                    container.AreConnectedTwoBlocks(node, j - nodeNumber * bIndex, k - nodeNumber * bIndex) &&
                                    container.AreConnectedTwoBlocks(node, k - nodeNumber * bIndex, l - nodeNumber * bIndex) &&
                                    container.AreConnectedTwoBlocks(node, i - nodeNumber * bIndex, l - nodeNumber * bIndex);
                                bool b2 = container.AreConnectedTwoBlocks(node, i - nodeNumber * bIndex, j - nodeNumber * bIndex) &&
                                    container.AreConnectedTwoBlocks(node, j - nodeNumber * bIndex, l - nodeNumber * bIndex) &&
                                    container.AreConnectedTwoBlocks(node, l - nodeNumber * bIndex, k - nodeNumber * bIndex) &&
                                    container.AreConnectedTwoBlocks(node, i - nodeNumber * bIndex, k - nodeNumber * bIndex);
                                bool b3 = container.AreConnectedTwoBlocks(node, i - nodeNumber * bIndex, l - nodeNumber * bIndex) &&
                                    container.AreConnectedTwoBlocks(node, l - nodeNumber * bIndex, j - nodeNumber * bIndex) &&
                                    container.AreConnectedTwoBlocks(node, j - nodeNumber * bIndex, k - nodeNumber * bIndex) &&
                                    container.AreConnectedTwoBlocks(node, i - nodeNumber * bIndex, k - nodeNumber * bIndex);
                                if (b1)
                                {
                                    arrayReturned[0] += powPK * powPK * powPK * powPK;
                                }

                                if (b2)
                                {
                                    arrayReturned[0] += powPK * powPK * powPK * powPK;
                                }

                                if (b3)
                                {
                                    arrayReturned[0] += powPK * powPK * powPK * powPK;
                                }
                            }
                        }
                    }
                }

                return arrayReturned;
            }
        }

        private List<List<int>> getMatches(List<int> _arr)
        {
            if (_arr.Count == 1)
            {
                return new List<List<int>> { _arr };
            }
            var result = new List<List<int>>();
            foreach (var i in _arr)
            {
                var ints = _arr.Select(b => b).ToList();
                ints.Remove(i);
                var matches = getMatches(ints);
                foreach (var match in matches)
                {
                    match.Add(i);
                    result.Add(match);
                }
            }
            return result;
        }

        private List<List<int>> matchesFrom4 = null;
        private List<List<int>> matchesFrom5 = null;

        private IEnumerable<List<int>> getMatchesFrom4
        {
            get { return matchesFrom4 ?? (matchesFrom4 = getMatches(new List<int> { 0, 1, 2, 3 })); }
        }

        private IEnumerable<List<int>> getMatchesFrom5
        {
            get { return matchesFrom5 ?? (matchesFrom5 = getMatches(new List<int> { 0, 1, 2, 3, 4 })); }
        }

        public SortedDictionary<Double, Double> Count5Cycle(int nodeNumber, int level)
        {
            SortedDictionary<Double, Double> arrayReturned = new SortedDictionary<Double, Double>();
            arrayReturned[0] = 0; // число циклов порядка 5
            arrayReturned[1] = 0; // число путей длиной 1 (ребер)
            arrayReturned[2] = 0; // число путей длиной 2
            arrayReturned[3] = 0; // число путей длиной 3

            if (level == container.Level)
            {
                return arrayReturned;
            }
            var array = new SortedDictionary<int, SortedDictionary<Double, Double>>();
            int bIndex = (int)container.BranchingIndex;

            for (int i = nodeNumber * bIndex; i < (nodeNumber + 1) * bIndex; ++i)
            {
                array[i] = Count5Cycle(i, level + 1);
                arrayReturned[0] += array[i][0];
                arrayReturned[1] += array[i][1];
                arrayReturned[2] += array[i][2];
                arrayReturned[3] += array[i][3];
            }

            BitArray node = container.TreeNode(level, nodeNumber);
            double powPK = Math.Pow(container.BranchingIndex, container.Level - (level + 1));

            int indexFrom = nodeNumber * bIndex;
            int indexTo = (nodeNumber + 1) * bIndex;

            Func<List<int>, List<List<int>>> matchesFunc = null;
            matchesFunc = delegate(List<int> _arr)
            {
                if (_arr.Count == 1)
                {
                    return new List<List<int>> { _arr };
                }
                var result = new List<List<int>>();
                foreach (var i in _arr)
                {
                    var ints = _arr.Select(b => b).ToList();
                    ints.Remove(i);
                    var matches = matchesFunc(ints);
                    foreach (var match in matches)
                    {
                        match.Add(i);
                        result.Add(match);
                    }
                }
                return result;
            };

            Func<int, int, bool> isConnected =
                (i, j) => container.AreConnectedTwoBlocks(node, i - indexFrom, j - indexFrom);
            Func<int[], bool> isAllConnected = delegate(int[] indexes)
            {
                for (int i = 0; i < indexes.Length; i++)
                {
                    for (int j = i + 1; j < indexes.Length; j++)
                    {
                        if (!isConnected(i, j))
                        {
                            return false;
                        }
                    }
                }
                return true;
            };
            Func<List<int>, int[], bool> isCircle = delegate(List<int> vertexList, int[] indexes)
            {
                var count = vertexList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (!isConnected(indexes[vertexList[i]], indexes[vertexList[(i + 1) % count]]))
                    {
                        return false;
                    }
                }
                return true;
            };

            //Func<double, double> fact = container.Factorial;
            SortedDictionary<Double, Double> ar = arrayReturned;
            SortedDictionary<int, SortedDictionary<Double, Double>> a = array;
            var n = (int)powPK;
            int n2 = n * n;
            int n3 = n2 * n;
            int n4 = n3 * n;
            int n5 = n4 * n;
            int n_2 = n * (n - 1) / 2;

            for (int i = indexFrom; i < indexTo; ++i)
            {
                for (int j = i + 1; j < indexTo; ++j)
                {
                    SortedDictionary<Double, Double> a_i = a[i];
                    SortedDictionary<Double, Double> a_j = a[j];
                    if (isConnected(i, j))
                    {
                        ar[0] += (a_i[2] * a_j[1]) / 2;
                        ar[0] += (a_i[1] * a_j[2]) / 2;
                        ar[0] += a_i[1] * n_2 * (n - 2);
                        ar[0] += a_j[1] * n_2 * (n - 2);
                        ar[0] += (a_i[3] / 2) * n;
                        ar[0] += (a_j[3] / 2) * n;

                        ar[1] += 2 * n2;
                        ar[2] += (a_i[1] + a_j[1]) * n * 2;
                        ar[2] += n_2 * n * 2;
                        ar[2] += n_2 * n * 2;
                        ar[3] += (a_i[2] + a_j[2]) * n * 2;
                        ar[3] += (a_i[1] + a_j[1]) * n_2 * 2;
                        ar[3] += (a_i[1] + a_j[1]) * (n - 2) * n * 2;
                        ar[3] += a_i[1] * a_j[1] * 2;
                        ar[3] += n_2 * n_2 * 8;
                    }
                    for (int k = j + 1; k < indexTo; k++)
                    {
                        SortedDictionary<Double, Double> a_k = a[k];
                        if (isAllConnected(new[] { i, j, k }))
                        {
                            var ds = new[] { a_i, a_j, a_k };
                            foreach (var d in ds)
                            {
                                ar[0] += d[2] * n2; // 9
                                ar[0] += d[1] * (n - 2) * n2; // 10
                                ar[0] += n_2 * n_2 * n * 4; // 12
                                ar[0] += d[1] * n_2 * n * 2; // 13

                                ar[3] += (d[1] / 2) * n2 * 12;
                            }
                            for (int l = 0; l < 3; l++)
                            {
                                var b1 = ds[(l + 1) % 3];
                                var b2 = ds[(l + 2) % 3];
                                ar[0] += b1[1] * b2[1] * n; // 11
                            }
                            ar[2] += n3 * 6; // fact(3);
                            ar[3] += n_2 * n2 * 12 * 3;
                        }
                        else // 13, 10
                        {
                            SortedDictionary<Double, Double> b1 = null, b2 = null, b = null;
                            if (isConnected(i, j) && isConnected(j, k) && !isConnected(i, k))
                            {
                                b1 = a_i;
                                b2 = a_k;
                                b = a_j;
                            }
                            else if (isConnected(i, j) && isConnected(i, k) && !isConnected(j, k))
                            {
                                b1 = a_j;
                                b2 = a_k;
                                b = a_i;
                            }
                            else if (isConnected(i, k) && isConnected(j, k) && !isConnected(i, j))
                            {
                                b1 = a_i;
                                b2 = a_j;
                                b = a_k;
                            }
                            if (b1 != null)
                            {
                                ar[0] += (b1[1] + b2[1]) * n_2 * n;
                                ar[0] += b[1] * (n - 2) * n2;

                                ar[2] += 2 * n3;
                                ar[3] += (b1[1] + b2[1]) * n2 * 2;
                                ar[3] += b[1] * n2 * 2;
                                ar[3] += n2 * n_2 * 8;
                            }
                        }

                        for (int l = k + 1; l < indexTo; l++)
                        {
                            var from4 = getMatchesFrom4;
                            var indexes = new[] { i, j, k, l };
                            foreach (List<int> ints in from4)
                            {
                                if (isCircle(ints, indexes))
                                {
                                    var b = a[indexes[ints[0]]];
                                    ar[0] += b[1] * n3;
                                    ar[0] += (n_2) * n3;
                                    ar[3] += n4;
                                }
                            }
                            for (int m = l + 1; m < indexTo; m++)
                            {
                                var from5 = getMatchesFrom5;
                                var indexes1 = new[] { i, j, k, l, m };
                                double count = 0;
                                foreach (List<int> ints in from5)
                                {
                                    if (isCircle(ints, indexes1))
                                    {
                                        count += n5;
                                    }
                                }
                                ar[0] += count / 10;
                            }
                        }
                    }
                }
            }

            return arrayReturned;
        }

        // Возвращает коэффициент класстеризации для данной вершины (vertexNumber).
        // Вычисляется с помощью числа циклов порядка 3, прикрепленных к данной вершине.
        private Double ClusterringCoefficientOfVertex(int vertexNumber)
        {
            // TODO !optimize!
            double degree = container.VertexDegree(vertexNumber, 0);
            if (degree == 0 || degree == 1)
                return 0;
            else
                return (2 * Count3CycleOfVertex(vertexNumber, 0)[0]) / (degree * (degree - 1));
        }

        // Возвращает число циклов порядка 3 прикрепленных к данному узлу 
        // в нулевом элементе SortedDictionary<int, double>.
        // Число циклов вычисляется в данном узле данного уровня.
        private SortedDictionary<Double, Double> Count3CycleOfVertex(int vertexNumber, int level)
        {
            SortedDictionary<Double, Double> result = new SortedDictionary<Double, Double>();
            result[0] = 0;  // число циклов 3 прикрепленных к данному узлу
            result[1] = 0;  // число ребер в данном подграфе (такое вычисление повышает эффективность)

            if (level == container.Level)
            {
                return result;
            }
            else
            {
                int numberNode = container.TreeIndex(vertexNumber, level);
                int vertexIndex = container.TreeIndex(vertexNumber, level + 1) % (int)container.BranchingIndex;
                BitArray node = container.TreeNode(level, numberNode);
                double powPK = Math.Pow(container.BranchingIndex, container.Level - level - 1);

                SortedDictionary<Double, Double> previousResult = Count3CycleOfVertex(vertexNumber, level + 1);
                result[0] += previousResult[0];
                result[1] += previousResult[1];

                double degree = container.VertexDegree(vertexNumber, level + 1);
                int nj = (int)(numberNode * container.BranchingIndex);
                for (int j = nj; j < container.BranchingIndex * (numberNode + 1); ++j)
                {
                    if (container.AreConnectedTwoBlocks(node, vertexIndex, j - nj))
                    {
                        result[0] += container.CalculateNumberOfEdges(level + 1, j);
                        result[0] += powPK * degree;

                        for (int k = j + 1; k < container.BranchingIndex * (numberNode + 1); ++k)
                        {
                            if (container.AreConnectedTwoBlocks(node, j - nj, k - nj) &&
                                container.AreConnectedTwoBlocks(node, k - nj, vertexIndex))
                            {
                                result[0] += powPK * powPK;
                            }
                        }
                    }
                }

                return result;
            }
        }

        // Возвращает коэффициент кластеризации графа.
        private Double ClusteringCoefficientOfVertex(long vert)
        {
            double sum = 0;
            long adjCount = 0;
            //loop over all levels
            for (int level = (int)container.Level - 1; level >= 0; level--)
            {
                //get vertex position in current level
                long vertNodeNum = Convert.ToInt64(Math.Floor(Convert.ToDouble(vert / container.BranchingIndex)));
                int vertNodeInd = Convert.ToInt32(vert % container.BranchingIndex);

                //get vertex adjacent vertexes in current node
                List<int> adjIndexes = container.NodeChildAdjacentsArray(level, vertNodeNum, vertNodeInd);

                long levelVertexCount = Convert.ToInt64(Math.Pow(container.BranchingIndex, container.Level - level - 1));
                //vertex subtree vertexes with adjacent subtrees vertexes
                long vertexSubTreeWithAdjSubTrees = adjCount * levelVertexCount * adjIndexes.Count;
                sum += vertexSubTreeWithAdjSubTrees;
                //add adjacent vertexes count
                adjCount += levelVertexCount * adjIndexes.Count;
                //adjacent subtrees weights
                for (int i = 0; i < container.BranchingIndex; i++)
                {
                    if (adjIndexes.IndexOf(i) != -1)
                    {
                        sum += container.CalculateNumberOfEdges(level + 1, 
                            vertNodeNum * container.BranchingIndex + i);
                    }
                }
                //connectivity of adjacent subtrees
                for (int i = 0; i < container.BranchingIndex; i++)
                {
                    if (adjIndexes.IndexOf(i) != -1)
                    {
                        for (int j = i; j < container.BranchingIndex; j++)
                        {
                            if (i != j && i != vertNodeInd && j != vertNodeInd)
                            {
                                sum += container.AreAdjacent(level, vertNodeNum, i, j) * Math.Pow(levelVertexCount, 2);
                            }
                        }
                    }
                }

                vert = vertNodeNum;
            }
            double vertClustCoef = 0;
            if (adjCount > 1)
            {
                vertClustCoef = 2 * sum / (adjCount * (adjCount - 1));
            }
            else if (adjCount == 1)
            {
                vertClustCoef = sum;
            }

            return vertClustCoef;
        }

        // Возвращает собственные значения.
        private List<Double> CalcEigenValue(BitArray bitArr, int mBase)
        {
            List<double> EigValue = new List<double>();

            List<double> basicEigValue = new List<double>(mBase);
            List<double> eigValueE = new List<double>(mBase);
            for (int i = 1; i < mBase; ++i)
            {
                eigValueE.Add(0);
            }
            eigValueE.Add(mBase);
            int bitArrSize = bitArr.Count;
            if (bitArr[0] == false)
            {
                if (bitArr[1] == false)
                {
                    for (int i = 0; i < mBase; ++i)
                    {
                        basicEigValue.Add(0);
                    }
                }
                else
                {
                    for (int i = 0; i < mBase; ++i)
                    {
                        basicEigValue.Add(1);
                    }
                }
            }
            else
            {
                if (bitArr[1] == false)
                {
                    for (int i = 1; i < mBase; ++i)
                    {
                        basicEigValue.Add(-1);
                    }
                    basicEigValue.Add(mBase - 1);
                }
                else
                {
                    for (int i = 1; i < mBase; ++i)
                    {
                        basicEigValue.Add(0);
                    }
                    basicEigValue.Add(mBase);
                }
            }
            int size = mBase;
            BitArray BA = new BitArray(bitArrSize + 1);
            for (int i = 0; i < bitArrSize; ++i)
                BA[i] = bitArr[i];
            BA.Set(bitArrSize, false);
            int x = 1;
            while (x != bitArrSize)
            {
                foreach (int elemE in eigValueE)
                {
                    int t1, t2;
                    if (BA[x] == true)
                        t1 = 1;
                    else
                        t1 = 0;
                    if (BA[x + 1] == true)
                        t2 = 1;
                    else
                        t2 = 0;
                    foreach (int elem in basicEigValue)
                        EigValue.Add(elem * elemE - t1 + t2);
                }
                ++x;
                basicEigValue.Clear();
                basicEigValue.InsertRange(0, EigValue);
                EigValue.Clear();

            }

            EigValue.InsertRange(0, basicEigValue);
            return EigValue;
        }

        // Возвращает число циклов данного порядка, с помощью собственных значений.
        public Double CalcCyclesCount(int cycleLength)
        {
            BitArray vector = new BitArray((int)container.Level);

            for (int i = 0; i < container.HierarchicTree.Length; i++)
            {
                vector[i] = container.HierarchicTree[i][0][0];
            }

            List<double> eigValue = CalcEigenValue(vector, (int)container.BranchingIndex);

            double total = 0;
            foreach (int i in eigValue)
            {
                total += Math.Pow(i, cycleLength);
            }
            return total / (2 * cycleLength);
        }

        // Возвращает среднее степеней.
        public Double AverageDegree()
        {
            return container.CalculateNumberOfEdges() * 2 / container.Size;
        }

        // Возвращает сумму минимальных путей. Не используется.
        public Double MinPathsSum()
        {
            long[] pathsInfo = GetSubgraphsPathInfo(0, 0);
            return pathsInfo[0] + pathsInfo[2];
        }

        #endregion
    }
}
