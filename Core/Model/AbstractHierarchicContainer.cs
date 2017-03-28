using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Model
{
    /// <summary>
    /// Abstract class presenting container of hierarchic type.
    /// </summary>
    public abstract class AbstractHierarchicContainer : AbstractContainer
    {
        /// <summary>
        /// Gets branches for the graph.
        /// </summary>
        /// <returns>Branches by levels.</returns>
        public abstract UInt32[][] GetBranches();
    }
}
