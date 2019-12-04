using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Extensions;
using Clr2Ts.Transpiler.Transpilation.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clr2Ts.Transpiler.Transpilation.Decorators
{
    /// <summary>
    /// Procudes decorators for TypeScript definitions.
    /// </summary>
    public sealed class DecoratorTranslator
    {
        private readonly TranspilationConfiguration _configuration;

        /// <summary>
        /// Creates a <see cref="DefaultTypeReferenceTranslator"/>.
        /// </summary>
        /// <param name="configurationSource">Source for the configuration that should be used.</param>
        public DecoratorTranslator(IConfigurationSource configurationSource)
        {
            _configuration = configurationSource.GetSection<TranspilationConfiguration>();
        }

        /// <summary>
        /// Produces a set of TypeScript decorators for the specified member.
        /// </summary>
        /// <param name="member">Member that should be decorated in its TypeScript translation.</param>
        /// <returns>A set of TypeScript decorators for the specified member.</returns>
        public DecoratorTranslationResult GenerateDecorators(MemberInfo member)
        {
            if (_configuration is null) return DecoratorTranslationResult.Empty;

            // POC: For now, only support type decorators - maybe implement some
            // sort of strategy pattern later to support property or enum member decorators.
            if (member is Type type)
            {
                return _configuration.ClassDecorators
                    .Select(x => GenerateClassDecorator(type, x))
                    .Aggregate(DecoratorTranslationResult.Empty, (current, next) => current.Merge(next));
            }

            return DecoratorTranslationResult.Empty;
        }

        private DecoratorTranslationResult GenerateClassDecorator(Type type, ClassDecoratorConfiguration configuration)
        {
            if (!configuration.Condition.IsMatch(type)) return DecoratorTranslationResult.Empty;

            var parameters = configuration.DecoratorParameters
                .Select(p => p.FormatWith(new Dictionary<string, object>
                {
                    ["Type"] = type,
                    ["AssemblyName"] = type.Assembly.GetName()
                }));

            return new DecoratorTranslationResult(
                $"@{configuration.DecoratorName}({string.Join(",", parameters)})" + Environment.NewLine,

                // add import for the decorator definition, if specified.
                configuration.DecoratorImportSource is null
                    ? CodeDependencies.Empty 
                    : CodeDependencies.FromImports(new[] { configuration.DecoratorImportSource }));
        }
    }
}
