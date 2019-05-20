using Clr2Ts.Transpiler.Configuration;
using System;
using Xunit;

namespace Clr2Ts.Transpiler.Tests.Configuration
{
    /// <summary>
    /// Tests for the extension methods on instances of <see cref="IConfigurationSource"/>.
    /// </summary>
    public class ConfigurationSourceExtensionsTests
    {
        /// <summary>
        /// GetRequiredSection should throw an <see cref="ArgumentNullException"/>
        /// when the provided configuration source is null.
        /// </summary>
        [Fact]
        public void ConfigurationSourceExtensions_GetRequiredSectionThrowsArgumentNullException()
        {
            IConfigurationSource source = null;
            Assert.Throws<ArgumentNullException>(() => source.GetRequiredSection<ConfigurationSection>());
        }

        /// <summary>
        /// GetRequiredSection should throw an <see cref="ConfigurationException"/>
        /// when the source returns null for the section.
        /// </summary>
        [Fact]
        public void ConfigurationSourceExtensions_GetRequiredSectionThrowsConfigurationException()
        {
            var source = new ConfigurationSourceWithoutSection();
            Assert.Throws<ConfigurationException>(() => source.GetRequiredSection<ConfigurationSection>());
        }

        /// <summary>
        /// GetRequiredSection should return the section from the source, if present.
        /// </summary>
        [Fact]
        public void ConfigurationSourceExtensions_GetRequiredSectionReturnsSection()
        {
            var source = new ConfigurationSourceWithSection();
            var section = source.GetRequiredSection<ConfigurationSection>();

            Assert.Equal("SomeValue", section.SomeValue);
        }

        private class ConfigurationSourceWithSection : IConfigurationSource
        {
            public T GetSection<T>() => Activator.CreateInstance<T>();
        }

        private class ConfigurationSourceWithoutSection: IConfigurationSource
        {
            public T GetSection<T>() => default(T);
        }

        [ConfigurationSection("section")]
        private class ConfigurationSection
        {
            public string SomeValue { get; set; } = "SomeValue";
        }
    }
}