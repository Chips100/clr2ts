using Clr2Ts.Transpiler.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Clr2Ts.Transpiler.Input
{
    internal static class AssemblyScannerTools
    {
        private static readonly ISet<string> ReferenceAssembliesDirectoryNames = new HashSet<string>(new[]
        {
            "ref"
        }, StringComparer.OrdinalIgnoreCase);

        public static IEnumerable<string> RedirectReferenceAssemblies(this IEnumerable<string> assemblyFiles, ILogger logger)
        {
            var redirected = assemblyFiles.Select(f => new FileInfo(f)).Select(file =>
            {
                var isRefAssembly = ReferenceAssembliesDirectoryNames.Contains(file.Directory.Name);
                if (!isRefAssembly) return file.FullName;

                var nonRefAssembly = Path.Combine(file.Directory.Parent.FullName, file.Name);
                if (!File.Exists(nonRefAssembly))
                {
                    logger.WriteWarning($"{file} seems to be a reference assembly, but could not redirect to { nonRefAssembly }.");
                    return null;
                }

                logger.WriteInformation($"{file} seems to be a reference assembly, redirecting to { nonRefAssembly }.");
                return nonRefAssembly;
            });

            return redirected
                .Where(f => f != null)
                .Distinct();
        }
    }
}
