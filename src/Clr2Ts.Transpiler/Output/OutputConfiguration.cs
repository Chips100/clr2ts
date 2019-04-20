using Clr2Ts.Transpiler.Configuration;

namespace Clr2Ts.Transpiler.Output
{
    /// <summary>
    /// Represents the output-specific configuration section for a transpilation process.
    /// </summary>
    [ConfigurationSection("output")]
    public sealed class OutputConfiguration
    {
        /// <summary>
        /// Creates an <see cref="OutputConfiguration"/>.
        /// </summary>
        /// <param name="clipboard">If set to true, the code will be written to the console output.</param>
        /// <param name="bundledFile">If specified, the code will be bundled and written to a single file.</param>
        /// <param name="files">If specified, the code will be written to individual files in a directory.</param>
        public OutputConfiguration(bool clipboard, string bundledFile, OutputFilesConfiguration files)
        {
            Clipboard = clipboard;
            BundledFile = bundledFile;
            Files = files;
        }

        /// <summary>
        /// Gets a value indicating if the code should be written to the console output.
        /// </summary>
        public bool Clipboard { get; }

        /// <summary>
        /// Gets the file to which the code should be bundled and written.
        /// </summary>
        public string BundledFile { get; }

        /// <summary>
        /// Gets the configuration that specifies how the code should be written to individual files.
        /// </summary>
        public OutputFilesConfiguration Files { get; }
    }
}