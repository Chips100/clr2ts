using Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation
{
    public sealed class TypeReferenceTranslator
    {
        private readonly IEnumerable<ITypeReferenceTranslationStrategy> _strategies;

        public TypeReferenceTranslator()
        {
            _strategies = new ITypeReferenceTranslationStrategy[]
            {
                new NullableTypeTranslationStrategy(this),
                new BuiltInTypeTranslationStrategy(this),
                new DefaultTranslationStrategy()
            };
        }

        public TypeReferenceTranslationResult Translate(Type referencedType)
            => _strategies.First(s => s.CanTranslateTypeReference(referencedType)).Translate(referencedType);
    }
}