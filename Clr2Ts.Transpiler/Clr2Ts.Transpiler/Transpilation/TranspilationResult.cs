using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clr2Ts.Transpiler.Transpilation
{
    public class TranspilationResult
    {
        public TranspilationResult(IEnumerable<CodeFragment> codeFragments)
        {
            if (codeFragments == null) throw new ArgumentNullException(nameof(codeFragments));

            CodeFragments = codeFragments.ToList();
        }

        public IEnumerable<CodeFragment> CodeFragments { get; }
    }
}