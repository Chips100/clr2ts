using System;
using System.Collections.Generic;
using System.Linq;

namespace Clr2Ts.Transpiler.Transpilation
{
    public class CodeFragment
    {
        public CodeFragment(CodeFragmentId id, IEnumerable<CodeFragmentId> dependencies, string code)
        {
            if (dependencies == null) throw new ArgumentNullException(nameof(dependencies));
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code cannot be empty.", nameof(code));

            Id = id ?? throw new ArgumentNullException(nameof(id));
            Dependencies = dependencies.ToList();
            Code = code;
        }

        public CodeFragmentId Id { get; }

        public IEnumerable<CodeFragmentId> Dependencies { get; }

        public string Code { get; }
    }
}
