using System;
using System.Collections.Generic;
using System.Text;

namespace Genode
{
    /// <summary>
    /// Specifies to ignore the field or property to being instantiated along with the instantiable object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field |
                    AttributeTargets.Property)]
    public class DontInstantiateAttribute : Attribute
    {
    }
}
