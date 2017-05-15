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

namespace BAModel
{
    /// <summary>
    /// Implementation of generator of random network of Baraba´si-Albert's model.
    /// </summary>
    class BANetworkGenerator : INetworkGenerator
    {
        private NonHierarchicContainer container;
        private NonHierarchicContainer initialcontainer;

        public BANetworkGenerator()
        {
            container = new NonHierarchicContainer();
            initialcontainer = new NonHierarchicContainer();
        }

        public INetworkContainer Container
        {
            get { return container; }
            set { container = (NonHierarchicContainer)value; }
        }

        public void RandomGeneration(Dictionary<GenerationParameter, object> genParam)
        {
            UInt32 numberOfVertices = Convert.ToUInt32(genParam[GenerationParameter.Vertices]);
            UInt32 edges = Convert.ToUInt32(genParam[GenerationParameter.Edges]);
            Double probability = Convert.ToDouble(genParam[GenerationParameter.Probability]);
            UInt32 stepCount = Convert.ToUInt32(genParam[GenerationParameter.StepCount]);

            bool pb = (probability < 0 || probability > 1);
            bool ev = (edges > numberOfVertices);
            if (pb || ev)
                throw new InvalidGenerationParameters();

            container.Size = numberOfVertices;
            initialcontainer.Size = numberOfVertices;
            Generate(stepCount, probability, edges);
        }
        
        public void StaticGeneration(MatrixInfoToRead matrixInfo)
        {
            container.SetMatrix(matrixInfo.Matrix);
            if (matrixInfo.ActiveStates != null)
                container.SetActivStatuses(matrixInfo.ActiveStates);
        }

        private RNGCrypto rand = new RNGCrypto();

        private void Generate(uint stepCount, double probability, uint edges)
        {
            GenerateInitialGraph(probability);
            container = initialcontainer;

            while (stepCount > 0)
            {
                double[] probabilyArray = container.CountProbabilities();
                container.AddVertex();
                container.RefreshNeighbourships(MakeGenerationStep(probabilyArray, edges));
                --stepCount;
            }
        }

        private void GenerateInitialGraph(double probability)
        {
            for(int i = 0; i < container.Size; ++i)
                for(int j = i + 1; j < container.Size; ++j)
                {
                    if (rand.NextDouble() < probability)
                        initialcontainer.AddConnection(i, j);
                }
        }

        private bool[] MakeGenerationStep(double[] probabilityArray, uint edges)
        {
            Dictionary<int, double> resultDic = new Dictionary<int, double>();
            int count = 0;
            for (int i = 0; i < probabilityArray.Length; ++i)
                resultDic.Add(i, probabilityArray[i] - rand.NextDouble());
          
            var items = from pair in resultDic
                        orderby pair.Value descending
                        select pair;
            
            bool[] result = new bool[container.Neighbourship.Count];
            foreach (var item in items)
            {
                if (count < edges)
                {
                    result[item.Key] = true;
                    count++;
                }
            }

            return result;
        }
    }
}
