using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Extensions;
using Clr2Ts.Transpiler.Logging;
using Clr2Ts.Transpiler.Transpilation.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clr2Ts.Transpiler.Transpilation.TypeDefinitionTranslation.Strategies
{
    /// <summary>
    /// Strategy for translating definitions of enum types.
    /// </summary>
    public sealed class EnumDefinitionTranslationStrategy : TranslationStrategyBase
    {
        /// <summary>
        /// Creates a <see cref="EnumDefinitionTranslator"/>.
        /// </summary>
        /// <param name="configurationSource">Source for the configuration that should be used.</param>
        /// <param name="templatingEngine">Engine to use for loading templates.</param>
        /// <param name="documentationSource">Source for looking up documentation comments for members.</param>
        /// <param name="logger">Logger to use for writing log messages.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="templatingEngine"/> or <paramref name="documentationSource"/> or <paramref name="logger"/> is null.</exception>
        public EnumDefinitionTranslationStrategy(IConfigurationSource configurationSource, ITemplatingEngine templatingEngine, IDocumentationSource documentationSource, ILogger logger)
            : base(configurationSource, templatingEngine, documentationSource, logger)
        { }

        /// <summary>
        /// Is overridden to define which type definitions can be translated by this strategy.
        /// </summary>
        /// <param name="type">Type definition that should be translated.</param>
        /// <returns>True, if this strategy can be used to translate the specified type definition; otherwise false.</returns>
        protected override bool CanTranslate(Type type)
            => type.IsEnum;

        /// <summary>
        /// Is overridden to define how the type definition is translated by this strategy.
        /// </summary>
        /// <param name="type">Type definition that should be tranlated.</param>
        /// <param name="templatingEngine">Templating engine that can be used to fill predefined code snippets.</param>
        /// <returns>Result of the translation.</returns>
        protected override CodeFragment Translate(Type type, ITemplatingEngine templatingEngine)
        {
            var code = templatingEngine.UseTemplate("EnumDefinition", new Dictionary<string, string>
            {
                { "EnumName", type.Name },
                { "Documentation", GenerateDocumentationComment(type) },
                { "EnumValues", GenerateEnumValues(type, templatingEngine).AddIndentation() },
                { "EnumAttributeMaps", GenerateEnumAttributeMaps(type, templatingEngine) }
            });
            
            return new CodeFragment(
                CodeFragmentId.ForClrType(type),
                code,
                CodeDependencies.Empty);
        }

        private string GenerateEnumValues(Type type, ITemplatingEngine templatingEngine)
        {
            return string.Join("," + Environment.NewLine, Enum.GetNames(type).Select(name =>
                templatingEngine.UseTemplate("EnumValue", new Dictionary<string, string>
                {
                    { "EnumValueName", name },
                    { "Documentation", GenerateDocumentationComment(type.GetMember(name).Single()) },
                    { "EnumValue", Convert.ChangeType(Enum.Parse(type, name), Enum.GetUnderlyingType(type)).ToString() }
                })));
        }

        private string GenerateEnumAttributeMaps(Type type, ITemplatingEngine templatingEngine)
        {
            return string.Join(Environment.NewLine,
                Configuration.EnumAttributeMaps.Select(map => GenerateEnumAttributeMap(type, templatingEngine, map.Key, map.Value)));
        }

        private string GenerateEnumAttributeMap(Type type, ITemplatingEngine templatingEngine, string mapName, string mapValueTemplate)
        {
            return templatingEngine.UseTemplate("EnumAttributeMap", new Dictionary<string, string>
            {
                { "EnumName", type.Name },
                { "MapName", mapName },
                { "MapItems", string.Join($",{Environment.NewLine}",
                    from memberName in Enum.GetNames(type)
                    select templatingEngine.UseTemplate("EnumAttributeMapItem", new Dictionary<string, string>
                    {
                        { "EnumName", type.Name },
                        { "EnumMember", memberName },
                        { "MappedValue", mapValueTemplate.FormatWith(type.GetMember(memberName).SingleOrDefault().CreateFormattingContext()) }
                    })).AddIndentation()
                }
            });
        }

        private IDictionary<string, Attribute> GetEnumMemberAttributes(Type type, string name)
        {
            var member = type.GetMember(name).SingleOrDefault();
            return type.GetMember(name).SingleOrDefault().CustomAttributes
                .Select(x => x.AttributeType)
                .ToDictionary(x => x.Name, member.GetCustomAttribute);
        }
    }
}