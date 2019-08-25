using System;
using System.Collections.Generic;
using System.Linq;

namespace Clr2Ts.Transpiler.Transpilation
{
    /// <summary>
    /// Represents a code fragment.
    /// </summary>
    public sealed class CodeFragment
    {
        /// <summary>
        /// Creates a <see cref="CodeFragment"/>.
        /// </summary>
        /// <param name="id">ID that is used to refer to this code fragment.</param>
        /// <param name="dependencies">Dependencies that this code fragment requires to be valid.</param>
        /// <param name="code">Actual code of this code fragment.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> or <paramref name="dependencies"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="code"/> is null or empty.</exception>
        public CodeFragment(CodeFragmentId id, IEnumerable<CodeFragmentId> dependencies, string code)
        {
            if (dependencies == null) throw new ArgumentNullException(nameof(dependencies));
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code cannot be empty.", nameof(code));

            Id = id ?? throw new ArgumentNullException(nameof(id));
            Code = code;
            Dependencies = dependencies
                // A code fragment does not need to indicate a dependency on itself.
                .Where(d => d != id)
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// Gets the ID that is used to refer to this code fragment.
        /// </summary>
        public CodeFragmentId Id { get; }

        /// <summary>
        /// Gets the dependencies that this code fragment requires to be valid.
        /// </summary>
        public IEnumerable<CodeFragmentId> Dependencies { get; }

        /// <summary>
        /// Gets the actual code of this code fragment.
        /// </summary>
        public string Code { get; }
    }
}