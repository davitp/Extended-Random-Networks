using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Core.Enumerations;
using Core.Model;
using Core.Utility;
using Core.Exceptions;
using RandomNumberGeneration;

namespace RegularHierarchicModel
{
    /// <summary>
    /// Implementation of regularly branching block-hierarchic network's generator.
    /// </summary>
    class RegularHierarchicNetworkGenerator : AbstractNetworkGenerator
    {
        /// <summary>
        /// Container with network of specified model (regular block-hierarchic).
        /// </summary>
        private RegularHierarchicNetworkContainer container;

        public RegularHierarchicNetworkGenerator(ContainerMode mode) : base(mode)
        {
            container = new RegularHierarchicNetworkContainer(mode);
        }

        public override INetworkContainer Container
        {
            get { return container; }
            set { container = (RegularHierarchicNetworkContainer)value; }
        }

        public override void RandomGeneration(Dictionary<GenerationParameter, object> genParam)
        {
            UInt32 branchingIndex = Convert.ToUInt32(genParam[GenerationParameter.BranchingIndex]);
            UInt32 level = Convert.ToUInt32(genParam[GenerationParameter.Level]);
            Double mu = Double.Parse(genParam[GenerationParameter.Mu].ToString(), CultureInfo.InvariantCulture);

            if (mu < 0)
                throw new InvalidGenerationParameters();

            container.BranchingIndex = branchingIndex;
            container.Level = level;
            container.HierarchicTree = GenerateTree(branchingIndex, level, mu);
        }

        private RNGCrypto rand = new RNGCrypto();
        private const int ARRAY_MAX_SIZE = 2000000000;        

        /// <summary>
        /// Recursively creates a block-hierarchic tree.
        /// </summary>
        /// <note>Data is initializing and generating started from root.</note>
        private BitArray[][] GenerateTree(UInt32 branchingIndex, 
            UInt32 level, 
            Double mu)
        {
            BitArray[][] treeMatrix = new BitArray[level][];

            uint nodeDataLength = (branchingIndex - 1) * branchingIndex / 2;
            long levelDataLength;
            int arrayCountForLevel;
            for (UInt32 i = level; i > 0; --i)
            {                
                levelDataLength = Convert.ToInt64(Math.Pow(branchingIndex, level - i) * nodeDataLength);
                arrayCountForLevel = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(levelDataLength) / ARRAY_MAX_SIZE));

                treeMatrix[level - i] = new BitArray[arrayCountForLevel];
                int j;
                for (j = 0; j < arrayCountForLevel - 1; j++)
                {
                    treeMatrix[level - i][j] = new BitArray(ARRAY_MAX_SIZE);
                }
                treeMatrix[level - i][j] = new BitArray(Convert.ToInt32(levelDataLength -
                    (arrayCountForLevel - 1) * ARRAY_MAX_SIZE));

                GenerateData(treeMatrix, i, branchingIndex, level, mu);
            }

            return treeMatrix;
        }
        
        /// <summary>
        /// Generates random data for current level of block-hierarchic tree.
        /// </summary>
        /// <node>Current level must be initialized.</node>
        private void GenerateData(BitArray[][] treeMatrix, 
            UInt32 currentLevel, 
            UInt32 branchingIndex, 
            UInt32 maxLevel, 
            Double mu)
        {
            for (int i = 0; i < treeMatrix[maxLevel - currentLevel].Length; i++)
            {
                for (int j = 0; j < treeMatrix[maxLevel - currentLevel][i].Length; j++)
                {
                    double k = rand.NextDouble();
                    if (k <= (1 / Math.Pow(branchingIndex, currentLevel * mu)))
                    {
                        treeMatrix[maxLevel - currentLevel][i][j] = true;
                    }
                    else
                    {
                        treeMatrix[maxLevel - currentLevel][i][j] = false;
                    }
                }
            }
        }
    }
}
