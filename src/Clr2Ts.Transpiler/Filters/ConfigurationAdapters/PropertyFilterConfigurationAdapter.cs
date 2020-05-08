using Clr2Ts.Transpiler.Filters.MemberInfoFilters;
using System.Collections.Generic;
using System.Reflection;

namespace Clr2Ts.Transpiler.Filters.ConfigurationAdapters
{
    /// <summary>
    /// Adapter for property filters as they are specified in the configuration.
    /// </summary>
    public sealed class PropertyFilterConfigurationAdapter : IFilter<PropertyInfo>
    {
        private readonly IFilter<PropertyInfo> _filter;

        /// <summary>
        /// Creates a <see cref="PropertyFilterConfigurationAdapter"/>.
        /// Constructor parameters correspond to the configuration schema.
        /// </summary>
        /// <param name="type">Filters properties by applying a filter to their declaring type.</param>
        /// <param name="hasAttribute">Filters properties to those that are decorated with one of the specified attributes.</param>
        public PropertyFilterConfigurationAdapter(TypeFilterConfigurationAdapter type, IEnumerable<string> hasAttribute)
        {
            var filters = new List<IFilter<PropertyInfo>>();

            if (type != null) filters.Add(ProjectedFilter.Create(type, (PropertyInfo p) => p.DeclaringType));
            if (hasAttribute != null) filters.Add(new HasAttributeFilter(hasAttribute));

            // Multiple filters in a single adapter must all be fulfilled for a match.
            _filter = CompositeFilter.And(filters);
        }

        /// <summary>
        /// Determines if the specified item is matched by this filter.
        /// </summary>
        /// <param name="item">Item to check.</param>
        /// <returns>True, if the item is matched by this filter; otherwise false.</returns>
        public bool IsMatch(PropertyInfo item) => _filter.IsMatch(item);
    }
}