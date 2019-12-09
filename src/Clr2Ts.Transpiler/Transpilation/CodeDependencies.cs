using System;
using System.Collections.Generic;
using System.Linq;

namespace Clr2Ts.Transpiler.Transpilation
{
    /// <summary>
    /// Represents a set of dependencies that a code fragment requires.
    /// </summary>
    public sealed class CodeDependencies
    {
        // Use sets to store the dependencies (have to be distinct),
        // but expose IEnumerables for immutability.
        private readonly ISet<CodeFragmentId> _codeFragments;
        private readonly ISet<Import> _imports;

        /// <summary>
        /// Creates a set of dependencies for a code fragment.
        /// </summary>
        /// <param name="codeFragments">Dependencies on other code fragments of the transpiled code base.</param>
        /// <param name="imports">Dependencies on TypeScript artifact defined outside of the transpiled codebase.</param>
        private CodeDependencies(IEnumerable<CodeFragmentId> codeFragments, IEnumerable<Import> imports)
        {
            if (codeFragments == null) throw new ArgumentNullException(nameof(codeFragments));
            if (imports == null) throw new ArgumentNullException(nameof(imports));

            _codeFragments = new HashSet<CodeFragmentId>(codeFragments);
            _imports = new HashSet<Import>(imports);
        }

        /// <summary>
        /// Gets the dependencies on other code fragments of the transpiled code base.
        /// </summary>
        public IEnumerable<CodeFragmentId> CodeFragments => _codeFragments;

        /// <summary>
        /// Gets the dependencies on TypeScript artifact defined outside of the transpiled codebase.
        /// </summary>
        public IEnumerable<Import> Imports => _imports;

        /// <summary>
        /// Removes the dependency on a specific code fragment from the current set of dependencies.
        /// </summary>
        /// <param name="id">ID of the code fragment to remove from the current set of dependencies.</param>
        /// <returns>The set of dependencies without the specified code fragment.</returns>
        public CodeDependencies WithoutCodeFragment(CodeFragmentId id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            return new CodeDependencies(
                CodeFragments.Except(new[] { id }),
                Imports);
        }

        /// <summary>
        /// Merges this set of dependencies with another set of dependencies.
        /// </summary>
        /// <param name="dependencies">Other dependencies that should be added onto the current dependencies.</param>
        /// <returns>A set of dependencies that contains all dependencies of the merged sets.</returns>
        public CodeDependencies Merge(CodeDependencies dependencies)
        {
            if (dependencies == null) throw new ArgumentNullException(nameof(dependencies));

            // Just concatenate the sequences; distinct is done
            // in construction of the resulting set.
            return new CodeDependencies(
                CodeFragments.Concat(dependencies.CodeFragments),
                Imports.Concat(dependencies.Imports));
        }

        /// <summary>
        /// Gets an empty set of dependencies.
        /// </summary>
        public static CodeDependencies Empty { get; }
            // Use singleton instance as it cannot be mutated.
            = new CodeDependencies(
                Enumerable.Empty<CodeFragmentId>(),
                Enumerable.Empty<Import>());

        /// <summary>
        /// Creates a set of dependencies referencing the specified code fragments.
        /// </summary>
        /// <param name="ids">IDs of the code fragments that should make up the set of dependencies.</param>
        /// <returns>A set of dependencies referencing the specified code fragments.</returns>
        public static CodeDependencies FromCodeFragments(IEnumerable<CodeFragmentId> ids)
        {
            if (ids == null) throw new ArgumentNullException(nameof(ids));

            return new CodeDependencies(ids, Enumerable.Empty<Import>());
        }

        /// <summary>
        /// Creates a set of dependencies referencing the specified imports
        /// of TypeScript artifacts defined outside of the transpiled codebase.
        /// </summary>
        /// <param name="imports">Imports of TypeScript artifacts that should make up the set of dependencies.</param>
        /// <returns>A set of dependencies referencing the specified TypeScript artifacts.</returns>
        public static CodeDependencies FromImports(IEnumerable<Import> imports)
        {
            if (imports == null) throw new ArgumentNullException(nameof(imports));

            return new CodeDependencies(Enumerable.Empty<CodeFragmentId>(), imports);
        }


        public override bool Equals(object obj)
            => obj is CodeDependencies dependencies
                && _codeFragments.SetEquals(dependencies._codeFragments)
                && _imports.SetEquals(dependencies._imports);

        public override int GetHashCode()
            // For now, just use the total count of the dependencies.
            // This may lead to unnecessary call of Equals, but should otherwise be harmless,
            // as this object is not used as keys in dictionaries yet.
            => _codeFragments.Count + _imports.Count;

        public static bool operator ==(CodeDependencies a, CodeDependencies b)
            => a?.Equals(b) ?? b == null;

        public static bool operator !=(CodeDependencies a, CodeDependencies b)
            => !(a == b);
    }
}