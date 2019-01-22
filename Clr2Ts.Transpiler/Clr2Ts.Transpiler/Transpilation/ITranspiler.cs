using System;
using System.Collections.Generic;

namespace Clr2Ts.Transpiler.Transpilation
{
    public interface ITranspiler
    {
        TranspilationResult Transpile(IEnumerable<Type> types);
    }
}
