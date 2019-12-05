﻿using Clr2Ts.Transpiler.Filters;
using Clr2Ts.Transpiler.Filters.ConfigurationAdapters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clr2Ts.Transpiler.Transpilation.Configuration
{
    /// <summary>
    /// Configuration of decorators that should be generated for TypeScript class definitions.
    /// </summary>
    public sealed class ClassDecoratorConfiguration
    {
        /// <summary>
        /// Creates a ClassDecoratorConfiguration.
        /// </summary>
        /// <param name="if">Condition that specifies the types to apply the decorator to.</param>
        /// <param name="decorator">Name of the decorator.</param>
        /// <param name="parameters">Parameters for the decorator (template strings).</param>
        /// <param name="import">Import to reference the decorator definition.</param>
        public ClassDecoratorConfiguration(TypeFilterConfigurationAdapter @if, string decorator, IEnumerable<string> parameters, string import)
        {
            Condition = @if;
            DecoratorName = decorator;
            DecoratorParameters = new List<string>(parameters ?? Enumerable.Empty<string>());
            Import = import;
        }

        /// <summary>
        /// Gets the condition that specifies the types to apply the decorator to.
        /// </summary>
        public IFilter<Type> Condition { get; }

        /// <summary>
        /// Gets the name of the decorator.
        /// </summary>
        public string DecoratorName { get; }

        /// <summary>
        /// Gets the parameters for the decorator (template strings).
        /// </summary>
        public IEnumerable<string> DecoratorParameters { get; }

        /// <summary>
        /// Gets the source from which the TypeScript decorator is imported.
        /// </summary>
        public string Import { get; }

        /// <summary>
        /// Creates a set of dependencies that references the decorator.
        /// </summary>
        /// <returns>A set of dependencies that references the decorator.</returns>
        public CodeDependencies CreateImportDependency()
        {
            // Source is optional; might be an ambient type (like "any").
            if (string.IsNullOrWhiteSpace(Import)) return CodeDependencies.Empty;

            // Construct dependencies referencing the mapped type.
            return CodeDependencies.FromImports(new[] { new Import(DecoratorName, Import) });
        }
    }
}