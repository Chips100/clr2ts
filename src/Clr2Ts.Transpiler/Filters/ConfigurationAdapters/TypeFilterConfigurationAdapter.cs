using Clr2Ts.Transpiler.Filters.TypeFilters;
using System;
using System.Collections.Generic;

namespace Clr2Ts.Transpiler.Filters.ConfigurationAdapters
{
    /// <summary>
    /// Adapter for type filters as they are specified in the configuration.
    /// </summary>
    public sealed class TypeFilterConfigurationAdapter: IFilter<Type>
    {
        private readonly IFilter<Type> _filter;

        /// <summary>
        /// Creates a <see cref="TypeFilterConfigurationAdapter"/>.
        /// Constructor parameters correspond to the configuration schema.
        /// </summary>
        /// <param name="hasAttribute">Filters types to those that are decorated with one of the specified attributes.</param>
        /// <param name="subTypeOf">Filters types to those that are subtypes of one of the specified types.</param>
        /// <param name="namespace">Filters types to those that are declared in the specified namespace.</param>
        public TypeFilterConfigurationAdapter(IEnumerable<string> hasAttribute, IEnumerable<string> subTypeOf, string @namespace)
        {
            var filters = new List<IFilter<Type>>();

            if (hasAttribute != null) filters.Add(new AttributeTypeFilter(hasAttribute));
            if (subTypeOf != null) filters.Add(new SubTypeFilter(subTypeOf));
            if (@namespace != null) filters.Add(new NamespaceFilter(@namespace));

            // Multiple filters in a single adapter must all be fulfilled for a match.
            _filter = CompositeFilter.And(filters);
        }

        /// <summary>
        /// Determines if the specified item is matched by this filter.
        /// </summary>
        /// <param name="item">Item to check.</param>
        /// <returns>True, if the item is matched by this filter; otherwise false.</returns>
        public bool IsMatch(Type item) => _filter.IsMatch(item);
    }
}