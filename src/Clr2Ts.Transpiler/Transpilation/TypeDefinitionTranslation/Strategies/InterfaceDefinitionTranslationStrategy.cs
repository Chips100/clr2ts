﻿using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Extensions;
using Clr2Ts.Transpiler.Logging;
using Clr2Ts.Transpiler.Transpilation.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clr2Ts.Transpiler.Transpilation.TypeDefinitionTranslation.Strategies
{
    /// <summary>
    /// Strategy for translating definitions of interface types.
    /// </summary>
    public sealed class InterfaceDefinitionTranslationStrategy : TranslationStrategyBase
    {
        /// <summary>
        /// Creates a <see cref="InterfaceDefinitionTranslationStrategy"/>.
        /// </summary>
        /// <param name="configurationSource">Source for the configuration that should be used.</param>
        /// <param name="templatingEngine">Engine to use for loading templates.</param>
        /// <param name="documentationSource">Source for looking up documentation comments for members.</param>
        /// <param name="logger">Logger to use for writing log messages.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="templatingEngine"/> or <paramref name="documentationSource"/> or <paramref name="logger"/> is null.</exception>
        public InterfaceDefinitionTranslationStrategy(IConfigurationSource configurationSource, ITemplatingEngine templatingEngine, IDocumentationSource documentationSource, ILogger logger)
            : base(configurationSource, templatingEngine, documentationSource, logger)
        { }

        /// <summary>
        /// Is overridden to define which type definitions can be translated by this strategy.
        /// </summary>
        /// <param name="type">Type definition that should be translated.</param>
        /// <returns>True, if this strategy can be used to translate the specified type definition; otherwise false.</returns>
        protected override bool CanTranslate(Type type)
            => type.IsInterface;

        /// <summary>
        /// Is overridden to define how the type definition is translated by this strategy.
        /// </summary>
        /// <param name="type">Type definition that should be tranlated.</param>
        /// <param name="templatingEngine">Templating engine that can be used to fill predefined code snippets.</param>
        /// <returns>Result of the translation.</returns>
        protected override CodeFragment Translate(Type type, ITemplatingEngine templatingEngine)
        {
            var properties = GeneratePropertyDefinitions(type, templatingEngine, out var dependencies);
            var declaration = GenerateClassDeclaration(type);
            dependencies = dependencies.Merge(declaration.dependencies);

            var code = templatingEngine.UseTemplate("InterfaceDefinition", new Dictionary<string, string>
            {
                { "InterfaceDeclaration", declaration.code },
                { "Documentation", GenerateDocumentationComment(type) },
                { "Properties", properties.AddIndentation() }
            });

            return new CodeFragment(
                CodeFragmentId.ForClrType(type),
                code,
                dependencies);
        }

        private (string code, CodeDependencies dependencies) GenerateClassDeclaration(Type type)
        {
            var deps = CodeDependencies.Empty;

            // Type name with generic type parameters (including constraints).
            var declaration = type.GetNameWithGenericTypeParameters(t =>
            {
                var constraintTranslations = t.GetGenericParameterConstraints()
                    .Except(new[] { typeof(ValueType) })
                    .Select(TypeReferenceTranslator.Translate);

                deps = constraintTranslations.Select(x => x.Dependencies).Aggregate(deps, (d, next) => d.Merge(next));
                return !constraintTranslations.Any() ? string.Empty :
                    $" extends {string.Join(" & ", constraintTranslations.Select(x => x.ReferencedTypeName))}";
            });

            return (declaration, deps);
        }

        private string GeneratePropertyDefinitions(Type type, ITemplatingEngine templatingEngine, out CodeDependencies dependencies)
        {
            var propertyCodeSnippets = new List<string>();
            var deps = CodeDependencies.Empty;

            foreach (var property in type.GetProperties())
            {
                Logger.WriteInformation($"Translating property {property.Name} on type {type}.");

                var typeReferenceTranslation = TypeReferenceTranslator.Translate(property.PropertyType);
                deps = deps.Merge(typeReferenceTranslation.Dependencies);

                propertyCodeSnippets.Add(templatingEngine.UseTemplate("InterfacePropertyDefinition", new Dictionary<string, string>
                {
                    { "PropertyName", GetTypeScriptPropertyName(property) },
                    { "Documentation", GenerateDocumentationComment(property) },
                    { "PropertyType", typeReferenceTranslation.ReferencedTypeName }
                }));
            }

            dependencies = deps;
            return string.Join(Environment.NewLine, propertyCodeSnippets);
        }

        private string GetTypeScriptPropertyName(PropertyInfo property)
            => Configuration.CamelCase ? property.Name.ToCamelCase() : property.Name;
    }
}