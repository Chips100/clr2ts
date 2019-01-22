using Clr2Ts.Transpiler.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Clr2Ts.Transpiler.Transpilation.TypeScript
{
    public class TypeScriptTranspiler : ITranspiler
    {
        private ITemplatingEngine _templatingEngine;

        public TypeScriptTranspiler(ITemplatingEngine templatingEngine)
        {
            _templatingEngine = templatingEngine ?? throw new ArgumentNullException(nameof(templatingEngine));
        }

        public TranspilationResult Transpile(IEnumerable<Type> types)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));

            return new TranspilationResult(types.Select(GenerateClassDefinition));
        }

        private CodeFragment GenerateClassDefinition(Type type)
        {
            var dependencies = type.GetPropertiesForDependencies()
                .Select(p => p.PropertyType).Select(CodeFragmentId.ForClrType);

            var code = _templatingEngine.UseTemplate("ClassDefinition", new Dictionary<string, string>
            {
                { "ClassName", type.Name },
                { "Properties", GeneratePropertyDefinitions(type).AddIndentation() }
            });

            return new CodeFragment(
                CodeFragmentId.ForClrType(type),
                dependencies,
                code);
        }

        private string GeneratePropertyDefinitions(Type type)
        {
            return string.Join(Environment.NewLine, type.GetPropertiesForTranspilation()
                .Select(p => _templatingEngine.UseTemplate("PropertyDefinition", new Dictionary<string, string>
                {
                    { "PropertyName", p.Name },
                    { "PropertyType", p.PropertyType.ToTypeScriptName() }
                })));
        }
    }
}