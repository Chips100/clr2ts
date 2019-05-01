using System;
using System.Linq;

namespace Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation.Strategies
{
    /// <summary>
    /// Strategy for translating a reference to a generic type argument.
    /// </summary>
    public sealed class GenericArgumentTranslationStrategy: ITypeReferenceTranslator
    {
        /// <summary>
        /// Is overridden to define which type references can be translated by this strategy.
        /// </summary>
        /// <param name="type">Type reference that should be translated.</param>
        /// <returns>True, if this strategy can be used to translate the specified type reference; otherwise false.</returns>
        public bool CanTranslateTypeReference(Type type) => type.IsGenericParameter;

        /// <summary>
        /// Is overridden to define how the type reference is translated by this strategy.
        /// </summary>
        /// <param name="referencedType">Type reference that should be tranlated.</param>
        /// <param name="translator">Full translator that can be used to translate parts of the complete type reference.</param>
        /// <returns>Result of the translation.</returns>
        public TypeReferenceTranslationResult Translate(Type referencedType)
            // Simple: Generic parameters are available by their name.
            => new TypeReferenceTranslationResult(referencedType.Name, Enumerable.Empty<CodeFragmentId>());
    }
}