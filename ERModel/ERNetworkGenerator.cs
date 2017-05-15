using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.Model;
using Core.Utility;
using Core.Enumerations;
using Core.Exceptions;
using NetworkModel;
using RandomNumberGeneration;

namespace ERModel
{
    /// <summary>
    /// Implementation of generator of random network of Erdős-Rényi's model.
    /// </summary>
    class ERNetworkGenerator : INetworkGenerator
    {
        private NonHierarchicContainer container;
     
        public ERNetworkGenerator()
        {
            container = new NonHierarchicContainer();
        }

        public INetworkContainer Container
        {
            get { return container; }
            set { container = (NonHierarchicContainer)value; }
        }

        public void RandomGeneration(Dictionary<GenerationParameter, object> genParam)
        {
            UInt32 numberOfVertices = Convert.ToUInt32(genParam[GenerationParameter.Vertices]);
            Double probability = Convert.ToDouble(genParam[GenerationParameter.Probability]);

            bool p = (probability < 0 || probability > 1);
            if (p)
                throw new InvalidGenerationParameters();
            
            container.Size = numberOfVertices;           
            FillValuesByProbability(probability);
        }

        public void StaticGeneration(MatrixInfoToRead matrixInfo)
        {
            container.SetMatrix(matrixInfo.Matrix);
            if (matrixInfo.ActiveStates != null)
                container.SetActivStatuses(matrixInfo.ActiveStates);
        }

        private RNGCrypto rand = new RNGCrypto();

        private void FillValuesByProbability(double p)
        {            
            for (int i = 0; i < container.Size; ++i)
            {
                for (int j = i + 1; j < container.Size; ++j)
                {
                    double a = rand.NextDouble();
                    if (a < p)
                    {
                        container.AddConnection(i, j);
                    }
                }
            }
        }
    }
}
