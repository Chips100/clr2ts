using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Configuration.Files;
using Clr2Ts.Transpiler.Input;
using System.IO;
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
    }
}