using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clr2Ts.Transpiler.Extensions
{
    /// <summary>
    /// Defines extension methods for instances of <see cref="MemberInfo"/>.
    /// </summary>
    public static class MemberInfoExtensions
    {
        private const string AttributeNameSuffix = "Attribute";

        /// <summary>
        /// Creates a context with information about the specified member
        /// that can be used for formatting template strings (see <see cref="StringExtensions.FormatWith(string, IDictionary{string, object})"/>).
        /// </summary>
        /// <param name="member">Member about which the context should provide information.</param>
        /// <returns>A context for formatting template strings with information about the specified member.</returns>
        public static IDictionary<string, object> CreateFormattingContext(this MemberInfo member)
        {
            if (member is null) throw new ArgumentNullException(nameof(member));

            var context =
                member is TypeInfo type ? CreateFormattingContext(type)
                : member is PropertyInfo property ? CreateFormattingContext(property)
                : new Dictionary<string, object>();

            AddAttributesToContext(context, member);
            return context;
        }

        private static IDictionary<string, object> CreateFormattingContext(TypeInfo type)
        {
            return new Dictionary<string, object>
            {
                ["Type"] = type,
                ["AssemblyName"] = type.Assembly.GetName()
            };
        }

        private static IDictionary<string, object> CreateFormattingContext(PropertyInfo property)
        {
            var propertyContext = new Dictionary<string, object>
            {
                ["Property"] = property,
                ["Type"] = property.DeclaringType
            };

            AddAttributesToContext(propertyContext, property.DeclaringType, "TypeAttributes");
            return propertyContext;
        }

        private static void AddAttributesToContext(IDictionary<string, object> context, MemberInfo member, string contextKey = "Attributes")
        {
            context.Add(contextKey, member.CustomAttributes
                // TODO: multiple attributes of same type. How to handle?
                // Idea: Create multiple contexts if attribute is actually used in template?
                .Select(x => member.GetCustomAttributes(x.AttributeType).First())
                .SelectMany(attribute => GetAttributeNames(attribute).Select(name => new { name, attribute }))
                .ToDictionary(x => x.name, x => x.attribute));
        }

        private static IEnumerable<string> GetAttributeNames(Attribute attribute)
        {
            var typeName = attribute.GetType().Name;
            
            yield return typeName;

            // Support referencing attributes in template strings without the Attribute suffix.
            if (typeName.EndsWith(AttributeNameSuffix))
            {
                yield return typeName.Substring(0, typeName.Length - AttributeNameSuffix.Length);
            }
        }
    }
}
