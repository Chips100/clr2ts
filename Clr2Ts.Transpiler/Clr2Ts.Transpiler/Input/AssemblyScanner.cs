using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Clr2Ts.Transpiler.TypeSystemModel;
using Newtonsoft.Json;

namespace Clr2Ts.Transpiler.Input
{
    /// <summary>
    /// Scans assemblies for types that should be transpiled.
    /// </summary>
    public sealed class AssemblyScanner
    {
        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        /// <summary>
        /// Scans the specified assembly file for type definitions that should be transpiled.
        /// </summary>
        /// <param name="assemblyFilename">Filename of the assembly that should be scanned.</param>
        /// <returns>A sequence of type definitions that were found in the assembly.</returns>
        public IEnumerable<ClrType> Scan(string assemblyFilename)
        {
            var assemblyDirectory = Path.GetDirectoryName(assemblyFilename);
            if (string.IsNullOrEmpty(assemblyDirectory)) throw new ArgumentException($"Invalid assembly file: {assemblyFilename}", nameof(assemblyFilename));

            using (var appDomainContext = AppDomainContext.Create())
            {
                appDomainContext.AddAssemblyResolveDirectory(new DirectoryInfo(assemblyDirectory));

                var proxy = appDomainContext.CreateProxyInstance<AppDomainProxy>();
                return JsonConvert.DeserializeObject<List<ClrType>>(
                    proxy.ScanAssembly(assemblyFilename), _jsonSerializerSettings);
            }
        }

        private class AppDomainProxy : MarshalByRefObject
        {
            public string ScanAssembly(string assemblyFileName)
            {
                var assembly = Assembly.LoadFile(assemblyFileName);
                var types = ScanAssemblyForTypes(assembly).ToList();

                return JsonConvert.SerializeObject(types, _jsonSerializerSettings);
            }

            private IEnumerable<ClrType> ScanAssemblyForTypes(Assembly assembly)
            {
                return assembly.GetTypes().Select(ClrType.FromType).Where(x => x != null);
            }
        }
    }
}