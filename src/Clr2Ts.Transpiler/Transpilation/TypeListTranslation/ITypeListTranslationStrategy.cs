﻿using System;
using System.Collections.Generic;

namespace Clr2Ts.Transpiler.Transpilation.TypeListTranslation
{
    /// <summary>
    /// Strategy for translating the definition of a type.
    /// </summary>
    public interface ITypeListTranslationStrategy
    {
        /// <summary>
        /// Determines if this strategy can be used to translate the specified type definition.
        /// </summary>
        /// <param name="type">Type definition that should be translated.</param>
        /// <returns>True, if this strategy can be used to translate the specified type definition; otherwise false.</returns>
        bool CanTranslateTypeDefinition(Type type);

        /// <summary>
        /// Translates the specified type definition.
        /// </summary>
        /// <param name="referencedType">Type definition that should be translated.</param>
        /// <returns>Result of the translation.</returns>
        (string code, IEnumerable<Import> imports) Translate(Type referencedType);
    }
}