using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Attributes
{
    /// <summary>
    /// Attribute for ResearchType (enum).
    /// FullName - user-friendly name for Research.
    /// Description - extended information about a Research.
    /// Implementation - the name of type, which implements a Research.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ResearchTypeInfo : Attribute
    {
        public ResearchTypeInfo(string fullName, string description, string implementation)
        {
            FullName = fullName;
            Description = description;
            Implementation = implementation;
        }

        public string FullName { get; private set; }
        public string Description { get; private set; }
        public string Implementation { get; private set; }
    }
}
