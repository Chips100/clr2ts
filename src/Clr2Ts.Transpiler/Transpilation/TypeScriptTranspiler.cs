using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Transpilation.Configuration;
using Clr2Ts.Transpiler.Transpilation.TypeDefinitionTranslation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clr2Ts.Transpiler.Transpilation.TypeScript
{
    /// <summary>
    /// Allows transpiling a set of interdependent .NET type definitions to TypeScript source code.
    /// </summary>
    public sealed class TypeScriptTranspiler
    {
        private readonly ITypeDefinitionTranslator _typeDefinitionTranslator;
        private readonly TranspilationConfiguration _configuration;

        /// <summary>
        /// Creates a <see cref="TypeScriptTranspiler"/>.
        /// </summary>
        /// <param name="typeDefinitionTranslator">Translator used to translate type definitions.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="typeDefinitionTranslator"/> is null.</exception>
        public TypeScriptTranspiler(IConfigurationSource configurationSource, ITypeDefinitionTranslator typeDefinitionTranslator)
        {
            if (configurationSource == null) throw new ArgumentNullException(nameof(configurationSource));

            _configuration = configurationSource.GetSection<TranspilationConfiguration>() ?? TranspilationConfiguration.Default;
            _typeDefinitionTranslator = typeDefinitionTranslator ?? throw new ArgumentNullException(nameof(typeDefinitionTranslator));
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

            // Only translate type definitions that are
            // not mapped to custom TypeScript types.
            types = types.Where(t => !_configuration.CustomTypeMaps.Any(m => m.MapsType(t)));

            var result = new TranspilationResult(types.Select(_typeDefinitionTranslator.Translate));
            return TranspileDependencies(result);
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

                    codeFragments.Add(_typeDefinitionTranslator.Translate(type));
                }

                currentResult = currentResult.AddCodeFragments(codeFragments);
            }

            return currentResult;
        }
    }
}