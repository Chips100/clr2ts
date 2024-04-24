using Clr2Ts.Transpiler.Extensions;
using Clr2Ts.Transpiler.Logging;
using Clr2Ts.Transpiler.Transpilation.Templating;
using System;
using System.Collections.Generic;

namespace Clr2Ts.Transpiler.Transpilation.TypeListTranslation.Strategies
{
    public sealed class ClassListTranslationStrategy: ITypeListTranslationStrategy
    {
        private readonly ITemplatingEngine _templatingEngine;

        /// <summary>
        /// Creates a <see cref="ClassListTranslationStrategy"/>.
        /// </summary>
        /// <param name="templatingEngine">Engine to use for loading templates.</param>
        /// <param name="logger">Logger to use for writing log messages.</param>
        public ClassListTranslationStrategy(
            ITemplatingEngine templatingEngine,
            ILogger logger)
        {
            _templatingEngine = templatingEngine;
        }

        public bool CanTranslateTypeDefinition(Type type)
        {
            return type.IsClass;
        }

        private static readonly Func<Type, string> GetClassNameWithGenerics = (type) => {
            if (type == null || type.IsGenericTypeParameter)
                return "any";

            return type.GetNameWithGenericTypeParameters(null, GetClassNameWithGenerics);
        };

        public (string code, IEnumerable<Import> imports) Translate(Type type)
        {
            var className = type.Name.Split("`")[0];
            var code = _templatingEngine.UseTemplate(
                "ClassListEntry",
                new Dictionary<string, string> {
                    { "AssemblyTypeDefinition", $"{className}, {type.Assembly.GetName().Name}" },
                    { "ClassName", className },
                    { "ClassNameWithGenerics", GetClassNameWithGenerics(type) }
                }
            );
            var imports = new[] {
                new Import(className, $"./{className}")
            };

            return (code, imports);
        }
    }
}