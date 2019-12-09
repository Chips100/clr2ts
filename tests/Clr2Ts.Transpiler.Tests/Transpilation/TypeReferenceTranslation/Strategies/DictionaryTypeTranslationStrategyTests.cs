using Clr2Ts.Transpiler.Tests.Mocks;
using Clr2Ts.Transpiler.Transpilation;
using Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation.Strategies;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Clr2Ts.Transpiler.Tests.Transpilation.TypeReferenceTranslation.Strategies
{
    /// <summary>
    /// Tests for the <see cref="DictionaryTypeTranslationStrategy"/>.
    /// </summary>
    public class DictionaryTypeTranslationStrategyTests
    {
        /// <summary>
        /// The DictionaryTypeTranslationStrategy should translate references
        /// to dictionaries with simple key/value types.
        /// </summary>
        [Fact]
        public void DictionaryTypeTranslationStrategy_TranslatesDictionary()
        {
            var translatorMock = new TypeReferenceTranslatorMock();
            var sut = new DictionaryTypeTranslationStrategy(translatorMock);

            var result = sut.Translate(typeof(IDictionary<int, bool>));

            Assert.Equal("{[key: number]: bool }", result.ReferencedTypeName);
            Assert.Equal(CodeDependencies.Empty, result.Dependencies);
        }
    }
}
