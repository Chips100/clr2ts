using System;

namespace Clr2Ts.EndToEnd.Targets.Attributes
{
    /// <summary>
    /// Example custom attribute, similar to the ComponentModel DisplayAttribute.
    /// </summary>
    public sealed class MyDisplayAttribute : Attribute
    {
        /// <summary>
        /// Creates a MyDisplayAttribute.
        /// </summary>
        /// <param name="name">Display Name of the attributed member.</param>
        /// <param name="description">Description of the attributed member.</param>
        public MyDisplayAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Gets the display Name of the attributed member.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description of the attributed member.
        /// </summary>
        public string Description { get; }
    }
}