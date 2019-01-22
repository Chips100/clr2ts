using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clr2Ts.Transpiler.Transpilation.TypeScript
{
    public static class TypeHandlingExtensions
    {
        private static readonly IDictionary<Type, string> BuiltInTypes = new Dictionary<Type, string>
        {
            { typeof(byte), "number" },
            { typeof(short), "number" },
            { typeof(int), "number" },
            { typeof(long), "number" },
            { typeof(float), "number" },
            { typeof(double), "number" },
            { typeof(decimal), "number" },
            { typeof(bool), "boolean" },
            { typeof(DateTime), "Date" },
            { typeof(string), "string" },
            { typeof(Guid), "string" },
            { typeof(object), "any" }
        };

        public static IEnumerable<PropertyInfo> GetPropertiesForTranspilation(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.GetProperties();
        }

        public static IEnumerable<PropertyInfo> GetPropertiesForDependencies(this Type type)
        {
            return type
                .GetPropertiesForTranspilation()
                .Where(p => !BuiltInTypes.ContainsKey(p.PropertyType.Reduce()));
        }

        public static string ToTypeScriptName(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            type = type.Reduce();
            if (BuiltInTypes.TryGetValue(type, out var builtInName)) return builtInName;

            return type.Name;
        }

        public static Type Reduce(this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            type = Nullable.GetUnderlyingType(type) ?? type;
            return type;
        }
    }
}