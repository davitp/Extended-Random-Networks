using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Attributes
{
    /// <summary>
    /// Attribute for ModelType (enum).
    /// FullName - user-friendly name for Model.
    /// Description - extended information about a Model.
    /// Implementation - the name of type, which implements a Model.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ModelTypeInfo : Attribute
    {
        public ModelTypeInfo(string fullName, string description, string implementation)
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
