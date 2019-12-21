using Clr2Ts.Transpiler.Filters.ConfigurationAdapters;
using System.Collections.Generic;
using System.Reflection;

namespace Clr2Ts.Transpiler.Transpilation.Configuration
{
    /// <summary>
    /// Configuration of decorators that should be generated for TypeScript property definitions.
    /// </summary>
    public sealed class PropertyDecoratorConfiguration : DecoratorConfiguration<PropertyInfo>
    {
        /// <summary>
        /// Creates a ClassDecoratorConfiguration.
        /// </summary>
        /// <param name="if">Condition that specifies the properties to apply the decorator to.</param>
        /// <param name="decorator">Name of the decorator.</param>
        /// <param name="parameters">Parameters for the decorator (template strings).</param>
        /// <param name="import">Import to reference the decorator definition.</param>
        public PropertyDecoratorConfiguration(PropertyFilterConfigurationAdapter @if, string decorator, IEnumerable<string> parameters, string import)
            : base(@if, decorator, parameters, import)
        { }
    }
}