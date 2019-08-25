using Clr2Ts.Transpiler.Configuration;
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
    /// Strategy for translating definitions of class types.
    /// </summary>
    public sealed class ClassDefinitionTranslationStrategy : TranslationStrategyBase
    {
        /// <summary>
        /// Creates a <see cref="ClassDefinitionTranslationStrategy"/>.
        /// </summary>
        /// <param name="configurationSource">Source for the configuration that should be used.</param>
        /// <param name="templatingEngine">Engine to use for loading templates.</param>
        /// <param name="documentationSource">Source for looking up documentation comments for members.</param>
        /// <param name="logger">Logger to use for writing log messages.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="templatingEngine"/> or <paramref name="documentationSource"/> or <paramref name="logger"/> is null.</exception>
        public ClassDefinitionTranslationStrategy(IConfigurationSource configurationSource, ITemplatingEngine templatingEngine, IDocumentationSource documentationSource, ILogger logger)
            : base(configurationSource, templatingEngine, documentationSource, logger)
        { }

        /// <summary>
        /// Is overridden to define which type definitions can be translated by this strategy.
        /// </summary>
        /// <param name="type">Type definition that should be translated.</param>
        /// <returns>True, if this strategy can be used to translate the specified type definition; otherwise false.</returns>
        protected override bool CanTranslate(Type type)
            => type.IsClass;

        /// <summary>
        /// Is overridden to define how the type definition is translated by this strategy.
        /// </summary>
        /// <param name="type">Type definition that should be tranlated.</param>
        /// <param name="templatingEngine">Templating engine that can be used to fill predefined code snippets.</param>
        /// <returns>Result of the translation.</returns>
        protected override CodeFragment Translate(Type type, ITemplatingEngine templatingEngine)
        {
            var code = templatingEngine.UseTemplate("ClassDefinition", new Dictionary<string, string>
            {
                { "ClassDeclaration", type.GetNameWithGenericTypeParameters() },
                { "Documentation", GenerateDocumentationComment(type) },
                { "Properties", GeneratePropertyDefinitions(type, templatingEngine, out var dependencies).AddIndentation() }
            });

            return new CodeFragment(
                CodeFragmentId.ForClrType(type),
                dependencies,
                code);
        }

        private string GeneratePropertyDefinitions(Type type, ITemplatingEngine templatingEngine, out IEnumerable<CodeFragmentId> dependencies)
        {
            var propertyCodeSnippets = new List<string>();
            var deps = new List<CodeFragmentId>();

            foreach (var property in type.GetProperties())
            {
                Logger.WriteInformation($"Translating property {property.Name} on type {type}.");

                var typeReferenceTranslation = TypeReferenceTranslator.Translate(property.PropertyType);
                deps.AddRange(typeReferenceTranslation.Dependencies);
                propertyCodeSnippets.Add(templatingEngine.UseTemplate("ClassPropertyDefinition", new Dictionary<string, string>
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