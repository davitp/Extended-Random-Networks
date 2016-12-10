using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.Enumerations;

namespace Core.Attributes
{
    /// <summary>
    /// Attribute for types derived from AbstractResearch type.
    /// ModelType - Model, which is available for current type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AvailableModelType : Attribute
    {
        public AvailableModelType(ModelType modelType)
        {
            ModelType = modelType;
        }

        public ModelType ModelType { get; private set; }
    }
}
