using System;
using System.Collections.Generic;
using System.Linq;

namespace Clr2Ts.Transpiler.Extensions
{
    /// <summary>
    /// Defines extension methods for instances of <see cref="Type"/>.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets all base types that the current type is derived from (including <see cref="object"/>).
        /// </summary>
        /// <param name="type">Current type.</param>
        /// <returns>A sequence with the base types that the current type is derived from.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="type"/> is null.</exception>
        public static IEnumerable<Type> GetBaseTypes(this Type type) 
            => GetBaseTypesIterator(type ?? throw new ArgumentNullException(nameof(type)));

        private static IEnumerable<Type> GetBaseTypesIterator(Type type)
        {
            Type baseType = type;
            while ((baseType = baseType.BaseType) != null) yield return baseType;
        }

        /// <summary>
        /// Gets the name of a type with its generic parameters
        /// written as <code>&lt;T1, T2, ...&gt;</code> (corresponding to the formal declaration).
        /// </summary>
        /// <param name="type">Type for which the name should be retreived.</param>
        /// <returns>The name of the type with its generic type parameters; or simply its name if the type is not generic.</returns>
        public static string GetNameWithGenericTypeParameters(this Type type)
            => type.GetNameWithReplacedTypeParameters(t => $"<{ string.Join(", ", t.GetGenericArguments().Select(x => x.Name)) }>");


        /// <summary>
        /// Gets the name of a type without its generic parameters.
        /// </summary>
        /// <param name="type">Type for which the name should be retreived.</param>
        /// <returns>The name of the type without its generic type parameters; or simply its name if the type is not generic.</returns>
        public static string GetNameWithoutGenericTypeParameters(this Type type)
            => type.GetNameWithReplacedTypeParameters(_ => string.Empty);

        private static string GetNameWithReplacedTypeParameters(this Type type, Func<Type, string> replacement)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (replacement == null) throw new ArgumentNullException(nameof(replacement));

            if (!type.IsGenericType) return type.Name;

            var genericDefinition = type.GetGenericTypeDefinition();

            return genericDefinition.Name.Replace(
                $"`{genericDefinition.GetGenericArguments().Length}", 
                replacement(genericDefinition));
        }
    }
}