using Clr2Ts.Transpiler.Configuration.Files;
using Clr2Ts.Transpiler.Input;
using Clr2Ts.Transpiler.Logging;
using Clr2Ts.Transpiler.Output;
using Clr2Ts.Transpiler.Transpilation.Templating;
using Clr2Ts.Transpiler.Transpilation.TypeScript;
using System;
using System.Linq;

namespace Clr2Ts.Cli
{
    /// <summary>
    /// Entry point for the CLI.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Runs the transpiler from the CLI.
        /// </summary>
        /// <param name="args">Arguments provided to the CLI.</param>
        public static void Main(string[] args)
        {
            try
            {
                Execute(args.FirstOrDefault());
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
        /// <param name="configurationFile">
        /// Configuration file for the transpilation. If omitted, will be searched 
        /// by looking in parent directories from the current working directory.
        /// </param>
        private static void Execute(string configurationFile)
        {
            // Configuration.
            var configuration = !string.IsNullOrWhiteSpace(configurationFile)
                ? JsonFileConfigurationSource.FromFile(configurationFile)
                : JsonFileConfigurationSource.FromDirectory(Environment.CurrentDirectory);

            // Change the current working directory based on the configuration file to allow relative paths.
            Environment.CurrentDirectory = configuration.ConfigurationFile.DirectoryName;

            // Logging.
            var logger = LoggerFactory.FromConfiguration(configuration);

            // Input.
            var assemblyScanner = new AssemblyScanner(logger);

            // Transpilation.
            var transpiler = new TypeScriptTranspiler(
                EmbeddedResourceTemplatingEngine.ForTypeScript(),
                new AssemblyXmlDocumentationSource(),
                logger);

            var result = transpiler.Transpile(assemblyScanner.GetTypesByConfiguration(configuration));

            // Output.
            var writer = CodeWriterFactory.FromConfiguration(configuration);
            writer.Write(result.CodeFragments);
        }
    }
}