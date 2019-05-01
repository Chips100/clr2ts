using System;

namespace Clr2Ts.Transpiler.Filters.TypeFilters
{
    /// <summary>
    /// Filter that matches types that are declared in the specified namespace.
    /// </summary>
    public sealed class NamespaceFilter: IFilter<Type>
    {
        private readonly string _namespace;

        /// <summary>
        /// Creates a <see cref="NamespaceFilter"/>.
        /// </summary>
        /// <param name="ns">Namespace that types need to be declared in to be matched by this filter.</param>
        public NamespaceFilter(string ns)
        {
            _namespace = ns ?? throw new ArgumentNullException(nameof(ns));
        }

        /// <summary>
        /// Determines if the specified item is matched by this filter.
        /// </summary>
        /// <param name="item">Item to check.</param>
        /// <returns>True, if the item is matched by this filter; otherwise false.</returns>
        public bool IsMatch(Type item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            // StartsWith comparison to allow configuration of more general namespaces.
            return item.Namespace.StartsWith(_namespace);
        }
    }
}