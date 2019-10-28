﻿using Clr2Ts.Transpiler.Configuration;
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
        /// <param name="customTypeMaps">Custom type maps mapping .NET types to TypeScript types.</param>
        public TranspilationConfiguration(bool? camelCase, IEnumerable<CustomTypeMap> customTypeMaps)
        {
            CamelCase = camelCase ?? true; // Defaults to true.
            CustomTypeMaps = customTypeMaps?.ToList() ?? Enumerable.Empty<CustomTypeMap>();
        }

        /// <summary>
        /// If set to true, property names should be converted to camelCase.
        /// </summary>
        public bool CamelCase { get; }

        /// <summary>
        /// Gets custom type maps mapping .NET types to TypeScript types.
        /// </summary>
        public IEnumerable<CustomTypeMap> CustomTypeMaps { get; }

        /// <summary>
        /// Returns a default configuration for the transpilation
        /// that should be used if the section has been omitted.
        /// </summary>
        public static TranspilationConfiguration Default => new TranspilationConfiguration(true, null);
    }
}