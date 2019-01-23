using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace Clr2Ts.Transpiler.Configuration
{
    /// <summary>
    /// Represents the root of the configuration for a transpilation process.
    /// </summary>
    public sealed class ConfigurationRoot
    {
        /// <summary>
        /// Creates a <see cref="ConfigurationRoot"/>.
        /// </summary>
        /// <param name="input">Input-specific configuration for the transpilation.</param>
        /// <exception cref="ConfigurationException">Thrown when <paramref name="input"/> is null.</exception>
        public ConfigurationRoot(InputConfiguration input)
        {
            Input = input ?? throw new ConfigurationException("The input section of the configuration cannot be omitted.");
        }

        /// <summary>
        /// Gets input-specific configuration for the transpilation.
        /// </summary>
        public InputConfiguration Input { get; }

        /// <summary>
        /// Creates a ConfigurationRoot by reading configuration from the specified file.
        /// </summary>
        /// <param name="file">Name of the file with the configuration.</param>
        /// <returns>A ConfigurationRoot as defined in the specified file.</returns>
        /// <exception cref="ConfigurationException">Thrown when an error occurs while reading the file or deserialization.</exception>
        public static ConfigurationRoot FromJsonFile(string file)
        {
            try
            {
                return JsonConvert.DeserializeObject<ConfigurationRoot>(
                    File.ReadAllText(file, Encoding.UTF8));
            }
            catch(Exception exception)
            {
                throw new ConfigurationException("Error while reading configuration from JSON file.", exception);
            }
        }
    }
}