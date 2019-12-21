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
            if (member is null) throw new ArgumentNullException(nameof(member));

            // Skip entirely if no specific configuration has been provided.
            if (_configuration is null) return DecoratorTranslationResult.Empty;

            // Combine all decorator definitions into a single result.
            return GenerateIndividualDecorators(member)
                .Aggregate(DecoratorTranslationResult.Empty, (current, next) => current.Merge(next));
        }

        private IEnumerable<DecoratorTranslationResult> GenerateIndividualDecorators(MemberInfo member)
        {
            if (member is Type type)
            {
                return _configuration.ClassDecorators
                    .Select(x => GenerateDecorator(type, x));
            }
            else if (member is PropertyInfo property)
            {
                return _configuration.PropertyDecorators
                    .Select(x => GenerateDecorator(property, x));
            }

            return Enumerable.Empty<DecoratorTranslationResult>();
        }

        private DecoratorTranslationResult GenerateDecorator<TTarget>(TTarget target, DecoratorConfiguration<TTarget> configuration)
            where TTarget: MemberInfo
        {
            if (!configuration.Condition.IsMatch(target)) return DecoratorTranslationResult.Empty;

            var parameters = configuration.DecoratorParameters
                .Select(p => p.FormatWith(target.CreateFormattingContext()));

            return new DecoratorTranslationResult(
                $"@{configuration.DecoratorName}({string.Join(",", parameters)})" + Environment.NewLine,
                configuration.CreateImportDependency());
        }
    }
}
