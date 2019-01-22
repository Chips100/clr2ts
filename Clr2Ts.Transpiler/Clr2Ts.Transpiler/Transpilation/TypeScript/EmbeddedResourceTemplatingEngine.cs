using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Clr2Ts.Transpiler.Transpilation.TypeScript
{
    public class EmbeddedResourceTemplatingEngine: ITemplatingEngine
    {
        public string UseTemplate(string templateName, IDictionary<string, string> replacements)
        {
            var templateFullName = $"{GetType().Namespace}.TypeScriptCodeTemplates.{templateName}.txt";

            using (var stream = GetType().Assembly.GetManifestResourceStream(templateFullName))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var templateContent = new StringBuilder(reader.ReadToEnd());

                foreach (var replacement in replacements)
                {
                    templateContent = templateContent.Replace($"[{replacement.Key}]", replacement.Value);
                }

                return templateContent.ToString();
            }
        }
    }
}