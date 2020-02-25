using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Configuration.Files;
using Clr2Ts.Transpiler.Input;
using Clr2Ts.Transpiler.Logging;
using Clr2Ts.Transpiler.Output;
using Clr2Ts.Transpiler.Transpilation.Templating;
using Clr2Ts.Transpiler.Transpilation.TypeDefinitionTranslation;
using Clr2Ts.Transpiler.Transpilation.TypeScript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                Execute(args.FirstOrDefault(), ParseReplaceTokens(args.Skip(1)));
            }
            catch (Exception exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
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
        /// <param name="configurationReplaceTokens">Replacements for user-defined tokens in the configuration file.</param>
        private static void Execute(string configurationFile, IDictionary<string, string> configurationReplaceTokens)
        {
            // Configuration.
            var configuration = !string.IsNullOrWhiteSpace(configurationFile)
                ? JsonFileConfigurationSource.FromFile(configurationFile, configurationReplaceTokens)
                : JsonFileConfigurationSource.FromDirectory(Environment.CurrentDirectory, configurationReplaceTokens);

            // Change the current working directory based on the configuration file to allow relative paths.
            Environment.CurrentDirectory = configuration.ConfigurationFile.DirectoryName;

            // Logging.
            var logger = LoggerFactory.FromConfiguration(configuration);
            var stopwatch = Stopwatch.StartNew();

            // Catch exceptions from the actual transpilation as
            // we can write them to the created logger.
            try
            {
                ExecuteTranspilation(configuration, logger);
                logger.WriteInformation($"Executed successfully, took {stopwatch.ElapsedMilliseconds}ms.");
            }
            catch(Exception exception)
            {
                logger.WriteError($"{exception}");
                logger.WriteInformation($"Encountered an error after {stopwatch.ElapsedMilliseconds}ms.");
                throw;
            }
        }

        private static void ExecuteTranspilation(IConfigurationSource configuration, ILogger logger)
        {
            // Input.
            using (var assemblyScanner = new AssemblyScanner(logger))
            {
                // Transpilation.
                var definitionTranslator = new DefaultTypeDefinitionTranslator(
                    configuration,
                    EmbeddedResourceTemplatingEngine.ForTypeScript(),
                    new AssemblyXmlDocumentationSource(),
                    logger);

                var transpiler = new TypeScriptTranspiler(configuration, definitionTranslator);
                var result = transpiler.Transpile(assemblyScanner.GetTypesByConfiguration(configuration));

                // Output.
                var writer = CodeWriterFactory.FromConfiguration(configuration);
                writer.Write(result.CodeFragments);
            }
        }

        private static IDictionary<string, string> ParseReplaceTokens(IEnumerable<string> commandLineArguments) =>
            (from arg in commandLineArguments
             let splitted = arg.Split('=')
             let key = splitted.First()
             let value = string.Join('=', splitted.Skip(1)) // Keep usages of the separator in the value.
             select new KeyValuePair<string, string>(key, value)).ToDictionary(x => x.Key, x => x.Value);

    }
}