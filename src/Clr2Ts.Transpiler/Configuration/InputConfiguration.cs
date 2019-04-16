using System.Collections.Generic;
using System.Linq;

namespace Clr2Ts.Transpiler.Configuration
{
    /// <summary>
    /// Represents the input-specific configuration section for a transpilation process.
    /// </summary>
    public sealed class InputConfiguration
    {
        /// <summary>
        /// Creates an <see cref="InputConfiguration"/>.
        /// </summary>
        /// <param name="assemblyFiles">Names of the assembly files that should be transpiled.</param>
        /// <exception cref="ConfigurationException">Thrown when <paramref name="assemblyFiles"/> is null or empty.</exception>
        public InputConfiguration(IEnumerable<string> assemblyFiles)
        {
            if (assemblyFiles == null || !assemblyFiles.Any())
            {
                throw new ConfigurationException("At least one assembly file must be specified in the input section of the configuration.");
            }

            AssemblyFiles = assemblyFiles.ToList();
        }

        /// <summary>
        /// Gets the names of the assembly files that should be transpiled.
        /// </summary>
        public IEnumerable<string> AssemblyFiles { get; }
    }
}