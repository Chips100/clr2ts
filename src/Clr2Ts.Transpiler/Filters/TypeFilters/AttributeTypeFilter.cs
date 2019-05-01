using System;
using System.Collections.Generic;
using System.Linq;

namespace Clr2Ts.Transpiler.Filters.TypeFilters
{
    /// <summary>
    /// Filter that matches types that are decorated with one of the specified attributes.
    /// </summary>
    public sealed class AttributeTypeFilter : IFilter<Type>
    {
        private readonly IEnumerable<string> _attributeNames;

        /// <summary>
        /// Creates a <see cref="AttributeTypeFilter"/>.
        /// </summary>
        /// <param name="attributeNames">Name of the attributes to look for on the filtered types.</param>
        public AttributeTypeFilter(IEnumerable<string> attributeNames)
        {
            if (attributeNames == null) throw new ArgumentNullException(nameof(attributeNames));

            _attributeNames = attributeNames.ToList();
        }

        /// <summary>
        /// Determines if the specified item is matched by this filter.
        /// </summary>
        /// <param name="item">Item to check.</param>
        /// <returns>True, if the item is matched by this filter; otherwise false.</returns>
        public bool IsMatch(Type item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var attributesOfType = new HashSet<string>(GetAttributeNamesOfType(item));
            return _attributeNames.All(x => GetAcceptedAttributeNames(x).Any(attributesOfType.Contains));
        }

        private IEnumerable<string> GetAttributeNamesOfType(Type type)
            => type.GetCustomAttributes(true).Select(attr => attr.GetType().Name);

        private IEnumerable<string> GetAcceptedAttributeNames(string specifiedName)
            // Allow specifying names with or without the "Attribute"-suffix.
            => new[] { specifiedName, $"{specifiedName}Attribute" };
    }
}