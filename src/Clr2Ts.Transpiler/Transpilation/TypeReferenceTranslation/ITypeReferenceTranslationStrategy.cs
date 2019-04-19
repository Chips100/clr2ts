using System;

namespace Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation
{
    /// <summary>
    /// Strategy for translating a reference to an arbitrary type.
    /// </summary>
    public interface ITypeReferenceTranslationStrategy
    {
        /// <summary>
        /// Determines if this strategy can be used to translate the specified type reference.
        /// </summary>
        /// <param name="type">Type reference that should be translated.</param>
        /// <returns>True, if this strategy can be used to translate the specified type reference; otherwise false.</returns>
        bool CanTranslateTypeReference(Type type);

        /// <summary>
        /// Translates the specified type reference.
        /// </summary>
        /// <param name="referencedType">Type reference that should be tranlated.</param>
        /// <returns>Result of the translation.</returns>
        TypeReferenceTranslationResult Translate(Type referencedType);
    }
}