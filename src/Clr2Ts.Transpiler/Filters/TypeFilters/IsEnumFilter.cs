using Clr2Ts.Transpiler.Extensions;
using System;

namespace Clr2Ts.Transpiler.Filters.TypeFilters
{
    /// <summary>
    /// Filter that matches only enum types.
    /// </summary>
    public sealed class IsEnumFilter : IFilter<Type>
    {
        /// <summary>
        /// Determines if the specified item is matched by this filter.
        /// </summary>
        /// <param name="item">Item to check.</param>
        /// <returns>True, if the item is matched by this filter; otherwise false.</returns>
        public bool IsMatch(Type item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            // Nullable enums are still enums from our perspective.
            return item.UnwrapNullable().IsEnum;
        }
    }
}