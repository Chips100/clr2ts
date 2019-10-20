using Clr2Ts.Transpiler.Filters;
using Clr2Ts.Transpiler.Filters.ConfigurationAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Clr2Ts.Transpiler.Transpilation.Decorators
{
    public sealed class TypeDecoratorConfiguration
    {
        public TypeDecoratorConfiguration(TypeFilterConfigurationAdapter @if, string decorator, IEnumerable<string> @params, string import)
        {
            Condition = @if;
            DecoratorName = decorator;
            DecoratorParameters = new List<string>(@params ?? Enumerable.Empty<string>());
            DecoratorImportSource = import;
        }

        public IFilter<Type> Condition { get; }

        public string DecoratorName { get; }

        public IEnumerable<string> DecoratorParameters { get; }

        public string DecoratorImportSource { get; }
    }
}