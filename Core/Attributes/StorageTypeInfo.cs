using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Attributes
{
    /// <summary>
    /// Attribute for StorageType (enum).
    /// Description - extended information about a Storage.
    /// Implementation - the name of type, which implements a Storage.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class StorageTypeInfo : Attribute
    {
        public StorageTypeInfo(string description, string implementation)
        {
            Description = description;
            Implementation = implementation;
        }

        public string Description { get; private set; }
        public string Implementation { get; private set; }
    }
}
