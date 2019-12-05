using System;

namespace Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation.Strategies
{
    /// <summary>
    /// Default strategy that assumes the type can be referenced by using its name
    /// and adding it as a dependency. Works when other transpiled types are referenced.
    /// </summary>
    public sealed class DefaultTranslationStrategy : ITypeReferenceTranslationStrategy
    {
        /// <summary>
        /// Is overridden to define which type references can be translated by this strategy.
        /// </summary>
        /// <param name="type">Type reference that should be translated.</param>
        /// <returns>True, if this strategy can be used to translate the specified type reference; otherwise false.</returns>
        public bool CanTranslateTypeReference(Type type) => true;

        /// <summary>
        /// Is overridden to define how the type reference is translated by this strategy.
        /// </summary>
        /// <param name="referencedType">Type reference that should be tranlated.</param>
        /// <param name="translator">Full translator that can be used to translate parts of the complete type reference.</param>
        /// <returns>Result of the translation.</returns>
        public TypeReferenceTranslationResult Translate(Type referencedType)
            => new TypeReferenceTranslationResult(
                referencedType.Name,
                CodeDependencies.FromCodeFragments(new[] { CodeFragmentId.ForClrType(referencedType) }));
    }
}