using Clr2Ts.Transpiler.Transpilation;
using System.Collections.Generic;

namespace Clr2Ts.Transpiler.Output
{
    /// <summary>
    /// Writes a set of code fragments to an output.
    /// </summary>
    public interface ICodeWriter
    {
        /// <summary>
        /// Writes the specified set of code fragments to the output.
        /// </summary>
        /// <param name="codeFragments">Code fragments that should be written.</param>
        void Write(IEnumerable<CodeFragment> codeFragments);
    }
}