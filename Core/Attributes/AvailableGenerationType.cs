using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.Enumerations;

namespace Core.Attributes
{
    /// <summary>
    /// Attribute for types derived from AbstractResearch type.
    /// GenerationType - type, which is available for current type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AvailableGenerationType : Attribute
    {
        public AvailableGenerationType(GenerationType generationType)
        {
            GenerationType = generationType;
        }

        public GenerationType GenerationType { get; private set; }
    }
}
