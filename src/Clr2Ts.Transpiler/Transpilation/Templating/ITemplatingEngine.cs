using System;
using System.Collections.Generic;

namespace Clr2Ts.Transpiler.Transpilation.Templating
{
    /// <summary>
    /// Allows usage of templates with simple placeholders.
    /// </summary>
    public interface ITemplatingEngine
    {
        /// <summary>
        /// Uses a template with the specified texts for its placeholders.
        /// </summary>
        /// <param name="templateName">Name of the template to use.</param>
        /// <param name="replacements">Texts that should be used to replace placeholders.</param>
        /// <returns>The resulting text that was created from the template.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="templateName"/> refers to an unknown template.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="replacements"/> is null.</exception>
        string UseTemplate(string templateName, IDictionary<string, string> replacements);
    }
}