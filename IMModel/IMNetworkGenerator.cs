using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.Enumerations;
using Core.Model;
using Core.Utility;
using Core.Exceptions;
using NetworkModel;
using RandomNumberGeneration;

namespace IMModel
{
    class IMNetworkGenerator : INetworkGenerator
    {
        private NonHierarchicContainer container;

        public IMNetworkGenerator()
        {
            container = new NonHierarchicContainer();
            rand = new RNGCrypto();
            node = 0;
        }

        public INetworkContainer Container
        {
            get { return container; }
            set { container = (NonHierarchicContainer)value; }
        }

        public void RandomGeneration(Dictionary<GenerationParameter, object> genParam)
        {
            UInt32 numberOfVertices = Convert.ToUInt32(genParam[GenerationParameter.Vertices]);
            UInt32 zeroLevelNodesCount = Convert.ToUInt32(genParam[GenerationParameter.ZeroLevelNodesCount]);
            Double probability = Convert.ToDouble(genParam[GenerationParameter.Probability]);
            UInt32 blocksCount = Convert.ToUInt32(genParam[GenerationParameter.BlocksCount]);
            Double alpha = Convert.ToDouble(genParam[GenerationParameter.Alpha]);
     
            bool pb = (probability < 0 || probability > 1);
            bool a = (alpha < 0 || alpha > 1);

            if (pb || blocksCount != 2 || a || !(IsPowerOfTwo(numberOfVertices/zeroLevelNodesCount)))
                throw new InvalidGenerationParameters();

            container.Size = numberOfVertices;
            Generate(numberOfVertices, zeroLevelNodesCount, blocksCount, probability, alpha);
        }

        public void StaticGeneration(MatrixInfoToRead matrixInfo)
        {
            container.SetMatrix(matrixInfo.Matrix);
        }

        private RNGCrypto rand;
        private uint node { get; set; }

        private bool IsPowerOfTwo(UInt32 x)
        {
            return (x != 0) && ((x & (x - 1)) == 0);
        }

        private void GenerateFullGraph(Int32[] nodes)
        {
            for (uint i = 0; i < nodes.Length - 1; ++i)
            {
                for (uint j = i + 1; j < nodes.Length; ++j)
                {
                    container.AddConnection(nodes[i], nodes[j]);
                }
            }
        }

        private void TwoBlocksProbablyConnection(double probability, UInt32[] firstBlock, UInt32[] secondBlock)
        {
            bool stop = false;

            while (!stop)
            {
                for (UInt32 i = 0; i < firstBlock.Length; ++i)
                    for (UInt32 j = 0; j < secondBlock.Length; ++j)
                    {
                        if (probability >= rand.NextDouble())
                        {
                            container.AddConnection(Convert.ToInt32(firstBlock[i]), Convert.ToInt32(secondBlock[j]));
                            stop = true;
                        }
                    }
            }
        }

        private UInt32[] GetTwoBlocksFromContainer(UInt32 startIndex, UInt32 size, out UInt32[] secondBlock)
        {
            UInt32[] firstBlock = new UInt32[size];
            secondBlock = new UInt32[size];
            for (UInt32 i = 0; i < 2 * size; ++i)
            {
                if (i < size)
                    firstBlock[i] = ++node;
                else
                    secondBlock[i - size] = ++node;
            }

            return firstBlock;
        }

        private void Generate(UInt32 numberOfVertices, UInt32 zeroLevelNodesCount,
            uint blocksCount, double probability, Double alpha)
        {
            Int32 fullGraphNodesCount = Convert.ToInt32(zeroLevelNodesCount);
            for (Int32 i = 0; i < container.Size; i += fullGraphNodesCount)
            {
                Int32[] fullgraphNodesArray = new Int32[fullGraphNodesCount];
                UInt32 k = 0;
                for (Int32 j = i; j < i + fullGraphNodesCount; ++j)
                {
                    fullgraphNodesArray[k++] = j;
                }

                GenerateFullGraph(fullgraphNodesArray);
            }

            UInt32 level = 1;
            double levelsCount = Math.Log(numberOfVertices / zeroLevelNodesCount, blocksCount);

            while (levelsCount > 0)
            {
                UInt32[] secondBlock;
                double levelP = alpha * Math.Pow(probability, level);
                node = 0;
                for (UInt32 i = 0; i < container.Size; i += 2 * Convert.ToUInt32(Math.Pow(2, level - 1) * zeroLevelNodesCount))
                {

                    TwoBlocksProbablyConnection(levelP,
                                                GetTwoBlocksFromContainer(i, 
                                                                          Convert.ToUInt32(Math.Pow(2, level-1) * zeroLevelNodesCount),
                                                                          out secondBlock),
                                                secondBlock);
                }

                --levelsCount;
                ++level;
            }
        }
    }
}
