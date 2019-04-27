using Clr2Ts.Transpiler.Filters.TypeFilters;
using System;
using Xunit;

namespace Clr2Ts.Transpiler.Tests.Filters.TypeFilters
{
    /// <summary>
    /// Tests for <see cref="SubTypeFilter"/>.
    /// </summary>
    public class SubTypeFilterTests
    {
        /// <summary>
        /// IsMatch should throw an <see cref="ArgumentNullException"/> if the specified type is null.
        /// </summary>
        [Fact]
        public void SubTypeFilter_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SubTypeFilter(new[] { "SomeBase" }).IsMatch(null);
            });
        }

        /// <summary>
        /// IsMatch should correctly test for the base class that the type is directly derived from.
        /// </summary>
        [Fact]
        public void SubTypeFilter_DirectBaseClass()
        {
            var sut = new SubTypeFilter(new[] { "BaseB" });
            Assert.True(sut.IsMatch(typeof(SubClass)));
            Assert.False(sut.IsMatch(typeof(NoMatchClass)));
        }

        /// <summary>
        /// IsMatch should correctly test for all base classes that the type is (indirectly) derived from.
        /// </summary>
        [Fact]
        public void SubTypeFilter_IndirectBaseClass()
        {
            var sut = new SubTypeFilter(new[] { "BaseA" });
            Assert.True(sut.IsMatch(typeof(SubClass)));
            Assert.False(sut.IsMatch(typeof(NoMatchClass)));
        }

        /// <summary>
        /// IsMatch should correctly test for interfaces that the type implements directly.
        /// </summary>
        [Fact]
        public void SubTypeFilter_Interface()
        {
            var sut = new SubTypeFilter(new[] { "IDirect" });
            Assert.True(sut.IsMatch(typeof(SubClass)));
            Assert.False(sut.IsMatch(typeof(NoMatchClass)));
        }

        /// <summary>
        /// IsMatch should correctly test for interfaces that a base class of the type
        /// implements (including interface inheritance).
        /// </summary>
        [Fact]
        public void SubTypeFilter_InterfaceFromBaseClass()
        {
            // BaseA implements ISomethingElse which is inherited from ISomething (interface inheritance).
            var sut = new SubTypeFilter(new[] { "ISomething" });
            Assert.True(sut.IsMatch(typeof(SubClass)));
            Assert.False(sut.IsMatch(typeof(NoMatchClass)));
        }

        private class BaseA : ISomethingElse { }

        private class BaseB : BaseA { }

        private class SubClass : BaseB, IDirect { }

        private interface ISomething { }

        private interface ISomethingElse : ISomething { }

        private interface IDirect { }

        private class NoMatchClass { }
    }
}