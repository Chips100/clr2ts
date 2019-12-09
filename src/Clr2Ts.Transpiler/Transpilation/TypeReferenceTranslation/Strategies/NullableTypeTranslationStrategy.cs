using System;

namespace Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation.Strategies
{
    /// <summary>
    /// Strategy for translating a reference to a nullable type.
    /// </summary>
    /// <remarks>As all types are nullable in TypeScript by default, this just unwraps the underlying type.</remarks>
    public sealed class NullableTypeTranslationStrategy : TranslationStrategyBase
    {
        /// <summary>
        /// Creates a <see cref="BuiltInTypeTranslationStrategy"/>.
        /// </summary>
        /// <param name="translator">Full translator that can be used to translate parts of the complete type reference.</param>
        public NullableTypeTranslationStrategy(ITypeReferenceTranslator translator)
            : base(translator)
        { }

        /// <summary>
        /// Is overridden to define which type references can be translated by this strategy.
        /// </summary>
        /// <param name="type">Type reference that should be translated.</param>
        /// <returns>True, if this strategy can be used to translate the specified type reference; otherwise false.</returns>
        protected override bool CanTranslate(Type type)
            => Nullable.GetUnderlyingType(type) != null;

        /// <summary>
        /// Is overridden to define how the type reference is translated by this strategy.
        /// </summary>
        /// <param name="referencedType">Type reference that should be tranlated.</param>
        /// <param name="translator">Full translator that can be used to translate parts of the complete type reference.</param>
        /// <returns>Result of the translation.</returns>
        protected override TypeReferenceTranslationResult Translate(Type referencedType, ITypeReferenceTranslator translator)
            => translator.Translate(Nullable.GetUnderlyingType(referencedType));
    }
}