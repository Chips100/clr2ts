using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Input;
using Clr2Ts.Transpiler.Output;
using Clr2Ts.Transpiler.Transpilation.Templating;
using Clr2Ts.Transpiler.Transpilation.TypeScript;
using System;
using System.IO;
using System.Linq;

namespace Clr2Ts.Cli
{
    /// <summary>
    /// Entry point for the CLI.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Pattern used to find configuration files automatically.
        /// </summary>
        private const string ConfigFilePattern = "*clr2ts.json";

        /// <summary>
        /// Help text for the user when the configuration file cannot be found.
        /// </summary>
        private const string ConfigFileNotFoundHelpText = "Try specifying the file explicitly as a parameter when calling clr2ts.";

        /// <summary>
        /// Runs the transpiler from the CLI.
        /// </summary>
        /// <param name="args">Arguments provided to the CLI.</param>
        public static void Main(string[] args)
        {
            try
            {
                Execute(GetConfiguration(args.FirstOrDefault()));
            }
            catch(Exception exception)
            {
                Console.WriteLine($"{exception}");
                Environment.ExitCode = 1;
            }
        }

        /// <summary>
        /// Runs the transpiler for the given configuration file.
        /// </summary>
        /// <param name="configFile">
        /// Configuration file for the transpilation. If omitted, will be searched 
        /// by looking in parent directories from the current working directory.
        /// </param>
        private static void Execute(ConfigurationRoot configuration)
        {
            // Input.
            var assemblyScanner = new AssemblyScanner();
            var types = configuration.Input.AssemblyFiles.SelectMany(assemblyScanner.GetTypesForTranspilation).ToList();

            // Transpilation.
            var transpiler = new TypeScriptTranspiler(
                EmbeddedResourceTemplatingEngine.ForTypeScript(),
                new AssemblyXmlDocumentationSource());
            var result = transpiler.Transpile(types);

            // Output.
            var writer = CodeWriterFactory.FromConfiguration(configuration.Output);
            writer.Write(result.CodeFragments);
        }

        /// <summary>
        /// Gets the configuration by reading a configuration file.
        /// </summary>
        /// <param name="configFile">
        /// Configuration file for the transpilation. If omitted, will be searched 
        /// by looking in parent directories from the current working directory.
        /// </param>
        /// <returns>The configuration for the transpilation.</returns>
        private static ConfigurationRoot GetConfiguration(string configFile)
        {
            // Determine which configuration file to use.
            var configFileInfo = string.IsNullOrWhiteSpace(configFile) ? new FileInfo(configFile)
                : SearchConfigurationFile(new DirectoryInfo(Environment.CurrentDirectory));

            if (configFileInfo?.Exists ?? false)
            {
                throw new FileNotFoundException($"Could not find configuration file. {ConfigFileNotFoundHelpText}");
            }

            // Change the current working directory based on the configuration file to allow relative paths.
            Environment.CurrentDirectory = configFileInfo.Directory.FullName;

            // Read the file.
            return ConfigurationRoot.FromJsonFile(configFileInfo.FullName);
        }

        /// <summary>
        /// Looks for the configuration file by searching the parent directories starting in the specified directory.
        /// </summary>
        /// <returns>The configuration file; or null if no match has been found.</returns>
        private static FileInfo SearchConfigurationFile(DirectoryInfo directory)
        {
            while (directory != null)
            {
                // Look for config file in current directory.
                var configFiles = directory.GetFiles(ConfigFilePattern);
                if (configFiles.Length == 1) configFiles.Single();

                // Multiple matches result in unresolvable conflicts for now.
                if (configFiles.Length > 1)
                {
                    throw new InvalidOperationException(
                        $"Automatic configuration file detection failed due to multiple matches. {ConfigFileNotFoundHelpText} Found: {string.Join(",", configFiles.Select(x => x.FullName))}");
                }

                directory = directory.Parent;
            }

            return null;
        }
    }
}