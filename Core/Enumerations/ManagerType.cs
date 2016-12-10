using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.Attributes;

namespace Core.Enumerations
{
    /// <summary>
    /// Enumaration, used for indicating the manager type for Research.
    /// Uses Attribute ManagerTypeInfo for storing metadata about every model.
    /// </summary>
    public enum ManagerType
    {
        [ManagerTypeInfo("Local manager",
            "...",
            "Manager.LocalEnsembleManager, Manager")]
        Local,

        [ManagerTypeInfo("WCF distributed manager",
            "...",
            "Manager.WCFDistributedEnsembleManager, Manager")]
        WCFDistributed
    }
}
