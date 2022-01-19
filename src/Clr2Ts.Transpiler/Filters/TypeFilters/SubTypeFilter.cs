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

        /// <summary>
        /// Determines if the specified item is matched by this filter.
        /// </summary>
        /// <param name="item">Item to check.</param>
        /// <returns>True, if the item is matched by this filter; otherwise false.</returns>
        public bool IsMatch(Type item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            // Search for implemented interfaces as well as base classes.
            return _baseTypeNames.All(x => IsSubTypeOf(item, x));
        }

        private bool IsSubTypeOf(Type type, string baseTypeName)
            // Subtype relation: Identical type, base class hierarchy or interface implementation.
            => new[] { type.UnwrapNullable() }.Concat(type.GetBaseTypes()).Concat(type.GetInterfaces())
                    .Any(bt => bt.Name == baseTypeName || bt.FullName == baseTypeName);
    }
}