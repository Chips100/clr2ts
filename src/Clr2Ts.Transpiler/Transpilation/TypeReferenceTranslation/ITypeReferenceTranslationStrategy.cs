using System;
using System.Collections.Generic;
using System.Text;

namespace Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation
{
    public interface ITypeReferenceTranslationStrategy
    {
        bool CanTranslateTypeReference(Type type);

        TypeReferenceTranslationResult Translate(Type referencedType);
    }
}