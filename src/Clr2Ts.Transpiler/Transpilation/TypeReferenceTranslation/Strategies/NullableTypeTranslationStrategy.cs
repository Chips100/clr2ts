using System;

namespace Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation.Strategies
{
    public sealed class NullableTypeTranslationStrategy: TranslationStrategyBase
    {
        public NullableTypeTranslationStrategy(TypeReferenceTranslator translator): base(translator)
        { }

        protected override bool CanTranslate(Type type)
            => Nullable.GetUnderlyingType(type) != null;

        protected override TypeReferenceTranslationResult Translate(Type referencedType, TypeReferenceTranslator translator)
            => translator.Translate(Nullable.GetUnderlyingType(referencedType));
    }
}