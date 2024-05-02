﻿using System;
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
        /// Returns the underlying type if a nullable type is specified,
        /// otherwise the type itself.
        /// </summary>
        /// <param name="type">Type that might be nullable.</param>
        /// <returns>The underlying type, if it is nullable; otherwise the type itself.</returns>
        public static Type UnwrapNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        /// <summary>
        /// Gets all base types that the current type is derived from (including <see cref="object"/>).
        /// </summary>
        /// <param name="type">Current type.</param>
        /// <returns>A sequence with the base types that the current type is derived from.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="type"/> is null.</exception>
        public static IEnumerable<Type> GetBaseTypes(this Type type) // Wrap iterator in other method to enforce eager parameter null check.
        {
            return GetBaseTypesIterator(type ?? throw new ArgumentNullException(nameof(type)));
        }

        private static IEnumerable<Type> GetBaseTypesIterator(Type type)
        {
            Type baseType = type;
            while ((baseType = baseType.BaseType) != null)
                yield return baseType;
        }

        /// <summary>
        /// Gets all interfaces that are implemented by the specified type,
        /// not including interfaces that have been implented by its base types.
        /// </summary>
        /// <param name="type">The type for which the interfaces should be determined.</param>
        /// <returns>A sequence of the interfaces that are implemented by the specified type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="type"/> is null.</exception>
        public static IEnumerable<Type> GetSelfImplementedInterfaces(this Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            var baseInterfaces = type.BaseType?.GetInterfaces() ?? Enumerable.Empty<Type>();
            return type.GetInterfaces().Except(baseInterfaces);
        }

        /// <summary>
        /// Gets the type of the elements that the specified collection type contains.
        /// </summary>
        /// <param name="collectionType">The type of the collection to inspect.</param>
        /// <returns>The type of the elements contained by the collection type; or null if it does not represent a collection.</returns>
        public static Type GetCollectionElementType(this Type collectionType)
        {
            if (collectionType is null)
                throw new ArgumentNullException(nameof(collectionType));

            // Check if the type is the IEnumerable interface itself,
            // otherwise look for the implemented IEnumerable interface.
            var enumerableInterface = GetSelfEnumerableType(collectionType)
                                   ?? GetImplementedEnumerableType(collectionType);

            return enumerableInterface?.GetGenericArguments().Single();
        }

        private static Type GetSelfEnumerableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                       ? type
                       : null;
        }

        private static Type GetImplementedEnumerableType(Type type)
        {
            return type.GetInterface(typeof(IEnumerable<>).Name);
        }

        /// <summary>
        /// Gets the name of a type with its generic parameters
        /// written as <code>&lt;T1, T2, ...&gt;</code> (corresponding to the formal declaration).
        /// </summary>
        /// <param name="type">Type for which the name should be retreived.</param>
        /// <param name="typeParameterSuffix">Optional suffix that should be added to type parameters.</param>
        /// <param name="typeOverrides">Optional if the type parameter should be overridden.</param>
        /// <returns>The name of the type with its generic type parameters; or simply its name if the type is not generic.</returns>
        public static string GetNameWithGenericTypeParameters(
            this Type type,
            Func<Type, string> typeParameterSuffix = null,
            Func<Type, string> typeOverrides = null)
        {
            return type.GetNameWithReplacedTypeParameters(
                t => $@"<{string.Join(", ",
                                      t.GetGenericArguments().Select(x => typeOverrides?.Invoke(x) ?? x.Name + typeParameterSuffix?.Invoke(x)))}>"
            );
        }

        /// <summary>
        /// Gets the name of a type without its generic parameters.
        /// </summary>
        /// <param name="type">Type for which the name should be retreived.</param>
        /// <returns>The name of the type without its generic type parameters; or simply its name if the type is not generic.</returns>
        public static string GetNameWithoutGenericTypeParameters(this Type type)
        {
            return type.GetNameWithReplacedTypeParameters(_ => string.Empty);
        }

        private static string GetNameWithReplacedTypeParameters(this Type type, Func<Type, string> replacement)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (replacement == null)
                throw new ArgumentNullException(nameof(replacement));

            if (!type.IsGenericType)
                return type.Name;

            var genericDefinition = type.GetGenericTypeDefinition();

            return genericDefinition.Name.Replace(
                $"`{genericDefinition.GetGenericArguments().Length}",
                replacement(genericDefinition)
            );
        }
    }
}