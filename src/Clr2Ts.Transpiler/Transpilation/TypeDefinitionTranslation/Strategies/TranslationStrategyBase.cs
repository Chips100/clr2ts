using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Logging;
using Clr2Ts.Transpiler.Transpilation.Configuration;
using Clr2Ts.Transpiler.Transpilation.Templating;
using Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation;
using System;
using System.Reflection;

namespace Clr2Ts.Transpiler.Transpilation.TypeDefinitionTranslation.Strategies
{
    /// <summary>
    /// Base implementation of strategies for translating type definitions.
    /// </summary>
    public abstract class TranslationStrategyBase : ITypeDefinitionTranslationStrategy
    {
        private readonly ITemplatingEngine _templatingEngine;
        private readonly IDocumentationSource _documentationSource;

        /// <summary>
        /// Creates a <see cref="TranslationStrategyBase"/>.
        /// </summary>
        /// <param name="configurationSource">Source for the configuration that should be used.</param>
        /// <param name="templatingEngine">Engine to use for loading templates.</param>
        /// <param name="documentationSource">Source for looking up documentation comments for members.</param>
        /// <param name="logger">Logger to use for writing log messages.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="templatingEngine"/> or <paramref name="documentationSource"/> or <paramref name="logger"/> is null.</exception>
        protected TranslationStrategyBase(IConfigurationSource configurationSource, ITemplatingEngine templatingEngine, IDocumentationSource documentationSource, ILogger logger)
        {
            if (configurationSource == null) throw new ArgumentNullException(nameof(configurationSource));

            _templatingEngine = templatingEngine ?? throw new ArgumentNullException(nameof(templatingEngine));
            _documentationSource = documentationSource ?? throw new ArgumentNullException(nameof(documentationSource));

            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            TypeReferenceTranslator = new DefaultTypeReferenceTranslator(configurationSource, logger);
            Configuration = configurationSource.GetSection<TranspilationConfiguration>()
                ?? TranspilationConfiguration.Default;
        }

        /// <summary>
        /// Gets the configuration that the strategy should respect.
        /// </summary>
        protected TranspilationConfiguration Configuration { get; }

        /// <summary>
        /// Gets a translator for type references in the type definition.
        /// </summary>
        protected ITypeReferenceTranslator TypeReferenceTranslator { get; }

        /// <summary>
        /// Gets a logger for writing messages.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Determines if this strategy can be used to translate the specified type definition.
        /// </summary>
        /// <param name="type">Type definition that should be translated.</param>
        /// <returns>True, if this strategy can be used to translate the specified type definition; otherwise false.</returns>
        public bool CanTranslateTypeDefinition(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return CanTranslate(type);
        }

        /// <summary>
        /// Translates the specified type definition.
        /// </summary>
        /// <param name="referencedType">Type definition that should be translated.</param>
        /// <returns>Result of the translation.</returns>
        public CodeFragment Translate(Type referencedType)
        {
            if (!CanTranslateTypeDefinition(referencedType))
            {
                throw new InvalidOperationException("Called Translate although CanTranslateTypeDefinition returns false.");
            }

            return Translate(referencedType, _templatingEngine);
        }

        /// <summary>
        /// Is overridden to define which type definitions can be translated by this strategy.
        /// </summary>
        /// <param name="type">Type definition that should be translated.</param>
        /// <returns>True, if this strategy can be used to translate the specified type definition; otherwise false.</returns>
        protected abstract bool CanTranslate(Type type);

        /// <summary>
        /// Is overridden to define how the type definition is translated by this strategy.
        /// </summary>
        /// <param name="type">Type definition that should be tranlated.</param>
        /// <param name="templatingEngine">Templating engine that can be used to fill predefined code snippets.</param>
        /// <returns>Result of the translation.</returns>
        protected abstract CodeFragment Translate(Type type, ITemplatingEngine templatingEngine);

        /// <summary>
        /// Creates a documentation comment for a type definition.
        /// </summary>
        /// <param name="member">Member that should be documented.</param>
        /// <returns>A string with the documentation comment for the specified member.</returns>
        protected string GenerateDocumentationComment(MemberInfo member)
        {
            var documentation = _documentationSource.GetDocumentationText(member);
            if (string.IsNullOrWhiteSpace(documentation)) return null;

            return $@"/**
 * {documentation}
 */
";
        }
    }
}