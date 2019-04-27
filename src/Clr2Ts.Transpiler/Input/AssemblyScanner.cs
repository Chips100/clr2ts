using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Clr2Ts.Transpiler.Input
{
    /// <summary>
    /// Allows scanning of an assembly file for types that should be transpiled.
    /// </summary>
    public sealed class AssemblyScanner
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Creates an <see cref="AssemblyScanner"/>.
        /// </summary>
        /// <param name="logger">Logger to use for writing log messages.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger"/> is null.</exception>
        public AssemblyScanner(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all types that should be transpiled according to the specified configuration.
        /// </summary>
        /// <param name="configurationSource">Source for the configuration that should be used.</param>
        /// <returns>A sequence with the types that should be transpiled according to the specified configuration.</returns>
        public IEnumerable<Type> GetTypesByConfiguration(IConfigurationSource configurationSource)
        {
            if (configurationSource == null) throw new ArgumentNullException(nameof(configurationSource));

            var configuration = configurationSource.GetRequiredSection<InputConfiguration>();
            var assemblyFiles = configuration.AssemblyFiles
                .SelectMany(x => new DirectoryInfo(Environment.CurrentDirectory)
                    .EnumerateFiles(x, SearchOption.AllDirectories)
                    .Select(f => f.FullName));

            return assemblyFiles.SelectMany(GetTypesFromAssembly)
                .Where(configuration.TypeFilter.IsMatch);
        }

        /// <summary>
        /// Scans the specified assembly file for types that should be transpiled.
        /// </summary>
        /// <param name="assemblyFile">Name of the assembly file.</param>
        /// <returns>A sequence with the types from the assembly that should be transpiled.</returns>
        public IEnumerable<Type> GetTypesFromAssembly(string assemblyFile)
        {
            // Support relative paths by resolving them first.
            var fileInfo = new FileInfo(assemblyFile);
            _logger.WriteInformation($"Loading assembly for transpilation: {fileInfo.FullName}");

            // For now, just return all types of the assembly.
            return Assembly.LoadFile(fileInfo.FullName).GetTypes();
        }
    }
}