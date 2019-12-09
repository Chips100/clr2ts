using Clr2Ts.Transpiler.Extensions;
using System;
using System.Linq;

namespace Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation.Strategies
{
    /// <summary>
    /// Strategy for translating a reference to a generic type.
    /// </summary>
    public sealed class GenericTypeTranslationStrategy : TranslationStrategyBase
    {
        /// <summary>
        /// Creates a <see cref="GenericTypeTranslationStrategy"/>.
        /// </summary>
        /// <param name="translator">Full translator that can be used to translate parts of the complete type reference.</param>
        public GenericTypeTranslationStrategy(ITypeReferenceTranslator translator)
            : base(translator)
        { }

        /// <summary>
        /// Is overridden to define which type references can be translated by this strategy.
        /// </summary>
        /// <param name="type">Type reference that should be translated.</param>
        /// <returns>True, if this strategy can be used to translate the specified type reference; otherwise false.</returns>
        protected override bool CanTranslate(Type type)
            => type.IsGenericType;

        /// <summary>
        /// Is overridden to define how the type reference is translated by this strategy.
        /// </summary>
        /// <param name="referencedType">Type reference that should be tranlated.</param>
        /// <param name="translator">Full translator that can be used to translate parts of the complete type reference.</param>
        /// <returns>Result of the translation.</returns>
        protected override TypeReferenceTranslationResult Translate(Type referencedType, ITypeReferenceTranslator translator)
        {
            var translatedTypeArguments = referencedType.GetGenericArguments().Select(translator.Translate);
            var referencedTypeName = referencedType.GetNameWithoutGenericTypeParameters()
                + $"<{ string.Join(", ", translatedTypeArguments.Select(x => x.ReferencedTypeName)) }>";

            return new TypeReferenceTranslationResult(referencedTypeName,
                CodeDependencies.FromCodeFragments(new[] { CodeFragmentId.ForClrType(referencedType.GetGenericTypeDefinition()) })
                    .Merge(translatedTypeArguments.Aggregate(CodeDependencies.Empty, (x, n) => x.Merge(n.Dependencies))));
        }
    }
}