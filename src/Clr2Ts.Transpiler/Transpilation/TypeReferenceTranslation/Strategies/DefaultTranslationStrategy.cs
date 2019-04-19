using System;

namespace Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation.Strategies
{
    public class DefaultTranslationStrategy : ITypeReferenceTranslationStrategy
    {
        public bool CanTranslateTypeReference(Type type) => true;

        public TypeReferenceTranslationResult Translate(Type referencedType)
            => new TypeReferenceTranslationResult(referencedType.Name, new[] { CodeFragmentId.ForClrType(referencedType) });
    }
}