using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using RandomNumberGeneration;

namespace Core.Model
{
    public abstract class AbstractNetworkContainer : INetworkContainer
    {
        public abstract UInt32 Size { get; set; }

        public abstract void SetMatrix(ArrayList matrix);
        public abstract bool[,] GetMatrix();

        public virtual void SetNeighbourship(List<int> neighbours, int size)
        {
            Debug.Assert(size != 0);
            Debug.Assert(neighbours.Count() % 2 == 0);

            ArrayList matrix = new ArrayList();

            bool[,] n = new bool[size, size];
            for (int i = 0; i < neighbours.Count(); i += 2)
            {
                n[neighbours[i], neighbours[i + 1]] = true;
                n[neighbours[i + 1], neighbours[i]] = true;
            }

            for (int i = 0; i < size; ++i)
            {
                ArrayList tmp = new ArrayList();
                for (int j = 0; j < size; ++j)
                {
                    tmp.Add(n[i, j]);
                }
                matrix.Add(tmp);
            }
            SetMatrix(matrix);
        }

        public abstract List<KeyValuePair<int, int>> GetNeighbourship();

        #region Activation Extra Interface

        private BitArray activeNodes = null;

        public BitArray GetActiveStatuses() => activeNodes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        public virtual void SetActiveStatuses(BitArray act)
        {
            Debug.Assert(act.Count == Size);
            activeNodes = act;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void RandomActivating(Double p)
        {
            RNGCrypto rand = new RNGCrypto();
            activeNodes = new BitArray((Int32)Size, false);
            for (Int32 i = 0; i < Size; ++i)
            {
                if (rand.NextDouble() <= p)
                {
                    activeNodes[i] = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="a"></param>
        public void SetActiveStatus(int i, bool a)
        {
            Debug.Assert(activeNodes != null);
            Debug.Assert(i >= 0 && i < activeNodes.Count);
            activeNodes[i] = a;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool GetActiveStatus(int i)
        {
            Debug.Assert(activeNodes != null);
            Debug.Assert(i >= 0 && i < activeNodes.Count);
            return activeNodes[i];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetActiveNodesCount()
        {
            Debug.Assert(activeNodes != null);
            int c = 0;
            for (int i = 0; i < activeNodes.Count; ++i)
            {
                if (activeNodes[i])
                {
                    ++c;
                }
            }
            return c;
        }

        public bool DoesActiveNodeExist()
        {
            Debug.Assert(activeNodes != null);
            for (int i = 0; i < activeNodes.Count; ++i)
            {
                if (activeNodes[i])
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
