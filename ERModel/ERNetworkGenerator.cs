using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

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
    class ERNetworkGenerator : AbstractNetworkGenerator
    {
        private NonHierarchicContainer container;
     
        public ERNetworkGenerator()
        {
            container = new NonHierarchicContainer();
        }

        public override INetworkContainer Container
        {
            get { return container; }
            set { container = (NonHierarchicContainer)value; }
        }

        public override void RandomGeneration(Dictionary<GenerationParameter, object> genParam)
        {
            UInt32 numberOfVertices = Convert.ToUInt32(genParam[GenerationParameter.Vertices]);
            Double probability = Double.Parse(genParam[GenerationParameter.Probability].ToString(), CultureInfo.InvariantCulture);

            bool p = (probability < 0 || probability > 1);
            if (p)
                throw new InvalidGenerationParameters();
            
            container.Size = numberOfVertices;           
            FillValuesByProbability(probability);
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
