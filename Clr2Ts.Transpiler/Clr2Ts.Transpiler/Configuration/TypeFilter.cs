using System;
using System.Linq;
using Clr2Ts.Transpiler.Extensions;

namespace Clr2Ts.Transpiler.Configuration
{
    /// <summary>
    /// Defines predicates to filter types based on different conditions.
    /// </summary>
    public abstract class TypeFilter
    {
        // Private constructor to enforce usage of factory methods.
        private TypeFilter() { }

        /// <summary>
        /// Evalutes if the specified type satisfies the condition of this filter.
        /// </summary>
        /// <param name="type">Type to test with this filter.</param>
        /// <returns>True, if the type satisfies the condition of this filter; otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="type"/> is null.</exception>
        public bool IsMatch(Type type)
            => IsMatchImplementation(type ?? throw new ArgumentNullException(nameof(type)));

        protected abstract bool IsMatchImplementation(Type type);
        
        /// <summary>
        /// Creates a filter that matches types that are a subtype of a specific type (interface or base class).
        /// </summary>
        /// <param name="baseTypeName">Type of which the matched types should be a subtype.</param>
        /// <returns>A filter that tests for the specified subtype relation.</returns>
        public static TypeFilter SubTypeOf(string baseTypeName) => new SubTypeOfTypeFilter(baseTypeName);


        private class SubTypeOfTypeFilter : TypeFilter
        {
            private readonly string _baseTypeName;

            public SubTypeOfTypeFilter(string baseTypeName)
            {
                if (string.IsNullOrWhiteSpace(baseTypeName)) throw new ArgumentNullException(nameof(baseTypeName));

                _baseTypeName = baseTypeName;
            }


            protected override bool IsMatchImplementation(Type type) =>
                // Search for implemented interfaces as well as base classes.
                type.GetInterface(_baseTypeName) != null || type.GetBaseTypes().Any(bt => bt.Name == _baseTypeName);
        }
    }
}