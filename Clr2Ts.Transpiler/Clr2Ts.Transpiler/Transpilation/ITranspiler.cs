using System;
using System.Collections.Generic;

namespace Clr2Ts.Transpiler.Transpilation
{
    /// <summary>
    /// Allows transpiling .NET type definitions to new source code.
    /// </summary>
    public interface ITranspiler
    {
        /// <summary>
        /// Transpiles the specified .NET type definitions to new source code.
        /// </summary>
        /// <param name="types">Types that should be transpiled.</param>
        /// <returns>The result of the transpilation process.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="types"/> is null.</exception>
        TranspilationResult Transpile(IEnumerable<Type> types);
    }
}