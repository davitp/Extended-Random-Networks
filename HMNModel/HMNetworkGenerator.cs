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
using NetworkModel;
using RandomNumberGeneration;

namespace HMNModel
{
    class HMNetworkGenerator : AbstractNetworkGenerator
    {
        private NonHierarchicContainer container;

        public HMNetworkGenerator()
        {
            container = new NonHierarchicContainer();
            rand = new RNGCrypto();
            node = 0;
        }

        public override INetworkContainer Container
        {
            get { return container; }
            set { container = (NonHierarchicContainer)value; }
        }

        public override void RandomGeneration(Dictionary<GenerationParameter, object> genParam)
        {
            UInt32 numberOfVertices = Convert.ToUInt32(genParam[GenerationParameter.Vertices]);
            UInt32 zeroLevelNodesCount = Convert.ToUInt32(genParam[GenerationParameter.ZeroLevelNodesCount]);
            Double probability = Double.Parse(genParam[GenerationParameter.Probability].ToString(), CultureInfo.InvariantCulture);
            UInt32 blocksCount = Convert.ToUInt32(genParam[GenerationParameter.BlocksCount]);
            Double alpha = Double.Parse(genParam[GenerationParameter.Alpha].ToString(), CultureInfo.InvariantCulture);
            Boolean makeConnected = Convert.ToBoolean(genParam[GenerationParameter.MakeConnected]);

            Boolean pb = (probability < 0 || probability > 1);
            Boolean a = (alpha < 0 || alpha > 1);

            if (pb || a || !(IsPowerOfN(numberOfVertices/zeroLevelNodesCount, blocksCount)))
            {
                throw new InvalidGenerationParameters();
            }

            container.Size = numberOfVertices;
            Generate(numberOfVertices, zeroLevelNodesCount, blocksCount, probability, alpha, makeConnected);
        }

        private RNGCrypto rand;
        private UInt32 node { get; set; }

        private Boolean IsPowerOfN(UInt32 x, UInt32 n)
        {
            if (x == 1)
            {
                return true;
            }

            if (x % n != 0)
            {
                return false;
            }

            return IsPowerOfN(x / n, n);
        }

        private void GenerateFullGraph(Int32[] nodes)
        {
            for (UInt32 i = 0; i < nodes.Length - 1; ++i)
            {
                for (UInt32 j = i + 1; j < nodes.Length; ++j)
                {
                    container.AddConnection(nodes[i], nodes[j]);
                }
            }
        }

        private void AddTwoBlocksConnection(Double probability, UInt32[] firstBlock, UInt32[] secondBlock)
        {
            Boolean stop = false;

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

        private void AddTwoBlocksPropablyConnection(Double probability, UInt32[] firstBlock, UInt32[] secondBlock)
        {
            for (UInt32 i = 0; i < firstBlock.Length; ++i)
                for (UInt32 j = 0; j < secondBlock.Length; ++j)
                {
                    if (probability >= rand.NextDouble())
                    {
                        container.AddConnection(Convert.ToInt32(firstBlock[i]), Convert.ToInt32(secondBlock[j]));
                    }
                }
        }

        private List<UInt32[]> GetBlocksFromNetwork(UInt32 count, UInt32 size)
        {
            UInt32[] block = null;
            List<UInt32[]> Blocks = new List<UInt32[]>();
            for (UInt32 j = 1; j <= count; j++)
            {
                block = new UInt32[size];
                for (UInt32 i = 0; i < size; ++i)
                {
                    block[i] = node++;
                }
                Blocks.Add(block);
            }

            return Blocks;
        }

        private void Generate(UInt32 numberOfVertices, UInt32 zeroLevelNodesCount,
            UInt32 blocksCount, Double probability, Double alpha, Boolean makeConnected)
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
            UInt32 levelsCount = (UInt32)(Math.Log(numberOfVertices / zeroLevelNodesCount, blocksCount));

            while (levelsCount > 0)
            {
                List<UInt32[]> blocks = null;
                Double levelP = alpha * Math.Pow(probability, level);
                UInt32 size = Convert.ToUInt32(Math.Pow(blocksCount, level - 1) * zeroLevelNodesCount);
                node = 0;
                for (UInt32 i = 0; i < container.Size; i += blocksCount * size)
                {
                    blocks = GetBlocksFromNetwork(blocksCount, size);

                    for (Int32 j = 1; j < blocks.Count; ++j)
                    {
                        if (makeConnected)
                            AddTwoBlocksConnection(levelP, blocks[0], blocks[j]);
                        else
                            AddTwoBlocksPropablyConnection(levelP, blocks[0], blocks[j]);
                    }
                }

                --levelsCount;
                ++level;
            }
        }
    }
}
