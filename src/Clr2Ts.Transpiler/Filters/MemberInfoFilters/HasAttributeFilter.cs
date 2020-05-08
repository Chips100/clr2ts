using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clr2Ts.Transpiler.Filters.MemberInfoFilters
{
    /// <summary>
    /// Filter that matches members that are decorated with one of the specified attributes.
    /// </summary>
    public sealed class HasAttributeFilter : IFilter<MemberInfo>
    {
        private readonly IEnumerable<string> _attributeNames;

        /// <summary>
        /// Creates a <see cref="HasAttributeFilter"/>.
        /// </summary>
        /// <param name="attributeNames">Name of the attributes to look for on the filtered members.</param>
        public HasAttributeFilter(IEnumerable<string> attributeNames)
        {
            if (attributeNames == null) throw new ArgumentNullException(nameof(attributeNames));

            _attributeNames = attributeNames.ToList();
        }

        /// <summary>
        /// Determines if the specified item is matched by this filter.
        /// </summary>
        /// <param name="item">Item to check.</param>
        /// <returns>True, if the item is matched by this filter; otherwise false.</returns>
        public bool IsMatch(MemberInfo item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var attributesOfType = new HashSet<string>(GetAttributeNames(item));
            return _attributeNames.All(x => GetAcceptedAttributeNames(x).Any(attributesOfType.Contains));
        }

        private IEnumerable<string> GetAttributeNames(MemberInfo type)
            => type.GetCustomAttributes(true).Select(attr => attr.GetType().Name);

        private IEnumerable<string> GetAcceptedAttributeNames(string specifiedName)
            // Allow specifying names with or without the "Attribute"-suffix.
            => new[] { specifiedName, $"{specifiedName}Attribute" };
    }
}