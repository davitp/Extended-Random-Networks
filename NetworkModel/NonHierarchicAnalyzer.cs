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
    public class NonHierarchicAnalyzer : AbstractNetworkAnalyzer
    {
        private NonHierarchicContainer container;

        public NonHierarchicAnalyzer(AbstractNetwork n) : base(n) { }

        public override INetworkContainer Container
        {
            get { return container; }
            set { container = (NonHierarchicContainer)value; }
        }

        protected override Double CalculateEdgesCountOfNetwork()
        {
            return (Double)container.ExistingEdgesCount();
        }

        protected override Double CalculateAveragePath()
        {
            if(!calledPaths)
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

        protected override List<Double> CalculateEigenValues()
        {
            bool[,] m = container.GetMatrix();

            EigenValueUtils eg = new EigenValueUtils();            
            try
            {
                eigenValues = eg.CalculateEigenValue(m);
                calledEigens = true;
                return eigenValues;
            }
            catch (SystemException)
            {
                return new List<Double>();
            }
        }

        protected override SortedDictionary<Double, Double> CalculateEigenDistanceDistribution()
        {
            bool[,] m = container.GetMatrix();

            EigenValueUtils eg = new EigenValueUtils();
            try
            {
                if (!calledEigens)
                    eg.CalculateEigenValue(m);

                return eg.CalcEigenValuesDist(eigenValues);
            }
            catch (SystemException)
            {
                return new SortedDictionary<Double, Double>();
            }
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
                        list = container.Neighbourship[item];
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
        //-----------------------------Aram--------------------------------------------------------------

        protected override SortedDictionary<double, double> CalculateSubtreeDistribution()
        {
            List<NonHierarchicContainer.Edge> Edges = new List<NonHierarchicContainer.Edge>();
            int i = 0;
            foreach (var item in container.ExistingEdges())
            {
                Edges.Add(new NonHierarchicContainer.Edge(item.Key, item.Value, i));
                i++;
            }
            List<NonHierarchicContainer.Edge> tree = new List<NonHierarchicContainer.Edge>();
            Stack<NonHierarchicContainer.Edge> stack = new Stack<NonHierarchicContainer.Edge>();
            foreach (NonHierarchicContainer.Edge edge in Edges)
                stack.Push(edge);

            List<int> l = new List<int>();
            for (int j = 0; j < Edges.Count; j++)
                l.Add(0);

            while (stack.Count != 0)
            {
                NonHierarchicContainer.Edge edge = stack.Pop();
                if (!tree.Remove(edge))
                {
                    tree.Add(edge);
                    l[tree.Count]++;
                    stack.Push(edge);
                    foreach (NonHierarchicContainer.Edge e in Edges)
                        if (NonHierarchicContainer.IsMaxHanging(tree, e))
                            stack.Push(e);
                }
            }
            SortedDictionary<double, double> result = new SortedDictionary<double, double>();
            for (int j = 1; j < l.Count; j++)
                result.Add(j, l[j]);

            return result;
        }
        //---------------------------------------------------------------------------------
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

        protected override SortedDictionary<Double, Double> CalculateCycleDistribution(UInt32 lowBound, UInt32 hightBound)
        {
            CyclesCounter cyclesCounter = new CyclesCounter(container);
            SortedDictionary<Double, Double> cyclesCount =
                new SortedDictionary<Double, Double>();
            double count = 0;
            for (int i = (int)lowBound; i <= hightBound; i++)
            {
                count = cyclesCounter.getCyclesCount(i);
                cyclesCount.Add(i, count);
            }

            return cyclesCount;
        }

        protected override SortedDictionary<Double, Double> CalculateCycles3Trajectory()
        {
            // Retrieving research parameters from network. Research MUST be Evolution. //
            Debug.Assert(network.ResearchParameterValues != null);
            Debug.Assert(network.ResearchParameterValues.ContainsKey(ResearchParameter.EvolutionStepCount));
            Debug.Assert(network.ResearchParameterValues.ContainsKey(ResearchParameter.TracingStepIncrement));
            Debug.Assert(network.ResearchParameterValues.ContainsKey(ResearchParameter.Nu));
            Debug.Assert(network.ResearchParameterValues.ContainsKey(ResearchParameter.PermanentDistribution));

            UInt32 stepCount = Convert.ToUInt32(network.ResearchParameterValues[ResearchParameter.EvolutionStepCount]);
            Double nu = Convert.ToDouble(network.ResearchParameterValues[ResearchParameter.Nu]);
            bool permanentDistribution = Convert.ToBoolean(network.ResearchParameterValues[ResearchParameter.PermanentDistribution]);
            object v = network.ResearchParameterValues[ResearchParameter.TracingStepIncrement];
            UInt32 tracingStepIncrement = ((v != null) ? Convert.ToUInt32(v) : 0);

            // keep initial container
            NonHierarchicContainer initialContainer = container.Clone();

            SortedDictionary<Double, Double> trajectory = new SortedDictionary<Double, Double>();
            uint currentStep = 0;
            uint currentTracingStep = tracingStepIncrement;
            double currentCycle3Count = CalculateCycles3();
            trajectory.Add(currentStep, currentCycle3Count);

            NonHierarchicContainer previousContainer = new NonHierarchicContainer();
            RNGCrypto rand = new RNGCrypto();
            while (currentStep != stepCount)
            {
                previousContainer = container.Clone();
                try
                {
                    ++currentStep;

                    long deltaCount = permanentDistribution ?
                        container.PermanentRandomization() : 
                        container.NonPermanentRandomization();
                    double newCycle3Count = currentCycle3Count + deltaCount;

                    int delta = (int)(newCycle3Count - currentCycle3Count);
                    if (delta > 0)
                    {
                        // accept
                        trajectory.Add(currentStep, newCycle3Count);
                        currentCycle3Count = newCycle3Count;
                    }
                    else
                    {
                        double probability = Math.Exp((-nu * Math.Abs(delta)));
                        if (rand.NextDouble() < probability)
                        {
                            // accept
                            trajectory.Add(currentStep, newCycle3Count);
                            currentCycle3Count = newCycle3Count;
                        }
                        else
                        {
                            // reject
                            trajectory.Add(currentStep, currentCycle3Count);
                            container = previousContainer;
                        }
                    }

                    network.UpdateStatus(NetworkStatus.StepCompleted);

                    if (currentTracingStep == currentStep)
                    {
                        container.Trace(network.ResearchName, 
                            "Realization_" + network.NetworkID.ToString(), 
                            "Matrix_" + currentTracingStep.ToString());
                        currentTracingStep += tracingStepIncrement;

                        network.UpdateStatus(NetworkStatus.StepCompleted);
                    }
                }
                catch (SystemException)
                {
                    container = initialContainer;
                }
            }

            container = initialContainer;
            return trajectory;
        }

        /// <summary>
        ///  calculating   degree centrality for each vertex
        /// </summary>
        /// <returns></returns>
        protected override List<Double> CalculateDegreeCentrality()
        {
            return null; // container.Degrees;
        }

        /// <summary>
        ///  calculating   betweenness centrality for each vertex
        /// </summary>
        /// <returns></returns>
        protected override List<Double> CalculateBetweennessCentrality()
        {
            UInt32 size = container.Size;
            double[] VB = new double[size];
            double[] dist = new double[size];
            double[] sigma = new double[size];
            double[] delta = new double[size];
            List<int>[] Pred = new List<int>[size];

            for (int i = 0; i < size; i++)
            {
                VB[i] = 0;
            }
            for (int i = 0; i < size; i++)
            {
                //initialization part//
                Stack stk = new Stack();
                Queue qu = new Queue();

                for (int w = 0; w < size; w++) { Pred[w] = new List<int>(); }
                for (int t = 0; t < size; t++)
                {
                    dist[t] = -1;
                    sigma[t] = 0;
                }
                dist[i] = 0;
                sigma[i] = 1;
                qu.Enqueue(i);

                //end initialization part//

                while (qu.Count != 0)
                {
                    int v = (int)qu.Dequeue();
                    stk.Push(v);
                    foreach (int neigh in container.Neighbourship[v])
                    {
                        if (dist[neigh] < 0)
                        {
                            qu.Enqueue(neigh);
                            dist[neigh] = dist[v] + 1;
                        }
                        if (dist[neigh] == dist[v] + 1)
                        {
                            sigma[neigh] = sigma[neigh] + sigma[v];
                            Pred[neigh].Add(v);
                        }
                    }
                }
                for (int j = 0; j < size; j++) { delta[j] = 0; }
                //accumulation part//
                while (stk.Count != 0)
                {
                    int w = (int)stk.Pop();
                    for (int j = 0; j < Pred[w].Count; j++)
                    {
                        int t = Pred[w][j];
                        delta[t] = delta[t] + (sigma[t] / sigma[w]) * (1 + delta[w]);
                    }
                    if (w != i)
                    {
                        VB[w] = VB[w] + delta[w];
                    }
                }
            }
            for (int i = 0; i < size; i++)
            {
                VB[i] = VB[i] / 2;
            }

            //Dictionary<KeyValuePair<int, int>, double> eb= EdgeBetweenness();

            List<ConnectedComponents> comps = GirvanNewmanCommunityDetection();
            return null; //VB;
        }

        /// <summary>
        /// calculating   betweenness centrality for each edge
        /// </summary>
        /// <returns></returns>
        public Dictionary<KeyValuePair<int, int>, double> EdgeBetweenness()
        {
            UInt32 size = container.Size;
            List<KeyValuePair<int, int>> edges = container.ExistingEdges();
            Dictionary<KeyValuePair<int, int>, double> EB = new Dictionary<KeyValuePair<int, int>, double>();
            double[] dist = new double[size];
            double[] sigma = new double[size];
            double[] delta = new double[size];
            List<int>[] Pred = new List<int>[size];

            for (int i = 0; i < size; i++)
            {
                //initialization part//
                Stack stk = new Stack();
                Queue qu = new Queue();

                for (int w = 0; w < size; w++) { Pred[w] = new List<int>(); }
                for (int t = 0; t < size; t++)
                {
                    dist[t] = -1;
                    sigma[t] = 0;
                }
                dist[i] = 0;
                sigma[i] = 1;
                qu.Enqueue(i);

                //end initialization part//

                while (qu.Count != 0)
                {
                    int v = (int)qu.Dequeue();
                    stk.Push(v);
                    foreach (int neigh in container.Neighbourship[v])
                    {
                        if (dist[neigh] < 0)
                        {
                            qu.Enqueue(neigh);
                            dist[neigh] = dist[v] + 1;
                        }
                        if (dist[neigh] == dist[v] + 1)
                        {
                            sigma[neigh] = sigma[neigh] + sigma[v];
                            Pred[neigh].Add(v);
                        }
                    }
                }
                for (int j = 0; j < size; j++) { delta[j] = 0; }

                double currenteb = 0.0;

                //accumulation part//
                while (stk.Count != 0)
                {
                    int w = (int)stk.Pop();
                    for (int j = 0; j < Pred[w].Count; j++)
                    {
                        int t = Pred[w][j];

                        KeyValuePair<int, int> currentedge = new KeyValuePair<int, int>(w, t);
                        KeyValuePair<int, int> currentedgeduplicate = new KeyValuePair<int, int>(t, w);

                        if (!EB.Keys.Contains(currentedge) && !EB.Keys.Contains(currentedgeduplicate))
                        {
                            EB.Add(currentedge, currenteb);
                        }

                        KeyValuePair<int, int> tempedge = EB.Keys.Contains(currentedge) ? currentedge : currentedgeduplicate;

                        EB[tempedge] += (sigma[t] / sigma[w]) * (1 + delta[w]);
                        delta[t] = delta[t] + (sigma[t] / sigma[w]) * (1 + delta[w]);
                    }
                }
            }
            return EB;
        }

        /// <summary>
        ///  calculating   closeness centrality for each vertex
        /// </summary>
        /// <returns></returns>
        protected override List<Double> CalculateClosenessCentrality()
        {
            UInt32 size = container.Size;
            double[] cCentralities = new double[size];
            int[] far = new int[size];
            int[] d = new int[size];

            for (int i = 0; i < size; i++)
            {
                Queue qu = new Queue();
                for (int j = 0; j < size; j++) { d[j] = -1; }
                qu.Enqueue(i);
                d[i] = 0;
                far[i] = 0;
                while (qu.Count != 0)
                {
                    int v = (int)qu.Dequeue();
                    foreach (int w in container.Neighbourship[v])
                    {
                        if (d[w] < 0)
                        {
                            qu.Enqueue(w);
                            d[w] = d[v] + 1;
                            far[i] = far[i] + d[w];
                        }
                    }
                }
                cCentralities[i] = (double)1 / (double)far[i];
            }

            return null; // cCentralities;
        }


        protected override double CalculateDr()
        {
            uint Size = container.Size;
            List<List<int>> matrix = new List<List<int>>();
            List<List<int>> lstNr = new List<List<int>>();
            

            for (int i = 0; i < Size; ++i)
            {
                lstNr.Add(container.Neighbourship[i]);
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
                        f(p, tmplst);
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
            
            if(lst != null)
                foreach (int item in lst)
                {
                    if (temp.Contains(item))
                    {
                        temp.Remove(item);
                    }
                }
                 
            return temp.Count;
        }

        private void f(int i, List<int> lst)
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
                List<int> l = container.Neighbourship[u];
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
                List<int> l = container.Neighbourship[u];
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
            SortedDictionary<Double, Double> degreeDistribution = new SortedDictionary<Double, Double>();
            for (uint i = 0; i < container.Size; ++i)
                degreeDistribution[i] = new Double();

            for (int i = 0; i < container.Size; ++i)
            {
                int degreeOfVertexI = container.Neighbourship[i].Count;
                ++degreeDistribution[degreeOfVertexI];
            }

            for (uint i = 0; i < container.Size; ++i)
                if (degreeDistribution[i] == 0)
                    degreeDistribution.Remove(i);

            return degreeDistribution;
        }

        // Разобраться, почему не реализована соответсвующая функция интерфейса.
        private int fullSubGgraph(int u)
        {
            List<int> n1;
            n1 = container.Neighbourship[u];
            List<int> n2 = new List<int>();
            int l = 0;
            bool t;
            int k = 0;
            while (l != n1.Count)
            {
                n2.Clear();
                n2.Add(u);
                if (l != 0)
                    n2.Add(n1[l]);
                for (int i = 0; i < n1.Count && i != l; i++)
                {
                    t = true;
                    for (int j = 0; j < n2.Count; j++)
                        if (container.AreConnected(n1[i], n2[j]) == false)
                        {
                            t = false;
                            break;
                        }
                    if (t == true)
                        n2.Add(n1[i]);
                }
                int p = n2.Count;
                if (p > k)
                    k = p;
                l++;
            }
            return k;
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

        // Возвращает степень максимального соединенного подграфа. Не используется.
        private int GetMaxFullSubgraph()
        {
            int k = 0;
            for (int i = 0; i < container.Size; i++)
                if (this.fullSubGgraph(i) > k)
                    k = this.fullSubGgraph(i);

            return k;
        }

        private long Get4OrderCyclesOfNode(int j)
        {
            List<int> neigboursList = container.Neighbourship[j];
            List<int> neigboursList1 = new List<int>();
            List<int> neigboursList2 = new List<int>();
            long count = 0;
            for (int i = 0; i < neigboursList.Count; i++)
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
            }

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


        //******comunity detection******//

        public void GirvanNewmanStep()
        {

            int initNcomp = GetConnectedComponents().Count();
            int ncomp = initNcomp;
            while (ncomp <= initNcomp)
            {
                // edge betweenness for G
                Dictionary<KeyValuePair<int, int>, double> bw = EdgeBetweenness();
                //find the edge with max centrality
                double max = bw.Values.Max();
                //find the edge with the highest centrality and remove all of them if there is more than one!

                foreach (KeyValuePair<KeyValuePair<int, int>, double> item in bw)
                {
                    if (item.Value == max)
                    {
                        container.RemoveConnection(item.Key.Key, item.Key.Value);
                    }
                }
                ncomp = GetConnectedComponents().Count();
            }
        }

        public double GetGirvanNewmanModularity()
        {
            double mod = 0.0;

            // TODO GETMATRIX is called
            bool[,] NewMatrix = container.GetMatrix();
            List<int> NewDeg = UpdateDegrees(NewMatrix, container.Size);
            List<ConnectedComponents> comps = GetConnectedComponents();
            foreach (ConnectedComponents c in comps)
            {
                int edgeWithinCmty = 0;
                int randomEdge = 0;
                foreach (List<int> u in c.component.Neighbourship.Values)
                {
                    foreach (int v in u)
                    {
                        edgeWithinCmty += NewDeg[v];
                        randomEdge += container.Degrees[v];
                    }
                }
                mod += ((float)(edgeWithinCmty) - (float)(randomEdge * randomEdge) / (float)(2 * container.CalculateNumberOfEdges()));
            }
            mod = mod / (float)(2 * container.CalculateNumberOfEdges());
            return mod;
        }


        public List<int> UpdateDegrees(bool[,] NewA, uint size)
        {
            int result = 0;
            List<int> degrees = new List<int>();
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    if (i != j)
                    {
                        if (NewA[i, j] == true)
                        {
                            ++result;
                        }
                    }
                }
                degrees.Add(result);
            }
            return degrees;
        }
        public List<ConnectedComponents> GirvanNewmanCommunityDetection()
        {
            List<ConnectedComponents> Bestcomps = new List<ConnectedComponents>();
            double BestQ = 0.0;
            double Q = 0.0;
            while (true)
           {
                GirvanNewmanStep();
                Q = GetGirvanNewmanModularity();
                if (Q > BestQ)
                {
                    BestQ = Q;
                    Bestcomps = GetConnectedComponents();
                }
                if (IsGraphEmpty())
                {
                    break;
                }
            }
            return Bestcomps;
        }

        public bool IsGraphEmpty()
        {
            bool temp = true;
            foreach (List<int> neigh in container.Neighbourship.Values)
            {
                if (neigh.Count != 0)
                {
                    temp = false;                        
                }
                    
            }
            return temp;
        }

        public IEnumerable<int> DepthFirstSearch(int nodeStart)
        {
            List<int> result = new List<int>();
            var stack = new Stack<int>();
            var visitedNodes = new HashSet<int>();
            stack.Push(nodeStart);
            while (stack.Count > 0)
            {
                var curr = stack.Pop();
                if (!visitedNodes.Contains(curr))
                {
                    visitedNodes.Add(curr);
                    yield return curr;
                    foreach (var next in container.Neighbourship[curr])
                    {
                        if (!visitedNodes.Contains(next))
                            stack.Push(next);
                    }
                }
            }          
        }
        public ConnectedComponents GetSubGraph(IEnumerable<int> nodes)
        {
            ConnectedComponents comp;
            NonHierarchicContainer g = new NonHierarchicContainer();
            Dictionary<int, int> tempnodes = new Dictionary<int, int>();
            for (int i = 0; i < nodes.Count(); i++)
            {                
                tempnodes = AddNodesToList(nodes);
                g.AddVertex();
            }
            foreach (var n in g.Neighbourship.Keys.ToList())
            {
                foreach (var neigh in container.Neighbourship[tempnodes[n]])
                {
                    for (int i = 0; i < tempnodes.Count; i++)
                    {
                        if (tempnodes[i] == neigh)
                        {
                            g.AddConnection(n, i);
                        }
                    }
                }
            }
            comp = new ConnectedComponents(g, tempnodes, tempnodes.Count);
            return comp;
        }
        public List<ConnectedComponents> GetConnectedComponents()
        {
            var visitedNodes = new HashSet<int>();
            var components = new List<ConnectedComponents>();

            foreach (var node in container.Neighbourship.Keys)
            {
                if (!visitedNodes.Contains(node))
                {
                    ConnectedComponents subGraph = GetSubGraph(DepthFirstSearch(node));
                    components.Add(subGraph);
                    visitedNodes.UnionWith(subGraph.vertexmatching.Values);
                }
            }
            return components;
        }

        public Dictionary<int, int> AddNodesToList(IEnumerable<int> nodes)
        {
            Dictionary<int, int> result = new Dictionary<int, int>();
            int[] arr= nodes.ToArray();
            for (int i = 0; i < arr.Length ;i++)
            {
                result.Add(i,arr[i]);
            }
            return result;
        }
        #endregion
    }
}
