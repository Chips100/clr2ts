using System;
using System.Collections.Generic;
using System.Linq;

namespace Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation.Strategies
{
    /// <summary>
    /// Strategy for translating a reference to a dictionary type.
    /// </summary>
    public sealed class DictionaryTypeTranslationStrategy: TranslationStrategyBase
    {
        /// <summary>
        /// Creates a <see cref="DictionaryTypeTranslationStrategy"/>.
        /// </summary>
        /// <param name="translator">Full translator that can be used to translate parts of the complete type reference.</param>
        public DictionaryTypeTranslationStrategy(ITypeReferenceTranslator translator) : base(translator)
        { }

        /// <summary>
        /// Is overridden to define which type references can be translated by this strategy.
        /// </summary>
        /// <param name="type">Type reference that should be translated.</param>
        /// <returns>True, if this strategy can be used to translate the specified type reference; otherwise false.</returns>
        protected override bool CanTranslate(Type type)
            => GetDictionaryType(type) != null;

        /// <summary>
        /// Is overridden to define how the type reference is translated by this strategy.
        /// </summary>
        /// <param name="referencedType">Type reference that should be tranlated.</param>
        /// <param name="translator">Full translator that can be used to translate parts of the complete type reference.</param>
        /// <returns>Result of the translation.</returns>
        protected override TypeReferenceTranslationResult Translate(Type referencedType, ITypeReferenceTranslator translator)
        {
            var dictionaryType = GetDictionaryType(referencedType);
            var translatedKeyType = translator.Translate(dictionaryType.GetGenericArguments()[0]);
            var translatedValueType = translator.Translate(dictionaryType.GetGenericArguments()[1]);

            return new TypeReferenceTranslationResult(
                $"{{[key: { translatedKeyType.ReferencedTypeName }]: { translatedValueType.ReferencedTypeName } }}",
                translatedKeyType.Dependencies.Concat(translatedValueType.Dependencies));
        }



        private Type GetDictionaryType(Type type)
            => GetSelfDictionaryType(type) ?? GetImplementedDictionaryType(type);

        private Type GetSelfDictionaryType(Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>) ? type : null;

        private Type GetImplementedDictionaryType(Type type)
            => type.GetInterface(typeof(IDictionary<,>).Name);
    }
}