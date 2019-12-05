using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Configuration.Files;
using Clr2Ts.Transpiler.Input;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Clr2Ts.Transpiler.Tests.Configuration.Files
{
    /// <summary>
    /// Tests for <see cref="JsonFileConfigurationSource"/>.
    /// </summary>
    public class JsonFileConfigurationSourceTests
    {
        /// <summary>
        /// Exceptions that are thrown when reading a configuration from
        /// a JSON file should be wrapped in a <see cref="JsonFileConfigurationSource"/>.
        /// </summary>
        [Fact]
        public void JsonFileConfigurationSource_ThrowsConfigurationException_WhenReadingFails()
        {
            var exception = Assert.Throws<ConfigurationException>(
                () => JsonFileConfigurationSource.FromFile("configuration.missing.json"));

            // Underlying exception should indicate the missing file.
            Assert.IsType<FileNotFoundException>(exception.InnerException);
        }

        /// <summary>
        /// Reading a configuration from a JSON file should be supported.
        /// </summary>
        [Fact]
        public void JsonFileConfigurationSource_ReadsConfigurationFromJsonFile()
        {
            var config = JsonFileConfigurationSource.FromFile("configuration.sample.json");

            Assert.Single(config.GetSection<InputConfiguration>().AssemblyFiles);
        }

        /// <summary>
        /// An <see cref="InvalidOperationException"/> should be thrown when
        /// trying to retreive a config section with an unsupported representation type.
        /// </summary>
        [Fact]
        public void JsonFileConfigurationSource_GetSectionByKeyThrowsInvalidOperationException()
        {
            var config = JsonFileConfigurationSource.FromFile("configuration.sample.json");

            Assert.Throws<InvalidOperationException>(() =>
                config.GetSection<UnconfiguredConfigSection>());
        }

        /// <summary>
        /// Exceptions that occur by deserializing the config section should
        /// be wrapped in a <see cref="ConfigurationException"/>.
        /// </summary>
        [Fact]
        public void JsonFileConfigurationSource_GetSectionByKeyThrowsConfigurationException()
        {
            var config = JsonFileConfigurationSource.FromFile("configuration.sample.json");

            var exception = Assert.Throws<ConfigurationException>(() =>
                config.GetSection<UnreadableConfigSection>());
            Assert.NotNull(exception.InnerException);
        }

        /// <summary>
        /// The ConfigurationSource should return a default value for missing sections (null).
        /// </summary>
        [Fact]
        public void JsonFileConfigurationSource_GetSectionByKeyReturnsDefaultValue()
        {
            var config = JsonFileConfigurationSource.FromFile("configuration.sample.json");

            Assert.Null(config.GetSection<MissingConfigSection>());
        }

        /// <summary>
        /// Configuration files can be found by a naming convention.
        /// </summary>
        [Fact]
        public void JsonFileConfigurationSource_SearchesFileInDirectory()
        {
            var config = JsonFileConfigurationSource.FromDirectory("Configuration/Files/SampleDirectoryWithConfigFile");

            Assert.Equal("testcase-sample-directory-with-configfile",
                config.GetSection<InputConfiguration>().AssemblyFiles.Single());
        }

        /// <summary>
        /// When searching for a configuration file by its name,
        /// the search should look in parent directories of the specified directory.
        /// </summary>
        [Fact]
        public void JsonFileConfigurationSource_SearchesFileInParentDirectories()
        {
            var config = JsonFileConfigurationSource.FromDirectory("Configuration/Files/SampleDirectoryWithSubDirectory/SampleSubDirectoryWithConfigFile");

            Assert.Equal("testcase-sample-subdirectory-with-configfile",
                config.GetSection<InputConfiguration>().AssemblyFiles.Single());
        }


        /// <summary>
        /// A <see cref="ConfigurationException"/> should be thrown when multiple
        /// configuration files match the naming convention in the same directory.
        /// </summary>
        [Fact]
        public void JsonFileConfigurationSource_MultipleSearchMatchThrowsConfigurationException()
        {
            Assert.Throws<ConfigurationException>(
                () => JsonFileConfigurationSource.FromDirectory("Configuration/Files/SampleDirectoryWithMultipleConfigFiles"));
        }

        /// <summary>
        /// A <see cref="ConfigurationException"/> should be thrown when no
        /// configuration files match the naming convention, looking upwards from the specified directory.
        /// </summary>
        [Fact]
        public void JsonFileConfigurationSource_NoSearchMatchThrowsConfigurationException()
        {
            Assert.Throws<ConfigurationException>(
                () => JsonFileConfigurationSource.FromDirectory("Configuration/Files/SampleDirectoryWithoutConfigFile"));
        }

        // Missing attribute.
        private class UnconfiguredConfigSection
        { }

        [ConfigurationSection("input")]
        private class UnreadableConfigSection
        {
            public int AssemblyFiles { get; set; }
        }

        [ConfigurationSection("missing")]
        private class MissingConfigSection
        {
            public int SomeValue { get; set; } = 5;
        }
    }
}