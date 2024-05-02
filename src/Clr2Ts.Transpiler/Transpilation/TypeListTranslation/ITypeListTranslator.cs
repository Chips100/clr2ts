using System;
using System.Collections.Generic;

namespace Clr2Ts.Transpiler.Transpilation.TypeListTranslation
{
    public interface ITypeListTranslator
    {
        /// <summary>
        /// Translates the definition of the specified type.
        /// </summary>
        /// <param name="types">Types for which the definition should be translated.</param>
        /// <returns>A <see cref="CodeFragment"/> with the definition of the specified type.</returns>
        IEnumerable<CodeFragment> Translate(IEnumerable<Type> types);
    }
}