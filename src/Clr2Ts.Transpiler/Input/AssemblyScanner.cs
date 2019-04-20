using Clr2Ts.Transpiler.Configuration;
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

            return assemblyFiles.SelectMany(GetTypesFromAssembly);
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

            // For now, just return all types of the assembly.
            return Assembly.LoadFile(fileInfo.FullName).GetTypes();
        }
    }
}