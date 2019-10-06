using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Logging;
using Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation
{
    /// <summary>
    /// Default implementation of a <see cref="ITypeReferenceTranslator"/> that makes use of all known strategies.
    /// </summary>
    public sealed class DefaultTypeReferenceTranslator: ITypeReferenceTranslator
    {
        private readonly IEnumerable<ITypeReferenceTranslationStrategy> _strategies;
        private readonly ILogger _logger;

        /// <summary>
        /// Creates a <see cref="DefaultTypeReferenceTranslator"/>.
        /// </summary>
        /// <param name="configurationSource">Source for the configuration that should be used.</param>
        /// <param name="logger">Logger to use for writing log messages.</param>
        public DefaultTypeReferenceTranslator(IConfigurationSource configurationSource, ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _strategies = new ITypeReferenceTranslationStrategy[]
            {
                new GenericArgumentTranslationStrategy(),
                new NullableTypeTranslationStrategy(configurationSource, this),
                new BuiltInTypeTranslationStrategy(configurationSource, this),
                new DictionaryTypeTranslationStrategy(configurationSource, this),
                new CollectionTypeTranslationStrategy(configurationSource, this),
                new GenericTypeTranslationStrategy(configurationSource, this),
                new CustomMappedTranslationStrategy(configurationSource, this),
                new DefaultTranslationStrategy()
            };
        }

        /// <summary>
        /// Translates the specified type reference.
        /// </summary>
        /// <param name="referencedType">Type reference that should be tranlated.</param>
        /// <returns>Result of the translation.</returns>
        public TypeReferenceTranslationResult Translate(Type referencedType)
        {
            var strategy = _strategies.First(s => s.CanTranslateTypeReference(referencedType));
            _logger.WriteInformation($"Using {strategy.GetType()} to translate reference to type {referencedType}.");

            return strategy.Translate(referencedType);
        }
    }
}