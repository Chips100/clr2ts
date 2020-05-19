using Clr2Ts.Transpiler.Extensions;
using System;

namespace Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation.Strategies
{
    /// <summary>
    /// Strategy for translating a reference to a collection type.
    /// </summary>
    public sealed class CollectionTypeTranslationStrategy : TranslationStrategyBase
    {
        /// <summary>
        /// Creates a <see cref="CollectionTypeTranslationStrategy"/>.
        /// </summary>
        /// <param name="translator">Full translator that can be used to translate parts of the complete type reference.</param>
        public CollectionTypeTranslationStrategy(ITypeReferenceTranslator translator)
            : base(translator)
        { }

        /// <summary>
        /// Is overridden to define which type references can be translated by this strategy.
        /// </summary>
        /// <param name="type">Type reference that should be translated.</param>
        /// <returns>True, if this strategy can be used to translate the specified type reference; otherwise false.</returns>
        protected override bool CanTranslate(Type type)
            => type.GetCollectionElementType() != null;

        /// <summary>
        /// Is overridden to define how the type reference is translated by this strategy.
        /// </summary>
        /// <param name="referencedType">Type reference that should be tranlated.</param>
        /// <param name="translator">Full translator that can be used to translate parts of the complete type reference.</param>
        /// <returns>Result of the translation.</returns>
        protected override TypeReferenceTranslationResult Translate(Type referencedType, ITypeReferenceTranslator translator)
        {
            var elementType = referencedType.GetCollectionElementType();
            var translatedElement = translator.Translate(elementType);

            return new TypeReferenceTranslationResult(
                $"Array<{ translatedElement.ReferencedTypeName }>",
                translatedElement.Dependencies);
        }
    }
}