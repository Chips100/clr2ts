using System;

namespace Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation.Strategies
{
    public abstract class TranslationStrategyBase: ITypeReferenceTranslationStrategy
    {
        private TypeReferenceTranslator _translator;

        protected TranslationStrategyBase(TypeReferenceTranslator translator)
        {
            _translator = translator ?? throw new ArgumentNullException(nameof(translator));
        }

        public bool CanTranslateTypeReference(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return CanTranslate(type);
        }

        public TypeReferenceTranslationResult Translate(Type referencedType)
        {
            if (!CanTranslateTypeReference(referencedType))
            {
                throw new InvalidOperationException("TODO: Called Translate although CanTranslateTypeReference returns false.");
            }

            return Translate(referencedType, _translator);
        }

        protected abstract bool CanTranslate(Type type);

        protected abstract TypeReferenceTranslationResult Translate(Type referencedType, TypeReferenceTranslator translator);
    }
}