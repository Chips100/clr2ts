using Clr2Ts.Transpiler.Configuration;
using System;
using Xunit;

namespace Clr2Ts.Transpiler.Tests.Configuration
{
    /// <summary>
    /// Tests for the <see cref="ConfigurationSectionAttribute"/>.
    /// </summary>
    public class ConfigurationSectionAttributeTests
    {
        /// <summary>
        /// The constructor of  <see cref="ConfigurationSectionAttribute"/>
        /// should throw an <see cref="ArgumentException"/> when the provided name is empty.
        /// </summary>
        [Fact]
        public void ConfigurationSectionAttribute_ThrowsArgumentExceptionWhenSectionNameIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => new ConfigurationSectionAttribute("   "));
        }

        /// <summary>
        /// A <see cref="InvalidOperationException"/> should be thrown when
        /// trying to read the section name of a type without the <see cref="ConfigurationSectionAttribute"/>.
        /// </summary>
        [Fact]
        public void ConfigurationSectionAttribute_GetSectionNameThrowsInvalidOperationExceptionForTypeWithoutAttribute()
        {
            Assert.Throws<InvalidOperationException>(() =>
                ConfigurationSectionAttribute.GetSectionName<TypeWithoutConfigurationSectionAttribute>());
        }

        /// <summary>
        /// GetSectionName should return the name specified in the attribute of the type.
        /// </summary>
        [Fact]
        public void ConfigurationSectionAttribute_GetSectionNameReturnsSectionNameOfType()
        {
            Assert.Equal("section", ConfigurationSectionAttribute
                .GetSectionName<TypeWithConfigurationSectionAttribute>());
        }

        private class TypeWithoutConfigurationSectionAttribute
        { }

        [ConfigurationSection("section")]
        private class TypeWithConfigurationSectionAttribute
        { }
    }
}