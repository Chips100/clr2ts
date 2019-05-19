using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Logging;
using Clr2Ts.Transpiler.Transpilation.Templating;
using Clr2Ts.Transpiler.Transpilation.TypeDefinitionTranslation.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clr2Ts.Transpiler.Transpilation.TypeDefinitionTranslation
{
    /// <summary>
    /// Default implementation of a <see cref="ITypeDefinitionTranslator"/> that makes use of all known strategies.
    /// </summary>
    public sealed class DefaultTypeDefinitionTranslator : ITypeDefinitionTranslator
    {
        private readonly ILogger _logger;
        private readonly IEnumerable<ITypeDefinitionTranslationStrategy> _strategies;

        /// <summary>
        /// Creates a <see cref="DefaultTypeDefinitionTranslator"/>.
        /// </summary>
        /// <param name="configurationSource">Source for the configuration that should be used.</param>
        /// <param name="templatingEngine">Engine to use for loading templates.</param>
        /// <param name="documentationSource">Source for looking up documentation comments for members.</param>
        /// <param name="logger">Logger to use for writing log messages.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="templatingEngine"/> or <paramref name="documentationSource"/> or <paramref name="logger"/> is null.</exception>
        public DefaultTypeDefinitionTranslator(IConfigurationSource configurationSource, ITemplatingEngine templatingEngine, IDocumentationSource documentationSource, ILogger logger)
        {
            if (configurationSource == null) throw new ArgumentNullException(nameof(configurationSource));
            if (templatingEngine == null) throw new ArgumentNullException(nameof(templatingEngine));
            if (documentationSource == null) throw new ArgumentNullException(nameof(documentationSource));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _strategies = new ITypeDefinitionTranslationStrategy[]
            {
                new ClassDefinitionTranslationStrategy(configurationSource, templatingEngine, documentationSource, logger),
                new EnumDefinitionTranslator(configurationSource, templatingEngine, documentationSource, logger)
            };
        }

        /// <summary>
        /// Translates the definition of the specified type.
        /// </summary>
        /// <param name="type">Type for which the definition should be translated.</param>
        /// <returns>A <see cref="CodeFragment"/> with the definition of the specified type.</returns>
        public CodeFragment Translate(Type type)
        {
            var strategy = _strategies.FirstOrDefault(s => s.CanTranslateTypeDefinition(type));
            if (strategy == null)
            {
                throw new NotSupportedException($"No strategy to translate definition of type {type}.");
            }

            _logger.WriteInformation($"Using {strategy.GetType()} to translate definition of type {type}.");
            return strategy.Translate(type);
        }
    }
}