using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clr2Ts.Transpiler.Transpilation.TypeScript
{
    /// <summary>
    /// Defines extension methods for handling types in the context of a TypeScript transpilation.
    /// </summary>
    public static class TypeHandlingExtensions
    {
        // Maps built-in types from .NET to built-in types from TypeScript.
        private static readonly IDictionary<Type, string> BuiltInTypes = new Dictionary<Type, string>
        {
            { typeof(byte), "number" },
            { typeof(short), "number" },
            { typeof(int), "number" },
            { typeof(long), "number" },
            { typeof(float), "number" },
            { typeof(double), "number" },
            { typeof(decimal), "number" },
            { typeof(bool), "boolean" },
            { typeof(DateTime), "Date" },
            { typeof(string), "string" },
            { typeof(Guid), "string" }, // special case: no real Guid type in TypeScript.
            { typeof(object), "any" }
        };

        /// <summary>
        /// Gets all properties of this type that should be transpiled.
        /// </summary>
        /// <param name="type">Type with the properties for transpilation.</param>
        /// <returns>A sequence with the properties of this type that should be transpiled.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is null.</exception>
        public static IEnumerable<PropertyInfo> GetPropertiesForTranspilation(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            // For now, just call GetProperties with the default parameters.
            return type.GetProperties();
        }

        /// <summary>
        /// Gets the dependencies that are required to build this type in a TypeScript definition.
        /// </summary>
        /// <param name="type">Type for which the dependencies should be detected.</param>
        /// <returns>A sequence with the types that are dependencies of this type.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is null.</exception>
        public static IEnumerable<Type> GetDependencies(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type
                .GetPropertiesForTranspilation()
                .Select(p => p.PropertyType.Reduce())
                .Where(t => !BuiltInTypes.ContainsKey(t));
        }

        /// <summary>
        /// Gets the name that should be used for the TypeScript version of this type.
        /// </summary>
        /// <param name="type">Type for which the TypeScript name should be determined.</param>
        /// <returns>A string with the name that should be used for the TypeScript version of this type.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is null.</exception>
        public static string ToTypeScriptName(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            
            // Look at the core type (e.g. ignore Nullable wrapping)
            // and consider translating to a built-in type.
            type = type.Reduce();
            if (BuiltInTypes.TryGetValue(type, out var builtInName)) return builtInName;

            // Use the original name if not a built-in type.
            return type.Name;
        }

        /// <summary>
        /// Reduces this type reference to its core type, e.g. unwraps nullable types or sequences.
        /// </summary>
        /// <param name="type">Type reference to reduce to its core type.</param>
        /// <returns>The core type of this type reference.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is null.</exception>
        public static Type Reduce(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            // Unwrap nullable type, if applicable.
            type = Nullable.GetUnderlyingType(type) ?? type;
            // TODO: sequences.
            return type;
        }
    }
}