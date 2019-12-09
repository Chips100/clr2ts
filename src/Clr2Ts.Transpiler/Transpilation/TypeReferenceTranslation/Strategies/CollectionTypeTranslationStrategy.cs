using System;
using System.Collections.Generic;
using System.Linq;

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
            => GetCollectionElementType(type) != null;

        /// <summary>
        /// Is overridden to define how the type reference is translated by this strategy.
        /// </summary>
        /// <param name="referencedType">Type reference that should be tranlated.</param>
        /// <param name="translator">Full translator that can be used to translate parts of the complete type reference.</param>
        /// <returns>Result of the translation.</returns>
        protected override TypeReferenceTranslationResult Translate(Type referencedType, ITypeReferenceTranslator translator)
        {
            var elementType = GetCollectionElementType(referencedType);
            var translatedElement = translator.Translate(elementType);

            return new TypeReferenceTranslationResult(
                $"Array<{ translatedElement.ReferencedTypeName }>",
                translatedElement.Dependencies);
        }

        private Type GetCollectionElementType(Type collectionType)
        {
            // Check if the type is the IEnumerable interface itself,
            // otherwise look for the implemented IEnumerable interface.
            var enumerableInterface = GetSelfEnumerableType(collectionType)
                ?? GetImplementedEnumerableType(collectionType);

            return enumerableInterface?.GetGenericArguments().Single();
        }

        private Type GetSelfEnumerableType(Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>) ? type : null;

        private Type GetImplementedEnumerableType(Type type)
            => type.GetInterface(typeof(IEnumerable<>).Name);
    }
}