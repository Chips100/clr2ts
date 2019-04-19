using System;
using System.Collections.Generic;
using System.Linq;
using Clr2Ts.Transpiler.Transpilation;

namespace Clr2Ts.Transpiler.Output
{
    /// <summary>
    /// Creates a composite CodeWriter that delegates to multiple other CodeWriters.
    /// </summary>
    public sealed class CompositeCodeWriter : ICodeWriter
    {
        private IEnumerable<ICodeWriter> _codeWriters;

        /// <summary>
        /// Creates a <see cref="CompositeCodeWriter"/>.
        /// </summary>
        /// <param name="codeWriters">Underlying CodeWriters that should be used.</param>
        public CompositeCodeWriter(IEnumerable<ICodeWriter> codeWriters)
        {
            if (codeWriters == null) throw new ArgumentNullException(nameof(codeWriters));

            // Create own private copy of the sequence.
            _codeWriters = codeWriters.ToList();
        }

        /// <summary>
        /// Writes the specified set of code fragments to the output.
        /// </summary>
        /// <param name="codeFragments">Code fragments that should be written.</param>
        public void Write(IEnumerable<CodeFragment> codeFragments)
        {
            // Enforce one-time evaluation.
            codeFragments = codeFragments.ToList();

            // Let each writer write the result.
            foreach (var codeWriter in _codeWriters) codeWriter.Write(codeFragments);
        }
    }
}