using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Clr2Ts.Transpiler.Input
{
    /// <summary>
    /// Allows scanning of an assembly file for types that should be transpiled.
    /// </summary>
    public sealed class AssemblyScanner
    {
        /// <summary>
        /// Scans the specified assembly file for types that should be transpiled.
        /// </summary>
        /// <param name="assemblyFile">Name of the assembly file.</param>
        /// <returns>A sequence with the types from the assembly that should be transpiled.</returns>
        public IEnumerable<Type> GetTypesForTranspilation(string assemblyFile)
        {
            // Support relative paths by resolving them first.
            var fileInfo = new FileInfo(assemblyFile);

            // For now, just return all types of the assembly.
            return Assembly.LoadFile(fileInfo.FullName).GetTypes();
        }
    }
}