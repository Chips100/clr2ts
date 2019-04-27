using Clr2Ts.Transpiler.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clr2Ts.Transpiler.Filters.TypeFilters
{
    /// <summary>
    /// Filter that matches types that are a subtype of one of the specified types (interface or base class).
    /// </summary>
    public sealed class SubTypeFilter : IFilter<Type>
    {
        private readonly IEnumerable<string> _baseTypeNames;

        /// <summary>
        /// Creates a <see cref="SubTypeFilter"/>.
        /// </summary>
        /// <param name="baseTypeNames">Name of the base types to look for on the filtered types.</param>
        public SubTypeFilter(IEnumerable<string> baseTypeNames)
        {
            _baseTypeNames = baseTypeNames ?? throw new ArgumentNullException(nameof(baseTypeNames));
        }

        public bool IsMatch(Type item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            // Search for implemented interfaces as well as base classes.
            return _baseTypeNames.Any(x => IsSubTypeOf(item, x));
        }

        /// <summary>
        /// Determines if the specified item is matched by this filter.
        /// </summary>
        /// <param name="item">Item to check.</param>
        /// <returns>True, if the item is matched by this filter; otherwise false.</returns>
        private bool IsSubTypeOf(Type type, string baseTypeName)
            => type.GetInterface(baseTypeName) != null
                || type.GetBaseTypes().Any(bt => bt.Name == baseTypeName);
    }
}