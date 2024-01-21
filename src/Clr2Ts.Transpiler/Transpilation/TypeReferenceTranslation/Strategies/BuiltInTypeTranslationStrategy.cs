using System;
using System.Collections.Generic;

namespace Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation.Strategies
{
    /// <summary>
    /// Strategy for translating a reference to a type that maps to a TypeScript built-in type.
    /// </summary>
    public sealed class BuiltInTypeTranslationStrategy : TranslationStrategyBase
    {
        private static readonly IDictionary<Type, string> BuiltInTypes = new Dictionary<Type, string>
        {
            { typeof(byte), "number" },
            { typeof(short), "number" },
            { typeof(int), "number" },
            { typeof(long), "number" },
            { typeof(float), "number" },
            { typeof(double), "number" },
            { typeof(decimal), "number" },
            { typeof(bool), "boolean" },
            { typeof(DateTime), "Date" },
            { typeof(string), "string" },
            { typeof(Guid), "string" }, // special case: no real Guid type in TypeScript.
            { typeof(object), "any" },

            // Make new basic types from net6 work out of the box;
            // can be overriden by custom type maps.
            { typeof(DateOnly), "Date" },
            { typeof(TimeOnly), "string" }
        };

        /// <summary>
        /// Creates a <see cref="BuiltInTypeTranslationStrategy"/>.
        /// </summary>
        /// <param name="translator">Full translator that can be used to translate parts of the complete type reference.</param>
        public BuiltInTypeTranslationStrategy(ITypeReferenceTranslator translator)
            : base(translator)
        { }

        /// <summary>
        /// Is overridden to define which type references can be translated by this strategy.
        /// </summary>
        /// <param name="type">Type reference that should be translated.</param>
        /// <returns>True, if this strategy can be used to translate the specified type reference; otherwise false.</returns>
        protected override bool CanTranslate(Type type)
            => BuiltInTypes.ContainsKey(type);

        /// <summary>
        /// Is overridden to define how the type reference is translated by this strategy.
        /// </summary>
        /// <param name="referencedType">Type reference that should be tranlated.</param>
        /// <param name="translator">Full translator that can be used to translate parts of the complete type reference.</param>
        /// <returns>Result of the translation.</returns>
        protected override TypeReferenceTranslationResult Translate(Type referencedType, ITypeReferenceTranslator translator)
            => new TypeReferenceTranslationResult(BuiltInTypes[referencedType], CodeDependencies.Empty);
    }
}