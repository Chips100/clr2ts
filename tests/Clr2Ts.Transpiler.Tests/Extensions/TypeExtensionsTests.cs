using Clr2Ts.Transpiler.Extensions;
using System;
using System.Linq;
using Xunit;

namespace Clr2Ts.Transpiler.Tests.Extensions
{
    /// <summary>
    /// Tests for extension methods for instances of <see cref="Type"/>.
    /// </summary>
    public class TypeExtensionsTests
    {
        /// <summary>
        /// GetBaseTypes should throw an <see cref="ArgumentNullException"/> eagerly if the type is null.
        /// </summary>
        [Fact]
        public void TypeExtensions_GetBaseTypes_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Do not evaluate the IEnumerable to ensure eager exception throwing. 
                var _ = ((Type)null).GetBaseTypes();
            });
        }

        /// <summary>
        /// GetBaseTypes should only return <see cref="object"/> if the type is not derived from another type.
        /// </summary>
        [Fact]
        public void TypeExtensions_GetBaseTypes_TypeDerivedFromObject()
        {
            Assert.Equal(
                new[] { typeof(object) },
                typeof(BaseA).GetBaseTypes().ToArray());
        }

        /// <summary>
        /// GetBaseTypes should return the type that the current type is directly derived from.
        /// </summary>
        [Fact]
        public void TypeExtensions_GetBaseTypes_TypeDerivedFromBaseClass()
        {
            Assert.Equal(
                new[] { typeof(BaseA), typeof(object) },
                typeof(BaseB).GetBaseTypes().ToArray());
        }

        /// <summary>
        /// GetBaseTypes should return the chain of base classes if multiple levels are involved.
        /// </summary>
        [Fact]
        public void TypeExtensions_GetBaseTypes_ChainOfBaseClasses()
        {
            Assert.Equal(
                new[] { typeof(BaseB), typeof(BaseA), typeof(object) },
                typeof(SubClass).GetBaseTypes().ToArray());
        }

        /// <summary>
        /// GetBaseTypes should return an empty sequence for the ultimate base type <see cref="object"/>.
        /// </summary>
        [Fact]
        public void TypeExtensions_GetBaseTypes_Object()
        {
            Assert.False(typeof(object).GetBaseTypes().Any());
        }

        /// <summary>
        /// GetBaseTypes should return an empty sequence for interfaces.
        /// </summary>
        [Fact]
        public void TypeExtensions_GetBaseTypes_Interface()
        {
            Assert.False(typeof(ISomethingElse).GetBaseTypes().Any());
        }


        private class BaseA { }

        private class BaseB : BaseA { }

        private class SubClass : BaseB { }


        private interface ISomething { }

        private interface ISomethingElse : ISomething { }
    }
}