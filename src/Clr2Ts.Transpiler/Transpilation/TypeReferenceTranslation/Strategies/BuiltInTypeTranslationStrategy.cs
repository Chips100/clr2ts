using System;
using System.Collections.Generic;
using System.Linq;

namespace Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation.Strategies
{
    public sealed class BuiltInTypeTranslationStrategy : TranslationStrategyBase
    {
        // Maps built-in types from .NET to built-in types from TypeScript.
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
            { typeof(object), "any" }
        };

        public BuiltInTypeTranslationStrategy(TypeReferenceTranslator translator) : base(translator)
        { }

        protected override bool CanTranslate(Type type)
            => BuiltInTypes.ContainsKey(type);

        protected override TypeReferenceTranslationResult Translate(Type referencedType, TypeReferenceTranslator translator)
            => new TypeReferenceTranslationResult(BuiltInTypes[referencedType], Enumerable.Empty<CodeFragmentId>());
    }
}