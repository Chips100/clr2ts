using System;
using System.Collections.Generic;
using System.Linq;

namespace Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation
{
    public sealed class TypeReferenceTranslationResult
    {
        public TypeReferenceTranslationResult(string referencedTypeName, IEnumerable<CodeFragmentId> dependencies)
        {
            if (dependencies == null) throw new ArgumentNullException(nameof(dependencies));
            if (string.IsNullOrWhiteSpace(referencedTypeName)) throw new ArgumentException("ReferencedTypeName cannot be empty.", nameof(referencedTypeName));

            ReferencedTypeName = referencedTypeName;
            Dependencies = dependencies.ToList();
        }

        public string ReferencedTypeName { get; }

        public IEnumerable<CodeFragmentId> Dependencies { get; }
    }
}