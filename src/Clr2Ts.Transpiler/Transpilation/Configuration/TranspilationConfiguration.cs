using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Filters.ConfigurationAdapters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Clr2Ts.Transpiler.Transpilation.Configuration
{
    /// <summary>
    /// Represents the transpilation-specific configuration section for a transpilation process.
    /// </summary>
    [ConfigurationSection("transpilation")]
    public sealed class TranspilationConfiguration
    {
        /// <summary>
        /// Creates an <see cref="TranspilationConfiguration"/>.
        /// </summary>
        /// <param name="camelCase">If set to true, property names should be converted to camelCase.</param>
        /// <param name="flattenBaseTypes">If set to true, properties derived from base types will be declared in the type itself.</param>
        /// <param name="runtimeDependencyLoading">If set to true, dependencies of a type will automatically be loaded when the type is used for the first time.</param>
        /// <param name="defaultValues">Strategy that defines how to assign default values to TypeScript properties.</param>
        /// <param name="customTypeMaps">Custom type maps mapping .NET types to TypeScript types.</param>
        /// <param name="classDecorators">Configuration for decorators that should be generated for classes in TypeScript.</param>
        /// <param name="propertyDecorators">Configuration for decorators that should be generated for properties in TypeScript.</param>
        /// <param name="enumAttributeMaps">Configuration for enum member attributes that should be stored in a map.</param>
        /// <param name="injectTypeHintCondition">Configuration for checking if to inject $type-hint to type.</param>
        public TranspilationConfiguration(
            bool? camelCase,
            bool? flattenBaseTypes,
            bool? runtimeDependencyLoading,
            DefaultValueStrategy? defaultValues,
            IEnumerable<CustomTypeMap> customTypeMaps,
            IEnumerable<ClassDecoratorConfiguration> classDecorators,
            IEnumerable<PropertyDecoratorConfiguration> propertyDecorators,
            IDictionary<string, string> enumAttributeMaps,
            TypeFilterConfigurationAdapter injectTypeHintCondition)
        {
            CamelCase = camelCase ?? true;
            RuntimeDependencyLoading = runtimeDependencyLoading ?? true;
            FlattenBaseTypes = flattenBaseTypes ?? false;
            DefaultValues = defaultValues ?? default(DefaultValueStrategy);
            CustomTypeMaps = customTypeMaps?.ToList() ?? Enumerable.Empty<CustomTypeMap>();
            ClassDecorators = classDecorators?.ToList() ?? Enumerable.Empty<ClassDecoratorConfiguration>();
            PropertyDecorators = propertyDecorators?.ToList() ?? Enumerable.Empty<PropertyDecoratorConfiguration>();
            EnumAttributeMaps = new ReadOnlyDictionary<string, string>(enumAttributeMaps ?? new Dictionary<string, string>());
            InjectTypeHintCondition = injectTypeHintCondition;
        }

        /// <summary>
        /// If set to true, property names should be converted to camelCase.
        /// </summary>
        public bool CamelCase { get; }

        /// <summary>
        /// If set to true, properties derived from base types will be declared in the type itself.
        /// </summary>
        public bool FlattenBaseTypes { get; }

        /// <summary>
        /// If specified, dependencies of a type will automatically be loaded
        /// when the type is used for the first time (to ensure evaluation of decorators, for example).
        /// </summary>
        public bool RuntimeDependencyLoading { get; }

        /// <summary>
        /// Gets the strategy that defines how to assign default values to TypeScript properties.
        /// </summary>
        public DefaultValueStrategy DefaultValues { get; }

        /// <summary>
        /// Gets custom type maps mapping .NET types to TypeScript types.
        /// </summary>
        public IEnumerable<CustomTypeMap> CustomTypeMaps { get; }

        /// <summary>
        /// Gets the configuration for decorators that should be generated for classes in TypeScript.
        /// </summary>
        public IEnumerable<ClassDecoratorConfiguration> ClassDecorators { get; }

        /// <summary>
        /// Gets the configuration for decorators that should be generated for properties in TypeScript.
        /// </summary>
        public IEnumerable<PropertyDecoratorConfiguration> PropertyDecorators { get; }

        /// <summary>
        /// Gets the configuration for enum member attributes that should be stored in a map.
        /// </summary>
        public IReadOnlyDictionary<string, string> EnumAttributeMaps { get; }

        public TypeFilterConfigurationAdapter InjectTypeHintCondition { get; }

        /// <summary>
        /// Returns a default configuration for the transpilation
        /// that should be used if the section has been omitted.
        /// </summary>
        public static TranspilationConfiguration Default =>
            new TranspilationConfiguration(
                true,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            );
    }
}