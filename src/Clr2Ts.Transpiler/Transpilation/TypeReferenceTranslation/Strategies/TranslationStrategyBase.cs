using System;

namespace Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation.Strategies
{
    /// <summary>
    /// Base for strategies for translating type references that
    /// use a full translator for parts of the complete type reference.
    /// </summary>
    public abstract class TranslationStrategyBase: ITypeReferenceTranslationStrategy
    {
        private ITypeReferenceTranslator _translator;

        /// <summary>
        /// Creates a <see cref="TranslationStrategyBase"/>.
        /// </summary>
        /// <param name="translator">Full translator that can be used to translate parts of the complete type reference.</param>
        protected TranslationStrategyBase(ITypeReferenceTranslator translator)
        {
            _translator = translator ?? throw new ArgumentNullException(nameof(translator));
        }

        /// <summary>
        /// Determines if this strategy can be used to translate the specified type reference.
        /// </summary>
        /// <param name="type">Type reference that should be translated.</param>
        /// <returns>True, if this strategy can be used to translate the specified type reference; otherwise false.</returns>
        public bool CanTranslateTypeReference(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return CanTranslate(type);
        }

        /// <summary>
        /// Translates the specified type reference.
        /// </summary>
        /// <param name="referencedType">Type reference that should be tranlated.</param>
        /// <returns>Result of the translation.</returns>
        public TypeReferenceTranslationResult Translate(Type referencedType)
        {
            if (!CanTranslateTypeReference(referencedType))
            {
                throw new InvalidOperationException("Called Translate although CanTranslateTypeReference returns false.");
            }

            return Translate(referencedType, _translator);
        }

        /// <summary>
        /// Is overridden to define which type references can be translated by this strategy.
        /// </summary>
        /// <param name="type">Type reference that should be translated.</param>
        /// <returns>True, if this strategy can be used to translate the specified type reference; otherwise false.</returns>
        protected abstract bool CanTranslate(Type type);

        /// <summary>
        /// Is overridden to define how the type reference is translated by this strategy.
        /// </summary>
        /// <param name="referencedType">Type reference that should be tranlated.</param>
        /// <param name="translator">Full translator that can be used to translate parts of the complete type reference.</param>
        /// <returns>Result of the translation.</returns>
        protected abstract TypeReferenceTranslationResult Translate(Type referencedType, ITypeReferenceTranslator translator);
    }
}