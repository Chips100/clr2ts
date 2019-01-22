using System;
using System.Collections.Generic;
using System.Linq;

namespace Clr2Ts.Transpiler.Configuration
{
    public class InputConfiguration
    {
        public InputConfiguration(IEnumerable<string> assemblyFiles)
        {
            AssemblyFiles = assemblyFiles?.ToList() ?? Enumerable.Empty<string>();
        }

        public IEnumerable<string> AssemblyFiles { get; }
    }
}
