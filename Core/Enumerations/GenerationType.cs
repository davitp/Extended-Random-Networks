using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Enumerations
{
    /// <summary>
    /// Enumaration, used for indicating generation type for Research.
    /// </summary>
    public enum GenerationType
    {
        /// <summary>
        /// Indicates random generation for each realization of research.
        /// </summary>
        Random,

        /// <summary>
        /// Indicates generation (reading) from file.
        /// <note>Realization count for a research with this generation type must be 1.</note>
        /// </summary>
        Static
    }
}
