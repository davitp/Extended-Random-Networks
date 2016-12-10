using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.Enumerations;

namespace Core.Attributes
{
    /// <summary>
    /// Attribute for types derived from AbstractResearch type.
    /// Parameter - a Research Parameter, which is required for running of a Research 
    /// of a current type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequiredResearchParameter : Attribute
    {
        public RequiredResearchParameter(ResearchParameter parameter)
        {
            Parameter = parameter;
        }

        public ResearchParameter Parameter { get; private set; }
    }
}
