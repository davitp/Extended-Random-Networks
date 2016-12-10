using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.Enumerations;

namespace Core.Attributes
{
    /// <summary>
    /// Attribute for types derived from AbstractNetworkModel type.
    /// Parameter - a Generation Parameter, which is required for generation of a network
    /// of current type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequiredGenerationParameter : Attribute
    {
        public RequiredGenerationParameter(GenerationParameter parameter)
        {
            Parameter = parameter;
        }

        public GenerationParameter Parameter { get; private set; }
    }
}
