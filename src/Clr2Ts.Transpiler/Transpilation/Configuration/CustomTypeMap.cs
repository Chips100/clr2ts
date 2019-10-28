using Clr2Ts.Transpiler.Extensions;
using System;

namespace Clr2Ts.Transpiler.Transpilation.Configuration
{
    /// <summary>
    /// Represents a mapping from a .NET type to a custom TypeScript type.
    /// </summary>
    public sealed class CustomTypeMap
    {
        /// <summary>
        /// Creates a custom type map.
        /// </summary>
        /// <param name="type">Name of the .NET type that is mapped to a custom TypeScript type.</param>
        /// <param name="name">Name of the TypeScript type.</param>
        /// <param name="source">Source from which the TypeScript type is imported.</param>
        public CustomTypeMap(string type, string name, string source)
        {
            Type = type;
            Name = name;
            Source = source;
        }

        /// <summary>
        /// Gets the name of the .NET type that is mapped to a custom TypeScript type.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets the name of the TypeScript type.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the source from which the TypeScript type is imported.
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// Determines if this custom type map maps the specified .NET type.
        /// </summary>
        /// <param name="type">Type that should be checked if it is covered by this map.</param>
        /// <returns>True, if the specified type is covered by this map; otherwise false.</returns>
        public bool MapsType(Type type)
        {
            var typeName = type.GetNameWithoutGenericTypeParameters();

            // Allow specifying the map via simple type name
            // or fully qualified with namespace.
            return Type == typeName || Type == $"{type.Namespace}.{typeName}";
        }

        /// <summary>
        /// Creates a set of dependencies that references the mapped type.
        /// </summary>
        /// <returns>A set of dependencies that references the mapped type.</returns>
        public CodeDependencies CreateImportDependency()
        {
            // Source is optional; might be an ambient type (like "any").
            if (string.IsNullOrWhiteSpace(Source)) return CodeDependencies.Empty;

            // Construct dependencies referencing the mapped type.
            return CodeDependencies.FromImports(new[] { new Import(Name, Source) });
        }
    }
}