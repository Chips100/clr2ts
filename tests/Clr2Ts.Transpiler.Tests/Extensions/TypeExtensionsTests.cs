using Clr2Ts.Transpiler.Extensions;
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
            Assert.Throws<ArgumentNullException>(
                () => {
                    // Do not evaluate the IEnumerable to ensure eager exception throwing. 
                    var _ = ((Type)null!).GetBaseTypes();
                }
            );
        }

        /// <summary>
        /// GetBaseTypes should only return <see cref="object"/> if the type is not derived from another type.
        /// </summary>
        [Fact]
        public void TypeExtensions_GetBaseTypes_TypeDerivedFromObject()
        {
            Assert.Equal(
                new[] {
                    typeof(object)
                },
                typeof(BaseA).GetBaseTypes().ToArray()
            );
        }

        /// <summary>
        /// GetBaseTypes should return the type that the current type is directly derived from.
        /// </summary>
        [Fact]
        public void TypeExtensions_GetBaseTypes_TypeDerivedFromBaseClass()
        {
            Assert.Equal(
                new[] {
                    typeof(BaseA),
                    typeof(object)
                },
                typeof(BaseB).GetBaseTypes().ToArray()
            );
        }

        /// <summary>
        /// GetBaseTypes should return the chain of base classes if multiple levels are involved.
        /// </summary>
        [Fact]
        public void TypeExtensions_GetBaseTypes_ChainOfBaseClasses()
        {
            Assert.Equal(
                new[] {
                    typeof(BaseB),
                    typeof(BaseA),
                    typeof(object)
                },
                typeof(SubClass).GetBaseTypes().ToArray()
            );
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

        /// <summary>
        /// GetSelfImplementedInterfaces should throw an <see cref="ArgumentNullException"/> eagerly if the type is null.
        /// </summary>
        [Fact]
        public void TypeExtensions_GetSelfImplementedInterfaces_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => {
                    // Do not evaluate the IEnumerable to ensure eager exception throwing. 
                    var _ = ((Type)null!).GetSelfImplementedInterfaces();
                }
            );
        }

        /// <summary>
        /// GetSelfImplementedInterfaces should only return the interfaces
        /// implemented by the class itself, not its base classes.
        /// </summary>
        [Fact]
        public void TypeExtensions_GetSelfImplementedInterfaces_ReturnsOnlyInterfaceFromClass()
        {
            Assert.Equal(
                new[] {
                    typeof(ISomethingElse)
                },
                typeof(SubClass).GetSelfImplementedInterfaces()
            );
        }

        /// <summary>
        /// GetSelfImplementedInterfaces should not return an interface implemented by the base types,
        /// even if it contains a generic parameter that is determined by the class itself.
        /// </summary>
        [Fact]
        public void TypeExtensions_GetSelfImplementedInterfaces_SupportsGenericsDeterminedBySubClass()
        {
            Assert.Equal(
                new[] {
                    typeof(IGenericInterface<string>)
                },
                typeof(GenericSubClass<int>).GetSelfImplementedInterfaces()
            );
        }

        /// <summary>
        /// GetNameWithGenericTypeParameters should throw an <see cref="ArgumentNullException"/> if the type is null.
        /// </summary>
        [Fact]
        public void TypeExtensions_GetNameWithGenericTypeParameters_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ((Type)null!).GetNameWithGenericTypeParameters());
        }

        /// <summary>
        /// GetNameWithGenericTypeParameters should just return the simple type name for non-generic types.
        /// </summary>
        [Fact]
        public void TypeExtensions_GetNameWithGenericTypeParameters_SupportsNonGenericType()
        {
            Assert.Equal(nameof(SubClass), typeof(SubClass).GetNameWithGenericTypeParameters());
        }

        /// <summary>
        /// GetNameWithGenericTypeParameters should add the generic type parameters
        /// to the simple type name listed in angle brackets.
        /// </summary>
        [Fact]
        public void TypeExtensions_GetNameWithGenericTypeParameters_SupportsGenericType()
        {
            Assert.Equal(
                $"{nameof(GenericSubClass<object>)}<T>",
                typeof(GenericSubClass<>).GetNameWithGenericTypeParameters()
            );
        }

        /// <summary>
        /// GetNameWithGenericTypeParameters should work with types that have more than one type parameter.
        /// </summary>
        [Fact]
        public void TypeExtensions_GetNameWithGenericTypeParameters_SupportsGenericTypeOfHigherArity()
        {
            Assert.Equal(
                $"{nameof(GenericWithArityTwo<object, object>)}<T1, T2>",
                typeof(GenericWithArityTwo<,>).GetNameWithGenericTypeParameters()
            );
        }

        /// <summary>
        /// GetNameWithoutGenericTypeParameters should throw an <see cref="ArgumentNullException"/> if the type is null.
        /// </summary>
        [Fact]
        public void TypeExtensions_GetNameWithoutGenericTypeParameters_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ((Type)null!).GetNameWithoutGenericTypeParameters());
        }

        /// <summary>
        /// GetNameWithoutGenericTypeParameters should just return the simple type name for non-generic types.
        /// </summary>
        [Fact]
        public void TypeExtensions_GetNameWithoutGenericTypeParameters_SupportsNonGenericType()
        {
            Assert.Equal(nameof(SubClass), typeof(SubClass).GetNameWithoutGenericTypeParameters());
        }

        /// <summary>
        /// GetNameWithoutGenericTypeParameters should just return the simple name
        /// of generic types, stripping the arity from the full type name.
        /// </summary>
        [Fact]
        public void TypeExtensions_GetNameWithoutGenericTypeParameters_SupportsGenericType()
        {
            Assert.Equal(
                $"{nameof(GenericSubClass<object>)}",
                typeof(GenericSubClass<>).GetNameWithoutGenericTypeParameters()
            );
        }

        /// <summary>
        /// GetNameWithoutGenericTypeParameters should work with types that have more than one type parameter.
        /// </summary>
        [Fact]
        public void TypeExtensions_GetNameWithoutGenericTypeParameters_SupportsGenericTypeOfHigherArity()
        {
            Assert.Equal(
                $"{nameof(GenericWithArityTwo<object, object>)}",
                typeof(GenericWithArityTwo<,>).GetNameWithoutGenericTypeParameters()
            );
        }


        private class BaseA
        {
        }

        private class BaseB: BaseA, ISomething
        {
        }

        private class SubClass: BaseB, ISomethingElse
        {
        }

        private class GenericBase<T>: IGenericInterface<T>
        {
        }

        private class GenericSubClass<T>: GenericBase<T>, IGenericInterface<string>
        {
        }

        private class GenericWithArityTwo<T1, T2>
        {
        }

        private interface ISomething
        {
        }

        private interface ISomethingElse: ISomething
        {
        }

        private interface IGenericInterface<T>
        {
        }
    }
}