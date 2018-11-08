using System;
using System.Collections.Generic;

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
    }
}