using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Extensions;
using Clr2Ts.Transpiler.Logging;
using Clr2Ts.Transpiler.Output;
using Clr2Ts.Transpiler.Transpilation.Templating;
using Clr2Ts.Transpiler.Transpilation.TypeListTranslation.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clr2Ts.Transpiler.Transpilation.TypeListTranslation
{
    public sealed class DefaultTypeListTranslator: ITypeListTranslator
    {
        private readonly ILogger _logger;
        private readonly ITemplatingEngine _templatingEngine;
        private readonly IEnumerable<ITypeListTranslationStrategy> _strategies;
        private readonly bool _dependencies;

        public DefaultTypeListTranslator(IConfigurationSource configurationSource, ITemplatingEngine templatingEngine, ILogger logger)
        {
            if (configurationSource == null)
                throw new ArgumentNullException(nameof(configurationSource));

            if (templatingEngine == null)
                throw new ArgumentNullException(nameof(templatingEngine));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _templatingEngine = templatingEngine;
            _strategies = new ITypeListTranslationStrategy[] {
                new ClassListTranslationStrategy(templatingEngine, logger)
            };
            _dependencies = configurationSource.GetSection<OutputConfiguration>().BundledFile == null;
        }

        public IEnumerable<CodeFragment> Translate(IEnumerable<Type> types)
        {
            var typeList = types.ToList();

            return new[] {
                ForNamespace(GetCommonNamespace(typeList), typeList)
            };
        }

        private CodeFragment ForNamespace(string @namespaceTypes, IEnumerable<Type> types)
        {
            var typeCodeFragments = types
                                    .Select(TranslateType)
                                    .Where(ci => ci.HasValue)
                                    .Select(ci => ci.Value)
                                    .ToList();

            var code = _templatingEngine.UseTemplate(
                "ClassList",
                new Dictionary<string, string> {
                    { "ClassListImport", "" },
                    { "ClassListEntry", string.Join(Environment.NewLine, typeCodeFragments.Select(ci => ci.code)).AddIndentation() }
                }
            );
            var dependencies = _dependencies
                                   ? CodeDependencies.FromImports(typeCodeFragments.SelectMany(ci => ci.imports))
                                   : CodeDependencies.Empty;

            return new CodeFragment(
                CodeFragmentId.ForClassList(@namespaceTypes),
                code,
                dependencies
            );
        }

        private (string code, IEnumerable<Import> imports)? TranslateType(Type type)
        {
            var strategy = _strategies.FirstOrDefault(s => s.CanTranslateTypeDefinition(type));
            if (strategy == null) {
                return null;
            }

            _logger.WriteInformation($"Using {strategy.GetType()} to translate definition of type {type}.");
            return strategy.Translate(type);
        }

        private string GetCommonNamespace(IList<Type> types)
        {
            var namespaces = types
                             .GroupBy(t => t.Namespace)
                             .Select(g => g.Key)
                             .ToList();

            var filteredNamespace = namespaces
                                    .SelectMany(
                                        // Creates all Sub-Namespaces for a given Namespace
                                        g => {
                                            var nSplit = g.Split(".");
                                            var namespaces = new string[nSplit.Length];
                                            for (var i = 0; i < nSplit.Length; i++) {
                                                namespaces[i] = string.Join(".", nSplit.Take(i + 1));
                                            }

                                            return namespaces;
                                        }
                                    )
                                    .GroupBy(g => g)
                                    .Where(g => g.Count() == namespaces.Count)
                                    .Select(g => g.Key)
                                    .Order()
                                    .ToList();

            return filteredNamespace.Count == 0
                       ? ""
                       : filteredNamespace.Last();
        }
    }
}