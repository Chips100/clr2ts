using System;
using System.Reflection;

namespace Clr2Ts.Transpiler.Configuration
{
    /// <summary>
    /// Attribute for classes representing configuration sections.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ConfigurationSectionAttribute : Attribute
    {
        /// <summary>
        /// Creates a <see cref="ConfigurationSectionAttribute"/>.
        /// </summary>
        /// <param name="sectionName">Name of the section that the decorated class represents.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="sectionName"/> is missing.</exception>
        public ConfigurationSectionAttribute(string sectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName)) throw new ArgumentException("SectionName cannot be empty.");

            SectionName = sectionName;
        }

        /// <summary>
        /// Gets the name of the section that the decorated type represents.
        /// </summary>
        public string SectionName { get; }

        /// <summary>
        /// Gets the name of the section that the specified type represents.
        /// </summary>
        /// <typeparam name="T">Type that represents a configuration section.</typeparam>
        /// <returns>The name of the section that the specified type represents.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the type does not have a <see cref="ConfigurationSectionAttribute"/>.</exception>
        public static string GetSectionName<T>()
        {
            var sectionAttribute = typeof(T).GetCustomAttribute<ConfigurationSectionAttribute>();
            if (sectionAttribute == null) throw new InvalidOperationException($"{nameof(ConfigurationSectionAttribute)} not used on type {typeof(T)}.");

            return sectionAttribute.SectionName;
        }
    }
}