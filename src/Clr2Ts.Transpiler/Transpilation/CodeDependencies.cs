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
        /// <summary>
        /// Creates a set of dependencies for a code fragment.
        /// </summary>
        /// <param name="codeFragments">Dependencies on other code fragments of the transpiled code base.</param>
        /// <param name="imports">Dependencies on TypeScript artifact defined outside of the transpiled codebase.</param>
        private CodeDependencies(IEnumerable<CodeFragmentId> codeFragments, IEnumerable<Import> imports)
        {
            if (codeFragments == null) throw new ArgumentNullException(nameof(codeFragments));
            if (imports == null) throw new ArgumentNullException(nameof(imports));

            CodeFragments = codeFragments.Distinct().ToList();
            Imports = imports.Distinct().ToList();
        }

        /// <summary>
        /// Gets the dependencies on other code fragments of the transpiled code base.
        /// </summary>
        public IEnumerable<CodeFragmentId> CodeFragments { get; }

        /// <summary>
        /// Gets the dependencies on TypeScript artifact defined outside of the transpiled codebase.
        /// </summary>
        public IEnumerable<Import> Imports { get; }

        /// <summary>
        /// Removes the dependency on a specific code fragment from the current set of dependencies.
        /// </summary>
        /// <param name="id">ID of the code fragment to remove from the current set of dependencies.</param>
        /// <returns>The set of dependencies without the specified code fragment.</returns>
        public CodeDependencies WithoutCodeFragment(CodeFragmentId id)
        {
            return new CodeDependencies(
                CodeFragments.Except(new[] { id }),
                Imports);
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
    }
}