using System;

namespace Clr2Ts.Transpiler.Transpilation.TypeDefinitionTranslation
{
    /// <summary>
    /// Translates definitions of arbitrary types.
    /// </summary>
    public interface ITypeDefinitionTranslator
    {
        /// <summary>
        /// Translates the definition of the specified type.
        /// </summary>
        /// <param name="type">Type for which the definition should be translated.</param>
        /// <returns>A <see cref="CodeFragment"/> with the definition of the specified type.</returns>
        CodeFragment Translate(Type type);
    }
}