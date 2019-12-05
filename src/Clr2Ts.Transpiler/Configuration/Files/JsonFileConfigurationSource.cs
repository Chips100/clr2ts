using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Clr2Ts.Transpiler.Configuration.Files
{
    /// <summary>
    /// Configuration source that reads configuration values from a JSON file.
    /// </summary>
    public sealed class JsonFileConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// Pattern used to find configuration files automatically.
        /// </summary>
        private const string ConfigFilePattern = "*clr2ts.json";

        /// <summary>
        /// Help text for the user when the configuration file cannot be found.
        /// </summary>
        private const string ConfigFileNotFoundHelpText = "Try specifying the file explicitly as a parameter when calling clr2ts.";


        // Private constructor to ensure usage of factory methods.
        private readonly IReadOnlyDictionary<string, object> _root;
        private JsonFileConfigurationSource(FileInfo configurationFile)
        {
            ConfigurationFile = configurationFile ?? throw new ArgumentNullException(nameof(configurationFile));
            _root = ReadConfigurationFile(configurationFile);
        }

        /// <summary>
        /// Gets a section from this configuration source.
        /// </summary>
        /// <typeparam name="T">Type of the section that should be looked up.</typeparam>
        /// <returns>The section as it is configured in this source.</returns>
        public T GetSection<T>()
        {
            var sectionName = ConfigurationSectionAttribute.GetSectionName<T>();
            if (!_root.ContainsKey(sectionName)) return default(T);

            try
            {
                var serialized = JsonConvert.SerializeObject(_root[sectionName]);
                return JsonConvert.DeserializeObject<T>(serialized);
            }
            catch (Exception exception)
            {
                throw new ConfigurationException($"Could not read configuration section {sectionName}. ", exception);
            }
        }

        /// <summary>
        /// Gets the file from which this configuration has been retreived.
        /// </summary>
        public FileInfo ConfigurationFile { get; }

        /// <summary>
        /// Creates a <see cref="JsonFileConfigurationSource"/> by searching for a configuration file in the specified directory.
        /// </summary>
        /// <param name="directory">Directory that should be searched for a configuration file.</param>
        /// <returns>A <see cref="JsonFileConfigurationSource"/> that represents the configuration file.</returns>
        public static JsonFileConfigurationSource FromDirectory(string directory)
            => new JsonFileConfigurationSource(SearchConfigurationFile(new DirectoryInfo(directory)));

        /// <summary>
        /// Creates a <see cref="JsonFileConfigurationSource"/> by reading the specified configuration file.
        /// </summary>
        /// <param name="fileName">Name of the file that should be used for the configuration.</param>
        /// <returns>A <see cref="JsonFileConfigurationSource"/> that represents the configuration file.</returns>
        public static JsonFileConfigurationSource FromFile(string fileName)
            => new JsonFileConfigurationSource(new FileInfo(fileName));

        /// <summary>
        /// Reads the sections from the specified configuration file.
        /// </summary>
        /// <param name="configurationFile">File from which the sections should be read.</param>
        /// <returns>A readonly dictionary that contains the sections from the configuration file.</returns>
        /// <exception cref="ConfigurationException">Thrown if the file could not be read or parsed.</exception>
        private IReadOnlyDictionary<string, object> ReadConfigurationFile(FileInfo configurationFile)
        {
            try
            {
                var root = JsonConvert.DeserializeObject<IDictionary<string, object>>(
                    File.ReadAllText(configurationFile.FullName, Encoding.UTF8));

                return new ReadOnlyDictionary<string, object>(root);
            }
            catch (Exception exception)
            {
                throw new ConfigurationException($"Could not read configuration file at {configurationFile.FullName}. "
                    + ConfigFileNotFoundHelpText, exception);
            }
        }

        /// <summary>
        /// Looks for the configuration file by searching the parent directories starting in the specified directory.
        /// </summary>
        /// <returns>The configuration file.</returns>
        /// <exception cref="ConfigurationException">Thrown if no configuration file could be found.</exception>
        private static FileInfo SearchConfigurationFile(DirectoryInfo directory)
        {
            while (directory != null)
            {
                // Look for config file in current directory.
                var configFiles = directory.GetFiles(ConfigFilePattern);
                if (configFiles.Length == 1) return configFiles.Single();

                // Multiple matches result in unresolvable conflicts for now.
                if (configFiles.Length > 1)
                {
                    throw new ConfigurationException(
                        $"Automatic configuration file detection failed due to multiple matches. {ConfigFileNotFoundHelpText} " +
                        $"Found: {string.Join(",", configFiles.Select(x => x.FullName))}");
                }

                directory = directory.Parent;
            }

            throw new ConfigurationException(
                $"Automatic configuration file detection failed because no matches were found. {ConfigFileNotFoundHelpText}");
        }
    }
}