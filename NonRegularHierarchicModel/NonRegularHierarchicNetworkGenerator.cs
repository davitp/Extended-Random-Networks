using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Globalization;

using Core.Enumerations;
using Core.Model;
using Core.Utility;
using Core.Exceptions;
using RandomNumberGeneration;

namespace NonRegularHierarchicModel
{
    /// <summary>
    /// Implementation of non regularly branching block-hierarchic network's generator.
    /// </summary>
    class NonRegularHierarchicNetworkGenerator : AbstractNetworkGenerator
    {
        /// <summary>
        /// Container with network of specified model (non regular block-hierarchic).
        /// </summary>
        private NonRegularHierarchicNetworkContainer container;

        public NonRegularHierarchicNetworkGenerator()
        {
            container = new NonRegularHierarchicNetworkContainer();
        }

        public override INetworkContainer Container
        {
            get { return container; }
            set { container = (NonRegularHierarchicNetworkContainer)value; }
        }

        public override void RandomGeneration(Dictionary<GenerationParameter, object> genParam)
        {
            UInt32 vertices = Convert.ToUInt32(genParam[GenerationParameter.Vertices]);
            UInt32 branchingIndex = Convert.ToUInt32(genParam[GenerationParameter.BranchingIndex]);
            Double mu = Double.Parse(genParam[GenerationParameter.Mu].ToString(), CultureInfo.InvariantCulture);

            if (mu < 0)
                throw new InvalidGenerationParameters();

            container.Vertices = vertices;
            container.BranchIndex = branchingIndex;            
            container.HierarchicTree = GenerateByVertices(mu);
        }

        public override void StaticGeneration(NetworkInfoToRead info)
        {
            Debug.Assert(info.Branches != null);
            container.SetBranches(info.Branches);
            base.StaticGeneration(info);
        }

        private RNGCrypto rand = new RNGCrypto();
        private const int ARRAY_MAX_SIZE = 2000000000;

        /// <summary>
        /// Dynamically generates a non-regular block-hierarchical network by the number of vertices. 
        /// </summary>
        /// <param name="mu">Density parameter.</param>
        /// <returns>Two-dimensional array of bits which define the connectedness
        /// of clusters between each other.</returns>
        private BitArray[][] GenerateByVertices(Double mu)
        {
            List<List<UInt32>> branchList = GenerateBranchList();
            container.Level = (UInt32)branchList.Count;
            container.Branches = new UInt32[container.Level][];
            BitArray[][] treeMatrix = new BitArray[container.Level][];

            for (int i = 0; i < container.Level; ++i)
            {
                int levelForTree = (int)container.Level - 1 - i;
                int levelVertexCount = branchList[levelForTree].Count;
                container.Branches[i] = new UInt32[levelVertexCount];
                long dataLength = 0;
                for (int j = 0; j < levelVertexCount; ++j)
                {
                    UInt32 nodeDataLength = branchList[levelForTree][j];
                    container.Branches[i][j] = nodeDataLength;
                    dataLength += nodeDataLength * (nodeDataLength - 1) / 2;
                }

                GenerateTreeMatrixForLevel(treeMatrix, i, dataLength);
            }

            GenerateData(treeMatrix, mu);
            return treeMatrix;
        }

        /// <summary>
        /// Creates the connectivity tree of the network.
        /// </summary>
        /// <returns>Two-dimensional List that contains the connectivity tree of the network.</returns>
        private List<List<UInt32>> GenerateBranchList()
        {
            List<List<UInt32>> branchList = new List<List<UInt32>>();
            uint left = container.Size;

            while (left != 1)
            {
                List<UInt32> list = new List<UInt32>();
                while (left != 0)
                {
                    UInt32 randomInt = (UInt32)rand.Next(1, (int)container.BranchIndex + 1);
                    if (randomInt > left)
                    {
                        randomInt = left;
                    }
                    left -= randomInt;
                    list.Add(randomInt);
                }
                branchList.Add(list);
                left = (uint)branchList[branchList.Count - 1].Count;
            }

            return branchList;
        }

        /// <summary>
        /// Generates the BitArray that desribes the connectedness of cluster on the given level. 
        /// </summary>
        /// <param name="treeMatrix">Two-dimensional array of bits which define the connectedness
        /// of clusters between each other.</param>
        /// <param name="level">The level of cluster.</param>
        /// <param name="dataLength">The lenght of data.</param>
        private void GenerateTreeMatrixForLevel(BitArray[][] treeMatrix, 
            int level, 
            long dataLength)
        {
            int arrCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(dataLength) / ARRAY_MAX_SIZE));
            treeMatrix[level] = new BitArray[arrCount];
            if (arrCount > 0)
            {
                int t;
                for (t = 0; t < arrCount - 1; ++t)
                {
                    treeMatrix[level][t] = new BitArray(ARRAY_MAX_SIZE);
                }
                treeMatrix[level][t] = new BitArray(Convert.ToInt32(dataLength - (arrCount - 1) * ARRAY_MAX_SIZE));
            }
        }

        /// <summary>
        /// Fills values in the two-dimensional array of bits which define the connectedness
        /// of clusters between each other.
        /// </summary>
        /// <param name="treeMatrix">Two-dimensional array of bits which define the connectedness
        /// of clusters between each other.</param>
        /// <param name="mu">Density parameter.</param>
        private void GenerateData(BitArray[][] treeMatrix, Double mu)
        {
            for (int currentLevel = 0; currentLevel < container.Level; ++currentLevel)
            {
                if (treeMatrix[currentLevel].Length > 0)
                {
                    uint branchSize = container.Branches[currentLevel][0];
                    int counter = 0, nodeNumber = 0;
                    for (int i = 0; i < treeMatrix[currentLevel].Length; i++)
                    {
                        for (int j = 0; j < treeMatrix[currentLevel][i].Length; j++)
                        {
                            if (counter == (branchSize * (branchSize - 1) / 2))
                            {
                                ++nodeNumber;
                                counter = 0;
                                branchSize = container.Branches[currentLevel][nodeNumber];
                            }
                            double k = rand.NextDouble();
                            if (k <= (1 / Math.Pow(container.CountLeaves(currentLevel, nodeNumber), mu)))
                            {
                                treeMatrix[currentLevel][i][j] = true;
                            }
                            else
                            {
                                treeMatrix[currentLevel][i][j] = false;
                            }
                            ++counter;
                        }
                    }
                }
            }
        }
    }
}
