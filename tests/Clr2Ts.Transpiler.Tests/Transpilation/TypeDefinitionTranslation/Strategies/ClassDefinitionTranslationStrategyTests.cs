using Clr2Ts.Transpiler.Filters.ConfigurationAdapters;
using Clr2Ts.Transpiler.Input;
using Clr2Ts.Transpiler.Tests.Mocks;
using Clr2Ts.Transpiler.Transpilation.Configuration;
using Clr2Ts.Transpiler.Transpilation.Templating;
using Clr2Ts.Transpiler.Transpilation.TypeDefinitionTranslation.Strategies;
using Xunit;

namespace Clr2Ts.Transpiler.Tests.Transpilation.TypeDefinitionTranslation.Strategies
{
    internal interface ITestInterface
    {
    }

    internal class TestClass_ClassDefinitionTranslationStrategy: ITestInterface
    {
        public string Name { get; set; }
    }

    /// <summary>
    /// Tests for the <see cref="ClassDefinitionTranslationStrategyTests"/>.
    /// </summary>
    public class ClassDefinitionTranslationStrategyTests
    {
        /// <summary>
        /// The DictionaryTypeTranslationStrategy should translate references
        /// to dictionaries with simple key/value types.
        /// </summary>
        [Fact]
        public void ClassDefinitionTranslationStrategyTests_TranslatesTypeHintInjection()
        {
            var definition = typeof(TestClass_ClassDefinitionTranslationStrategy);
            var config = new ConfigurationSourceMock(
                new TranspilationConfiguration(
                    true,
                    false,
                    false,
                    null,
                    null,
                    null,
                    null,
                    null,
                    new TypeFilterConfigurationAdapter(
                        null,
                        new[] {
                            "ITestInterface"
                        },
                        null,
                        false
                    )
                )
            );

            var cdts = new ClassDefinitionTranslationStrategy(
                config,
                EmbeddedResourceTemplatingEngine.ForTypeScript(),
                new AssemblyXmlDocumentationSource(),
                new LoggerMock()
            );

            Assert.True(cdts.CanTranslateTypeDefinition(definition));

            var result = cdts.Translate(definition);

            Assert.Contains(
                "public override readonly $type: string = \"Clr2Ts.Transpiler.Tests.Transpilation.TypeDefinitionTranslation.Strategies.TestClass_ClassDefinitionTranslationStrategy, Clr2Ts.Transpiler.Tests\"",
                result.Code
            );
        }
    }
}