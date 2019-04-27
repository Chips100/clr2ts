using Clr2Ts.Transpiler.Extensions;
using Clr2Ts.Transpiler.Logging;
using Clr2Ts.Transpiler.Transpilation.Templating;
using Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clr2Ts.Transpiler.Transpilation.TypeScript
{
    /// <summary>
    /// Allows transpiling .NET type definitions to TypeScript source code.
    /// </summary>
    public sealed class TypeScriptTranspiler
    {
        private readonly ITypeReferenceTranslator _typeReferenceTranslator;
        private readonly ITemplatingEngine _templatingEngine;
        private readonly IDocumentationSource _documentationSource;
        private readonly ILogger _logger;

        /// <summary>
        /// Creates a <see cref="TypeScriptTranspiler"/>.
        /// </summary>
        /// <param name="templatingEngine">Engine to use for loading templates.</param>
        /// <param name="documentationSource">Source for looking up documentation comments for members.</param>
        /// <param name="logger">Logger to use for writing log messages.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="templatingEngine"/> or <paramref name="documentationSource"/> or <paramref name="logger"/> is null.</exception>
        public TypeScriptTranspiler(ITemplatingEngine templatingEngine, IDocumentationSource documentationSource, ILogger logger)
        {
            _templatingEngine = templatingEngine ?? throw new ArgumentNullException(nameof(templatingEngine));
            _documentationSource = documentationSource ?? throw new ArgumentNullException(nameof(documentationSource));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _typeReferenceTranslator = new DefaultTypeReferenceTranslator(logger);
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

            var result = new TranspilationResult(types.Select(GenerateClassDefinition));
            return TranspileDependencies(result);
        }

        private CodeFragment GenerateClassDefinition(Type type)
        {
            _logger.WriteInformation($"Translating type {type}.");
            var code = _templatingEngine.UseTemplate("ClassDefinition", new Dictionary<string, string>
            {
                { "ClassDeclaration", type.GetNameWithGenericTypeParameters() },
                { "Documentation", GenerateDocumentationComment(type) },
                { "Properties", GeneratePropertyDefinitions(type, out var dependencies).AddIndentation() }
            });

            return new CodeFragment(
                CodeFragmentId.ForClrType(type),
                dependencies,
                code);
        }

        private TranspilationResult TranspileDependencies(TranspilationResult currentResult)
        {
            while (currentResult.GetUnresolvedDependencies().Any())
            {
                var codeFragments = new List<CodeFragment>();
                foreach (var dependency in currentResult.GetUnresolvedDependencies())
                {
                    if (!dependency.TryRecreateClrType(out var type))
                    {
                        throw new InvalidOperationException($"Detected unresolvable dependency that could not be transpiled: {dependency}");
                    }

                    codeFragments.Add(GenerateClassDefinition(type));
                }

                currentResult = currentResult.AddCodeFragments(codeFragments);
            }

            return currentResult;
        }

        private string GeneratePropertyDefinitions(Type type, out IEnumerable<CodeFragmentId> dependencies)
        {
            var propertyCodeSnippets = new List<string>();
            var deps = new List<CodeFragmentId>();

            foreach(var property in type.GetProperties())
            {
                _logger.WriteInformation($"Translating property {property.Name} on type {type}.");

                var typeReferenceTranslation = _typeReferenceTranslator.Translate(property.PropertyType);
                deps.AddRange(typeReferenceTranslation.Dependencies);
                propertyCodeSnippets.Add(_templatingEngine.UseTemplate("PropertyDefinition", new Dictionary<string, string>
                {
                    { "PropertyName", property.Name },
                    { "Documentation", GenerateDocumentationComment(property) },
                    { "PropertyType", typeReferenceTranslation.ReferencedTypeName }
                }));
            }

            dependencies = deps.Distinct().ToList();
            return string.Join(Environment.NewLine, propertyCodeSnippets);
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