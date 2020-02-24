using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Filters;
using Clr2Ts.Transpiler.Filters.ConfigurationAdapters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clr2Ts.Transpiler.Input
{
    /// <summary>
    /// Represents the input-specific configuration section for a transpilation process.
    /// </summary>
    [ConfigurationSection("input")]
    public sealed class InputConfiguration
    {
        /// <summary>
        /// Creates an <see cref="InputConfiguration"/>.
        /// </summary>
        /// <param name="assemblyFiles">Names of the assembly files that should be transpiled.</param>
        /// <param name="typeFilters">Filters that should be used to determine which types should be transpiled.</param>
        /// <exception cref="ConfigurationException">Thrown when <paramref name="assemblyFiles"/> is null or empty.</exception>
        public InputConfiguration(IEnumerable<string> assemblyFiles, IEnumerable<TypeFilterConfigurationAdapter> typeFilters = null)
        {
            if (assemblyFiles == null || !assemblyFiles.Any())
            {
                throw new ConfigurationException("At least one assembly file must be specified in the input section of the configuration.");
            }

            AssemblyFiles = assemblyFiles.ToList();
            TypeFilter = typeFilters != null ? CompositeFilter.Or(typeFilters) : ConstantFilter.MatchAll<Type>();
        }

        /// <summary>
        /// Gets the names of the assembly files that should be transpiled.
        /// </summary>
        public IEnumerable<string> AssemblyFiles { get; }

        /// <summary>
        /// Gets the filter that should be used to determine which types should be transpiled.
        /// </summary>
        public IFilter<Type> TypeFilter { get; }
    }
}