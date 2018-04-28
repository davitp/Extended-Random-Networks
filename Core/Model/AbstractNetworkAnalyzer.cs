using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

using Core.Enumerations;
using Model.Eigenvalues;

namespace Core.Model
{
    /// <summary>
    /// Abstract class presenting random network analyzer.
    /// </summary>
    public abstract class AbstractNetworkAnalyzer : INetworkAnalyzer
    {
        protected AbstractNetwork network;

        public abstract INetworkContainer Container { get; set; }

        public AbstractNetworkAnalyzer(AbstractNetwork n)
        {
            network = n;
        }

        public Double CalculateEdgesCount()
        {
            return CalculateEdgesCountOfNetwork();
        }

        public Object CalculateOption(AnalyzeOption option)
        {
            switch (option)
            {
                case AnalyzeOption.AvgClusteringCoefficient:
                    return CalculateAverageClusteringCoefficient();
                case AnalyzeOption.AvgDegree:
                    return CalculateAverageDegree();
                case AnalyzeOption.AvgPathLength:
                    return CalculateAveragePath();
                case AnalyzeOption.ClusteringCoefficientDistribution:
                    return CalculateClusteringCoefficientDistribution();
                case AnalyzeOption.ClusteringCoefficientPerVertex:
                    return CalculateClusteringCoefficientPerVertex();
                case AnalyzeOption.CompleteComponentDistribution:
                    return CalculateCompleteComponentDistribution();
                case AnalyzeOption.ConnectedComponentDistribution:
                    return CalculateConnectedComponentDistribution();
                case AnalyzeOption.SubtreeDistribution:
                    return CalculateSubtreeDistribution();
                case AnalyzeOption.CycleDistribution:
                    // TODO
                    return CalculateCycleDistribution(1, 1);
                case AnalyzeOption.Cycles3:
                    return CalculateCycles3();
                case AnalyzeOption.Cycles3Eigen:
                    return CalculateCycles3Eigen();
                case AnalyzeOption.Cycles3Trajectory:
                    return CalculateCycles3Trajectory();
                case AnalyzeOption.Cycles4:
                    return CalculateCycles4();
                case AnalyzeOption.Cycles4Eigen:
                    return CalculateCycles4Eigen();
                case AnalyzeOption.Cycles5:
                    return CalculateCycles5();
                case AnalyzeOption.DegreeDistribution:
                    return CalculateDegreeDistribution();
                case AnalyzeOption.Diameter:
                    return CalculateDiameter();
                case AnalyzeOption.DistanceDistribution:
                    return CalculateDistanceDistribution();
                case AnalyzeOption.EigenDistanceDistribution:
                    return CalculateEigenDistanceDistribution();
                case AnalyzeOption.EigenValues:
                    return CalculateEigenValues();
                case AnalyzeOption.LaplacianEigenValues:
                    return CalculateLaplacianEigenValues();
                case AnalyzeOption.TriangleByVertexDistribution:
                    return CalculateTriangleByVertexDistribution();
                case AnalyzeOption.BetweennessCentrality:
                    return CalculateBetweennessCentrality();
                case AnalyzeOption.ClosenessCentrality:
                    return CalculateClosenessCentrality();
                case AnalyzeOption.DegreeCentrality:
                    return CalculateDegreeCentrality();
                case AnalyzeOption.Dr:
                    return CalculateDr();
                case AnalyzeOption.ModelA_OR_StdTime_All:
                    return CalculateActivePartModelA(true, true, true);
                case AnalyzeOption.ModelA_OR_ExtTime_All:
                    return CalculateActivePartModelA(true, false, true);
                case AnalyzeOption.ModelA_OR_StdTime_Passives:
                    return CalculateActivePartModelA(true, true, false);
                case AnalyzeOption.ModelA_OR_ExtTime_Passives:
                    return CalculateActivePartModelA(true, false, false);
                case AnalyzeOption.ModelA_AND_StdTime_All:
                    return CalculateActivePartModelA(false, true, true);
                case AnalyzeOption.ModelA_AND_ExtTime_All:
                    return CalculateActivePartModelA(false, false, true);
                case AnalyzeOption.ModelA_AND_StdTime_Passives:
                    return CalculateActivePartModelA(false, true, false);
                case AnalyzeOption.ModelA_AND_ExtTime_Passives:
                    return CalculateActivePartModelA(false, false, false);
                case AnalyzeOption.ModelB:
                    return CalculateActivePartModelB();
                case AnalyzeOption.Degeneracy:
                    return CalculateDegeneracy();
                default:
                    return null;
            }
        }

        /// <summary>
        /// Calculate degeneracy
        /// </summary>
        /// <returns></returns>
        protected virtual double CalculateDegeneracy()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates count of edges of the network.
        /// </summary>
        /// <returns>Count of edges.</returns>
        protected virtual Double CalculateEdgesCountOfNetwork()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the average path length of the network.
        /// </summary>
        /// <returns>Average path length.</returns>
        protected virtual Double CalculateAveragePath()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the diameter of the network.
        /// </summary>
        /// <returns>Diameter.</returns>
        protected virtual Double CalculateDiameter()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the average value of vertex degrees in the network.
        /// </summary>
        /// <returns>Average degree.</returns>
        protected virtual Double CalculateAverageDegree()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the average value of vertex clustering coefficients in the network.  
        /// </summary>
        /// <returns>Average clustering coefficient.</returns>
        protected virtual Double CalculateAverageClusteringCoefficient()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the number of cycles of length 3 in the network.
        /// </summary>
        /// <returns>Number of cycles 3.</returns>
        protected virtual Double CalculateCycles3()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the number of cycles of length 4 in the network.
        /// </summary>
        /// <returns>Number of cycles 4.</returns>
        protected virtual Double CalculateCycles4()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the number of cycles of length 5 in the network.
        /// </summary>
        /// <returns>Number of cycles 5.</returns>
        protected virtual Double CalculateCycles5()
        {
            throw new NotImplementedException();
        }

        private bool calledEigens = false;
        private List<double> eigenValues = new List<double>();

        /// <summary>
        /// Calculates the eigenvalues of adjacency matrix of the network.
        /// </summary>
        /// <returns>List of eigenvalues.</returns>
        protected List<Double> CalculateEigenValues()
        {
            bool[,] m = Container.GetMatrix();

            EigenValueUtils eg = new EigenValueUtils();
            try
            {
                eigenValues = eg.CalculateEigenValue(m);
                calledEigens = true;
                return eigenValues;
            }
            catch (SystemException)
            {
                return new List<double>();
            }
        }

        protected List<Double> CalculateLaplacianEigenValues()
        {
            bool[,] m = Container.GetMatrix();
            double[,] lm = new double[m.GetLength(0), m.GetLength(1)];
            for (int i = 0; i < m.GetLength(0); ++i)
            {
                int connection_count = 0;
                for (int j = 0; j < m.GetLength(1); ++j)
                {
                    if (i == j)
                        continue;
                    if (m[i, j])
                    {
                        lm[i, j] = -1;
                        ++connection_count;
                    }
                    else
                    {
                        lm[i, j] = 0;
                    }

                }
                lm[i, i] = connection_count;
            }

            EigenValueUtils eg = new EigenValueUtils();
            try
            {
                return eg.CalculateEigenValue(lm);
            }
            catch (SystemException)
            {
                return new List<double>();
            }
        }

        /// <summary>
        /// Calculates distances between eigenvalues.
        /// </summary>
        /// <returns>(distance, count) pairs.</returns>
        protected SortedDictionary<Double, Double> CalculateEigenDistanceDistribution()
        {
            bool[,] m = Container.GetMatrix();

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

        /// <summary>
        /// Calculates the number of cycles of length 3 in the network using eigenvalues.
        /// </summary>
        /// <returns>Number of cycles 3.</returns>
        protected virtual Double CalculateCycles3Eigen()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the number of cycles of length 4 in the network using eigenvalues.
        /// </summary>
        /// <returns>Number of cycles 4.</returns>
        protected virtual Double CalculateCycles4Eigen()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates degrees of vertices in the network.
        /// </summary>
        /// <returns>(degree, count) pairs.</returns>
        protected virtual SortedDictionary<Double, Double> CalculateDegreeDistribution()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates clustering coefficients of vertices in the network.
        /// </summary>
        /// <returns>(clustering coefficient, count) pairs.</returns>
        protected virtual SortedDictionary<Double, Double> CalculateClusteringCoefficientDistribution()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates clustering coefficient for each vertex in the network.
        /// </summary>
        /// <returns>(vertex, coefficient) pairs.</returns>
        protected virtual SortedDictionary<Double, Double> CalculateClusteringCoefficientPerVertex()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates counts of connected components in the network.
        /// </summary>
        /// <returns>(order of connected component, count) pairs.</returns>
        protected virtual SortedDictionary<Double, Double> CalculateConnectedComponentDistribution()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates counts of subtrees in the network.
        /// </summary>
        /// <returns>(order of connected component, count) pairs.</returns>
        protected virtual SortedDictionary<Double, Double> CalculateSubtreeDistribution()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates counts of complete components in the network.
        /// </summary>
        /// <returns>(order of complete component, count) pairs.</returns>
        protected virtual SortedDictionary<Double, Double> CalculateCompleteComponentDistribution()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates minimal path lengths in the network.
        /// </summary>
        /// <returns>(minimal path length, count) pairs.</returns>
        protected virtual SortedDictionary<Double, Double> CalculateDistanceDistribution()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates counts of triangles by vertices in the network.
        /// </summary>
        /// <returns>(triangle count, count) pairs.</returns>
        protected virtual SortedDictionary<Double, Double> CalculateTriangleByVertexDistribution()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the counts of cycles in the network.
        /// </summary>
        /// <param name="lowBound">Minimal length of cycle.</param>
        /// <param name="hightBound">Maximal length of cycle.</param>
        /// <returns>(cycle length, count) pairs.</returns>
        protected virtual SortedDictionary<Double, Double> CalculateCycleDistribution(UInt32 lowBound, UInt32 hightBound)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the counts of cycles 3 in the network during evolution process.
        /// </summary>
        /// <param name="stepCount">Count of steps of evolution.</param>
        /// <param name="nu">Managment parameter.</param>
        /// <param name="permanentDistribution">Indicates if degree distribution must be permanent during evolution process.</param>
        /// <returns>(step, cycles 3 count)</returns>
        protected virtual SortedDictionary<Double, Double> CalculateCycles3Trajectory()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual List<Double> CalculateDegreeCentrality()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual List<Double> CalculateClosenessCentrality()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual List<Double> CalculateBetweennessCentrality()
        {
            throw new NotImplementedException();
        }

        protected virtual SortedDictionary<Double, Double> CalculateDr()
        {
            throw new NotImplementedException();
        }

        protected virtual SortedDictionary<Double, Double> CalculateActivePartModelA(bool b1, bool b2, bool b3)
        {
            throw new NotImplementedException();
        }

        protected virtual SortedDictionary<Double, Double> CalculateActivePartModelB()
        {
            throw new NotImplementedException();
        }
    }
}
