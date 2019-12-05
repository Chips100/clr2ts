using Clr2Ts.Transpiler.Configuration;
using System.Collections.Generic;
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
        /// <param name="customTypeMaps">Custom type maps mapping .NET types to TypeScript types.</param>
        /// <param name="classDecorators">Configuration for decorators that should be generated in TypeScript.</param>
        public TranspilationConfiguration(bool? camelCase, bool? flattenBaseTypes, IEnumerable<CustomTypeMap> customTypeMaps, IEnumerable<ClassDecoratorConfiguration> classDecorators)
        {
            CamelCase = camelCase ?? true; // Defaults to true.
            FlattenBaseTypes = flattenBaseTypes ?? false;
            CustomTypeMaps = customTypeMaps?.ToList() ?? Enumerable.Empty<CustomTypeMap>();
            ClassDecorators = classDecorators?.ToList() ?? Enumerable.Empty<ClassDecoratorConfiguration>();
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
        /// Gets custom type maps mapping .NET types to TypeScript types.
        /// </summary>
        public IEnumerable<CustomTypeMap> CustomTypeMaps { get; }

        /// <summary>
        /// Gets the configuration for decorators that should be generated in TypeScript.
        /// </summary>
        public IEnumerable<ClassDecoratorConfiguration> ClassDecorators { get; }

        /// <summary>
        /// Returns a default configuration for the transpilation
        /// that should be used if the section has been omitted.
        /// </summary>
        public static TranspilationConfiguration Default => new TranspilationConfiguration(true, null, null, null);
    }
}