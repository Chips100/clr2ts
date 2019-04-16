using System;
using Clr2Ts.Transpiler.Configuration;
using Xunit;

namespace Clr2Ts.Transpiler.Tests.Configuration
{
    /// <summary>
    /// Tests for TypeFilters.
    /// </summary>
    public class TypeFilterTests
    {
        /// <summary>
        /// IsMatch should throw an <see cref="ArgumentNullException"/> if the specified type is null.
        /// </summary>
        [Fact]
        public void TypeFilter_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                TypeFilter.SubTypeOf("SomeBase").IsMatch(null);
            });
        }

        /// <summary>
        /// SubTypeOf should throw an <see cref="ArgumentNullException"/> if no name for the base type is given.
        /// </summary>
        [Fact]
        public void TypeFilter_SubTypeOf_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                TypeFilter.SubTypeOf("    ");
            });
        }

        /// <summary>
        /// SubTypeOf should correctly test for the base class that the type is directly derived from.
        /// </summary>
        [Fact]
        public void TypeFilter_SubTypeOf_DirectBaseClass()
        {
            var sut = TypeFilter.SubTypeOf("BaseB");
            Assert.True(sut.IsMatch(typeof(SubClass)));
            Assert.False(sut.IsMatch(typeof(NoMatchClass)));
        }

        /// <summary>
        /// SubTypeOf should correctly test for all base classes that the type is (indirectly) derived from.
        /// </summary>
        [Fact]
        public void TypeFilter_SubTypeOf_IndirectBaseClass()
        {
            var sut = TypeFilter.SubTypeOf("BaseA");
            Assert.True(sut.IsMatch(typeof(SubClass)));
            Assert.False(sut.IsMatch(typeof(NoMatchClass)));
        }

        /// <summary>
        /// SubTypeOf should correctly test for interfaces that the type implements directly.
        /// </summary>
        [Fact]
        public void TypeFilter_SubTypeOf_Interface()
        {
            var sut = TypeFilter.SubTypeOf("IDirect");
            Assert.True(sut.IsMatch(typeof(SubClass)));
            Assert.False(sut.IsMatch(typeof(NoMatchClass)));
        }

        /// <summary>
        /// SubTypeOf should correctly test for interfaces that a base class of the type
        /// implements (including interface inheritance).
        /// </summary>
        [Fact]
        public void TypeFilter_SubTypeOf_InterfaceFromBaseClass()
        {
            // BaseA implements ISomethingElse which is inherited from ISomething (interface inheritance).
            var sut = TypeFilter.SubTypeOf("ISomething");
            Assert.True(sut.IsMatch(typeof(SubClass)));
            Assert.False(sut.IsMatch(typeof(NoMatchClass)));
        }

        private class BaseA : ISomethingElse { }

        private class BaseB: BaseA { }

        private class SubClass: BaseB, IDirect { }

        private interface ISomething { }

        private interface ISomethingElse: ISomething { }

        private interface IDirect { }

        private class NoMatchClass { }
    }
}