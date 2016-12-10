using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Attributes
{
    /// <summary>
    /// Attribute for ManagerType (enum).
    /// FullName - user-friendly name for Manager.
    /// Description - extended information about a Manager.
    /// Implementation - the name of type, which implements a Manager.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ManagerTypeInfo : Attribute
    {
        public ManagerTypeInfo(string fullName, string description, string implementation)
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
