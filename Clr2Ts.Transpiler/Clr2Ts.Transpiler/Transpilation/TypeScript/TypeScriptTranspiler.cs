using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Clr2Ts.Transpiler.Transpilation.TypeScript
{
    public class TypeScriptTranspiler
    {
        public CodeFragment Transpile(Type type)
        {
            var id = CodeFragmentId.ForClrType(type);
            var code = GenerateClassDefinition(type);
            var dependencies = GetPropertiesForTranspilation(type)
                .Select(p => p.PropertyType).Select(CodeFragmentId.ForClrType);

            return new CodeFragment(id, dependencies, code);
        }

        private string GenerateClassDefinition(Type type)
        {
            return UseTemplate("ClassDefinition", new Dictionary<string, string>
            {
                { "ClassName", type.Name },
                { "Properties", GeneratePropertyDefinitions(type) }
            });
        }

        private string GeneratePropertyDefinitions(Type type)
        {
            return string.Join(Environment.NewLine, GetPropertiesForTranspilation(type)
                .Select(p => UseTemplate("PropertyDefinition", new Dictionary<string, string>
                {
                    { "PropertyName", p.Name },
                    { "PropertyType", p.PropertyType.Name }
                })));
        }

        private IEnumerable<PropertyInfo> GetPropertiesForTranspilation(Type type)
            => type.GetProperties();

        private string UseTemplate(string templateName, IDictionary<string, string> replacements)
        {
            var templateFullName = typeof(TypeScriptTranspiler).Namespace 
                + $".TypeScriptCodeTemplates.{templateName}.txt";

            using (var stream = typeof(TypeScriptTranspiler).Assembly.GetManifestResourceStream(templateFullName))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var templateContent = new StringBuilder(reader.ReadToEnd());

                foreach(var replacement in replacements)
                {
                    templateContent = templateContent.Replace($"[{replacement.Key}]", replacement.Value);
                }

                return templateContent.ToString();
            }
        }
    }
}