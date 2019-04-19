using System;
using System.Collections.Generic;
using System.Linq;

namespace Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation
{
    /// <summary>
    /// Represents the result of a translated type reference.
    /// </summary>
    public sealed class TypeReferenceTranslationResult
    {
        /// <summary>
        /// Creates a <see cref="TypeReferenceTranslationResult"/>.
        /// </summary>
        /// <param name="referencedTypeName">Name, by which the type should be referenced in the translation.</param>
        /// <param name="dependencies">Dependencies that are required by referencing the type.</param>
        public TypeReferenceTranslationResult(string referencedTypeName, IEnumerable<CodeFragmentId> dependencies)
        {
            if (dependencies == null) throw new ArgumentNullException(nameof(dependencies));
            if (string.IsNullOrWhiteSpace(referencedTypeName)) throw new ArgumentException("ReferencedTypeName cannot be empty.", nameof(referencedTypeName));

            ReferencedTypeName = referencedTypeName;
            Dependencies = dependencies.ToList();
        }

        /// <summary>
        /// Gets the name, by which the type should be referenced in the translation.
        /// </summary>
        public string ReferencedTypeName { get; }

        /// <summary>
        /// Gets the dependencies that are required by referencing the type.
        /// </summary>
        public IEnumerable<CodeFragmentId> Dependencies { get; }
    }
}