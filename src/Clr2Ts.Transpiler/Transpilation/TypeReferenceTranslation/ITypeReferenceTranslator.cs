using System;

namespace Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation
{
    /// <summary>
    /// Translates references to arbitrary types.
    /// </summary>
    public interface ITypeReferenceTranslator
    {
        /// <summary>
        /// Translates the specified type reference.
        /// </summary>
        /// <param name="referencedType">Type reference that should be tranlated.</param>
        /// <returns>Result of the translation.</returns>
        TypeReferenceTranslationResult Translate(Type referencedType);
    }
}