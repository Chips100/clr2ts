using System;
using System.Collections.Generic;
using System.Reflection;

namespace Clr2Ts.Transpiler.Input
{
    public class AssemblyScanner
    {
        public IEnumerable<Type> GetTypesForTranspilation(string assemblyFile)
        {
            var assembly = Assembly.LoadFile(assemblyFile);

            return assembly.GetTypes();
        }
    }
}