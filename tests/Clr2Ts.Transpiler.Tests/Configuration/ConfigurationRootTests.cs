using Clr2Ts.Transpiler.Configuration;
using System.IO;
using System.Linq;
using Xunit;

namespace Clr2Ts.Transpiler.Tests.Configuration
{
    /// <summary>
    /// Tests for <see cref="ConfigurationRoot"/>.
    /// </summary>
    public class ConfigurationRootTests
    {
        /// <summary>
        /// The constructor should throw a <see cref="ConfigurationException"/> 
        /// when no input configuration is provided.
        /// </summary>
        [Fact]
        public void ConfigurationRoot_ThrowsConfigurationException_WhenInputNull()
        {
            Assert.Throws<ConfigurationException>(() => new ConfigurationRoot(null));
        }

        /// <summary>
        /// Exceptions that are thrown when reading a configuration from
        /// a JSON file should be wrapped in a <see cref="ConfigurationException"/>.
        /// </summary>
        [Fact]
        public void ConfigurationRoot_ThrowsConfigurationException_WhenReadingFails()
        {
            var exception = Assert.Throws<ConfigurationException>(
                () => ConfigurationRoot.FromJsonFile("configuration.missing.json"));

            // Underlying exception should indicate the missing file.
            Assert.IsType<FileNotFoundException>(exception.InnerException);
        }

        /// <summary>
        /// Reading a configuration from a JSON file should be supported.
        /// </summary>
        [Fact]
        public void ConfigurationRoot_ReadsConfigurationFromJsonFile()
        {
            var config = ConfigurationRoot.FromJsonFile("configuration.sample.json");

            Assert.Single(config.Input.AssemblyFiles);
        }
    }
}