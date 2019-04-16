using System.IO;

namespace Clr2Ts.Transpiler.Tests
{
    /// <summary>
    /// Information about the sample assembly that is used in unit tests.
    /// </summary>
    /// <remarks>
    /// No hard reference to that assembly should be introduced
    /// as that is one aspect to test (working with unknown assemblies in different directories).
    /// The sample assembly is configured as a dependency in the solution for correct build order.
    /// </remarks>
    internal static class SampleAssemblyInfo
    {
        /// <summary>
        /// Version of the sample assembly.
        /// </summary>
        public static string Version => "1.2.3.4";

        /// <summary>
        /// Name of the sample assembly.
        /// </summary>
        public static string Name => "Clr2Ts.Transpiler.Tests.SampleAssembly";

        /// <summary>
        /// Directory in which the sample assembly is located.
        /// </summary>
        public static DirectoryInfo LocationDirectory => new DirectoryInfo(
            Path.Combine("..", "..", "..", "..", Name, "bin", "netstandard2.0"));

        /// <summary>
        /// Location of the sample assembly file.
        /// </summary>
        public static FileInfo Location => new FileInfo(
            Path.Combine(LocationDirectory.FullName, $"{Name}.dll"));
    }
}