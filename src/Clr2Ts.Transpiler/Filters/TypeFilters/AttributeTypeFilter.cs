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
        private readonly HashSet<string> _acceptedAttributeNames;

        /// <summary>
        /// Creates a <see cref="AttributeTypeFilter"/>.
        /// </summary>
        /// <param name="attributeNames">Name of the attributes to look for on the filtered types.</param>
        public AttributeTypeFilter(IEnumerable<string> attributeNames)
        {
            if (attributeNames == null) throw new ArgumentNullException(nameof(attributeNames));

            _acceptedAttributeNames = new HashSet<string>(
                attributeNames.SelectMany(GetAcceptedAttributeNames));
        }

        /// <summary>
        /// Determines if the specified item is matched by this filter.
        /// </summary>
        /// <param name="item">Item to check.</param>
        /// <returns>True, if the item is matched by this filter; otherwise false.</returns>
        public bool IsMatch(Type item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            return GetAttributeNamesOfType(item).All(_acceptedAttributeNames.Contains);
        }

        private IEnumerable<string> GetAttributeNamesOfType(Type type)
            => type.GetCustomAttributes(true).Select(attr => attr.GetType().Name);

        private IEnumerable<string> GetAcceptedAttributeNames(string specifiedName)
            // Allow specifying names with or without the "Attribute"-suffix.
            => new[] { specifiedName, $"{specifiedName}Attribute" };
    }
}