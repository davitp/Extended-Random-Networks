using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkModel
{
    public class ConnectedComponents
    {
        public NonHierarchicContainer component;
        public Dictionary<int, int> vertexmatching;
        public int count;
        public ConnectedComponents(NonHierarchicContainer component, Dictionary<int, int> vertexmatching, int count)
        {
            this.component = component;
            this.vertexmatching = vertexmatching;
            this.count = count;
        }
    }
}
