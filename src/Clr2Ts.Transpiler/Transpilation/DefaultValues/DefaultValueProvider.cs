using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Logging;
using Clr2Ts.Transpiler.Transpilation.Configuration;
using Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clr2Ts.Transpiler.Transpilation.DefaultValues
{
    /// <summary>
    /// Provides default values to use for generated TypeScript code.
    /// </summary>
    public sealed class DefaultValueProvider
    {
        private readonly IDictionary<DefaultValueStrategy, Func<Type, string>> _defaultValueAssignments
            = new Dictionary<DefaultValueStrategy, Func<Type, string>>
            {
                { DefaultValueStrategy.None, _ => string.Empty },
                { DefaultValueStrategy.AlwaysNull, _ => " = null" },
                { DefaultValueStrategy.PrimitiveDefaults, t => $" = {GetPrimitiveDefaultAsTypeScript(t)}" }
            };

        private readonly TranspilationConfiguration _configuration;
        private readonly ILogger _logger;

        /// <summary>
        /// Creates a DefaultValueProvider.
        /// </summary>
        /// <param name="configurationSource">Configuration that the provider should respect.</param>
        /// <param name="logger">Logger to use for writing log messages.</param>
        public DefaultValueProvider(IConfigurationSource configurationSource, ILogger logger, ITypeReferenceTranslator typeReferenceTranslator)
        {
            if (configurationSource is null) throw new ArgumentNullException(nameof(configurationSource));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configurationSource.GetSection<TranspilationConfiguration>()
                ?? TranspilationConfiguration.Default;

            _defaultValueAssignments.Add(DefaultValueStrategy.DefaultConstructor,
                type => GetDefaultConstructorCall(type, _configuration, typeReferenceTranslator));
        }

        /// <summary>
        /// Creates a TypeScript assignment that assigns the default value to a property, e.g. <code>= null;</code>.
        /// </summary>
        /// <param name="property">Property that should receive a default value assignment.</param>
        /// <returns>TypeScript code with the assignment of the default value, e.g. <code>= null;</code>.</returns>
        public string Assignment(PropertyInfo property)
        {
            if (_defaultValueAssignments.TryGetValue(_configuration.DefaultValues, out var assignmentFactory))
            {
                return assignmentFactory(property.PropertyType);
            }

            // Fallback: no assignment.
            _logger.WriteWarning($"No strategy for creating assignments with DefaultValueStrategy: {_configuration.DefaultValues}");
            return string.Empty;
        }

        private string GetDefaultConstructorCall(Type type, TranspilationConfiguration configuration, ITypeReferenceTranslator typeReferenceTranslator)
        {
            var unconstructables = new[] { typeof(string), typeof(object) };
            if (configuration.CustomTypeMaps.Any(map => map.MapsType(type))
                || !type.IsClass
                || unconstructables.Contains(type)
                || type.IsGenericParameter)
            {
                return _defaultValueAssignments[DefaultValueStrategy.PrimitiveDefaults](type);
            }

            return $" = new {typeReferenceTranslator.Translate(type).ReferencedTypeName}()";
        }

        private static string GetPrimitiveDefaultAsTypeScript(Type type)
        {
            if (!type.IsPrimitive) return "null";

            // Cache default values per type as generic construction via reflection may get expensive.
            return ClrDefaultCache.GetOrAdd(type, t =>
            {
                // https://stackoverflow.com/questions/325426/programmatic-equivalent-of-defaulttype
                var defaultValue = typeof(DefaultValueProvider)
                    .GetMethod(nameof(GetDefaultGeneric), BindingFlags.NonPublic | BindingFlags.Static)
                    .MakeGenericMethod(t).Invoke(null, null);

                return JsonConvert.SerializeObject(defaultValue);
            });
        }

        private static T GetDefaultGeneric<T>() => default(T);

        private static readonly ConcurrentDictionary<Type, string> ClrDefaultCache
            = new ConcurrentDictionary<Type, string>();
    }
}