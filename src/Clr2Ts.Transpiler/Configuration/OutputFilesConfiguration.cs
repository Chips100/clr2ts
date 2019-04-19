namespace Clr2Ts.Transpiler.Configuration
{
    /// <summary>
    /// Represents the configuration that specifies how the code should be written to individual files.
    /// </summary>
    public sealed class OutputFilesConfiguration
    {
        /// <summary>
        /// Creates an <see cref="OutputFilesConfiguration"/>.
        /// </summary>
        /// <param name="directory">Directory, in which the files should be created.</param>
        /// <param name="mimicNamespacesWithSubdirectories">If set to true, files will be placed into subdirectories that correspond to the namespaces.</param>
        public OutputFilesConfiguration(string directory, bool mimicNamespacesWithSubdirectories)
        {
            if (string.IsNullOrWhiteSpace(directory)) throw new ConfigurationException("The directory for the files output must be specified.");

            Directory = directory;
            MimicNamespacesWithSubdirectories = mimicNamespacesWithSubdirectories;
        }

        /// <summary>
        /// Gets the directory, in which the files should be created.
        /// </summary>
        public string Directory { get; }

        /// <summary>
        /// Gets a value indicating if the files should be placed into subdirectories that correspond to the namespaces.
        /// </summary>
        public bool MimicNamespacesWithSubdirectories { get; }
    }
}