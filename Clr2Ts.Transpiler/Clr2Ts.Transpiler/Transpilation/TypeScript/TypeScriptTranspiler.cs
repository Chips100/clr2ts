using Clr2Ts.Transpiler.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clr2Ts.Transpiler.Transpilation.TypeScript
{
    /// <summary>
    /// Allows transpiling .NET type definitions to TypeScript source code.
    /// </summary>
    public sealed class TypeScriptTranspiler : ITranspiler
    {
        private ITemplatingEngine _templatingEngine;
        private IDocumentationSource _documentationSource;

        /// <summary>
        /// Creates a <see cref="TypeScriptTranspiler"/>.
        /// </summary>
        /// <param name="templatingEngine">Engine to use for loading templates.</param>
        /// <param name="documentationSource">Source for looking up documentation comments for members.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="templatingEngine"/> or <paramref name="documentationSource"/> is null.</exception>
        public TypeScriptTranspiler(ITemplatingEngine templatingEngine, IDocumentationSource documentationSource)
        {
            _templatingEngine = templatingEngine ?? throw new ArgumentNullException(nameof(templatingEngine));
            _documentationSource = documentationSource ?? throw new ArgumentNullException(nameof(documentationSource));
        }

        /// <summary>
        /// Transpiles the specified .NET type definitions to new source code.
        /// </summary>
        /// <param name="types">Types that should be transpiled.</param>
        /// <returns>The result of the transpilation process.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="types"/> is null.</exception>
        public TranspilationResult Transpile(IEnumerable<Type> types)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));

            return new TranspilationResult(types.Select(GenerateClassDefinition));
        }

        private CodeFragment GenerateClassDefinition(Type type)
        {
            var dependencies = type.GetDependencies().Select(CodeFragmentId.ForClrType);

            var code = _templatingEngine.UseTemplate("ClassDefinition", new Dictionary<string, string>
            {
                { "ClassName", type.Name },
                { "Documentation", GenerateDocumentationComment(type) },
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
                    { "Documentation", GenerateDocumentationComment(p) },
                    { "PropertyType", p.PropertyType.ToTypeScriptName() }
                })));
        }
        
        private string GenerateDocumentationComment(MemberInfo member)
        {
            var documentation = _documentationSource.GetDocumentationText(member);
            if (string.IsNullOrWhiteSpace(documentation)) return null;

            return $@"/**
 * {documentation}
 */
";
        }
    }
}