using System.Collections.Generic;

namespace Clr2Ts.Transpiler.Transpilation.TypeScript
{
    public interface ITemplatingEngine
    {
        string UseTemplate(string templateName, IDictionary<string, string> replacements);
    }
}