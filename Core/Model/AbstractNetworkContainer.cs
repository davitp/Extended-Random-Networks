using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Core.Model
{
    public abstract class AbstractNetworkContainer : INetworkContainer
    {
        public abstract UInt32 Size { get; set; }

        public abstract void SetMatrix(ArrayList matrix);
        public abstract bool[,] GetMatrix();
        public abstract List<KeyValuePair<int, int>> GetNeighbourship();

        private BitArray activeNodes = null;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        public void SetActivStatuses(BitArray act)
        {
            Debug.Assert(act.Count == Size);
            activeNodes = act;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="a"></param>
        public void SetActiveStatus(int i, bool a)
        {
            Debug.Assert(activeNodes != null);
            Debug.Assert(i > 0 && i < activeNodes.Count);
            if (a != activeNodes[i])
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
            Debug.Assert(i > 0 && i < activeNodes.Count);
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
                ++c;
            return c;
        }
    }
}
