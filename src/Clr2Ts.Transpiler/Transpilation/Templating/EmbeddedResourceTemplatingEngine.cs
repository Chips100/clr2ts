using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Clr2Ts.Transpiler.Transpilation.Templating
{
    /// <summary>
    /// Allows usage of templates with simple placeholders stored in embedded resources.
    /// </summary>
    public sealed class EmbeddedResourceTemplatingEngine : ITemplatingEngine
    {
        private readonly string _subFolder;

        private EmbeddedResourceTemplatingEngine(string subFolder)
        {
            _subFolder = subFolder;
        }

        /// <summary>
        /// Creates a TemplatingEngine by reading embedded resources with TypeScript code snippets.
        /// </summary>
        /// <returns>The TemplatingEngine based on embedded resources.</returns>
        public static ITemplatingEngine ForTypeScript()
            => new EmbeddedResourceTemplatingEngine("TypeScriptCodeTemplates");

        /// <summary>
        /// Uses a template with the specified texts for its placeholders.
        /// </summary>
        /// <param name="templateName">Name of the template to use.</param>
        /// <param name="replacements">Texts that should be used to replace placeholders.</param>
        /// <returns>The resulting text that was created from the template.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="templateName"/> refers to an unknown template.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="replacements"/> is null.</exception>
        public string UseTemplate(string templateName, IDictionary<string, string> replacements)
        {
            if (replacements == null) throw new ArgumentNullException(nameof(replacements));

            // Read embedded resource that represents the template.
            var templateFullName = $"{GetType().Namespace}.{_subFolder}.{templateName}.txt";
            var stream = GetType().Assembly.GetManifestResourceStream(templateFullName);
            if (stream == null) throw new ArgumentOutOfRangeException(nameof(templateName), $"Unknown template: {templateName}");

            using (stream)
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