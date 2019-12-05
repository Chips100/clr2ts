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
    public sealed class AssemblyScanner : IDisposable
    {
        private bool _disposed;
        private readonly IList<DirectoryInfo> _probingDirectories = new List<DirectoryInfo>();
        private readonly ILogger _logger;

        /// <summary>
        /// Creates an <see cref="AssemblyScanner"/>.
        /// </summary>
        /// <param name="logger">Logger to use for writing log messages.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger"/> is null.</exception>
        public AssemblyScanner(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            AppDomain.CurrentDomain.AssemblyResolve += ResolveWithProbingDirectories;
        }


        /// <summary>
        /// Gets all types that should be transpiled according to the specified configuration.
        /// </summary>
        /// <param name="configurationSource">Source for the configuration that should be used.</param>
        /// <returns>A sequence with the types that should be transpiled according to the specified configuration.</returns>
        public IEnumerable<Type> GetTypesByConfiguration(IConfigurationSource configurationSource)
        {
            if (configurationSource == null) throw new ArgumentNullException(nameof(configurationSource));
            EnsureNotDisposed();

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
            EnsureNotDisposed();

            // Support relative paths by resolving them first.
            var fileInfo = new FileInfo(assemblyFile);
            _probingDirectories.Add(fileInfo.Directory);

            // For now, just return all types of the assembly.
            _logger.WriteInformation($"Loading assembly for transpilation: {fileInfo.FullName}");
            return Assembly.LoadFile(fileInfo.FullName).GetTypes();
        }

        /// <summary>
        /// Disposes of this AssemblyScanner.
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
            AppDomain.CurrentDomain.AssemblyResolve -= ResolveWithProbingDirectories;
        }

        private void EnsureNotDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(AssemblyScanner));
        }

        private Assembly ResolveWithProbingDirectories(object sender, ResolveEventArgs args)
        {
            var expectedFileName = $"{args.Name.Split(',').First()}.dll";
            var files = _probingDirectories.Select(d => new FileInfo(Path.Combine(d.FullName, expectedFileName)));
            var matchingFile = files.FirstOrDefault(f => f.Exists);

            if (matchingFile != null)
            {
                _logger.WriteInformation($"Resolving dependency {args.Name} by loading assembly from file: {matchingFile.FullName}");
                return Assembly.LoadFile(matchingFile.FullName);
            }

            _logger.WriteWarning($"Could not resolve dependency {args.Name}. " +
                $"Searched for files: {string.Join(", ", files.Select(f => f.FullName))}");
            return null;
        }
    }
}