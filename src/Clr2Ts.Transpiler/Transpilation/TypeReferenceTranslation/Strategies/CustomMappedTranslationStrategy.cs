using Clr2Ts.Transpiler.Configuration;
using System;
using System.Linq;

namespace Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation.Strategies
{
    /// <summary>
    /// Strategy for translating a reference to a mapped type.
    /// </summary>
    public sealed class CustomMappedTranslationStrategy : TranslationStrategyBase
    {
        /// <summary>
        /// Creates a <see cref="CustomMappedTranslationStrategy"/>.
        /// </summary>
        /// <param name="configurationSource">Source for the configuration that should be used.</param>
        /// <param name="translator">Full translator that can be used to translate parts of the complete type reference.</param>
        public CustomMappedTranslationStrategy(IConfigurationSource configurationSource, ITypeReferenceTranslator translator)
            : base(configurationSource, translator)
        { }

        /// <summary>
        /// Is overridden to define which type references can be translated by this strategy.
        /// </summary>
        /// <param name="type">Type reference that should be translated.</param>
        /// <returns>True, if this strategy can be used to translate the specified type reference; otherwise false.</returns>
        protected override bool CanTranslate(Type type)
            => Configuration.CustomTypeMaps.Any(m => m.MapsType(type));

        /// <summary>
        /// Is overridden to define how the type reference is translated by this strategy.
        /// </summary>
        /// <param name="referencedType">Type reference that should be tranlated.</param>
        /// <param name="translator">Full translator that can be used to translate parts of the complete type reference.</param>
        /// <returns>Result of the translation.</returns>
        protected override TypeReferenceTranslationResult Translate(Type referencedType, ITypeReferenceTranslator translator)
        {
            var map = Configuration.CustomTypeMaps.FirstOrDefault(m => m.MapsType(referencedType));

            return new TypeReferenceTranslationResult(
                map.Name, CodeDependencies.FromImports(new[] { new Import(map.Name, map.Source) }));
        }
    }
}