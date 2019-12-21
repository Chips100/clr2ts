using System;

namespace Clr2Ts.EndToEnd.Targets.Attributes
{
    /// <summary>
    /// Example for a class-level attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ClassLevelAttribute: Attribute
    {
        /// <summary>
        /// Creates a ClassLevelAttribute.
        /// </summary>
        /// <param name="className">Some special name for the attributed class.</param>
        public ClassLevelAttribute(string className)
        {
            ClassName = className;
        }
        
        /// <summary>
        /// Gets some special name for the attributed class.
        /// </summary>
        public string ClassName { get; }
    }
}