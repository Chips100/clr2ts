using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Clr2Ts.EndToEnd.Tests
{
    /// <summary>
    /// E2E:
    ///     - Targets-Project gets transpiled by the clr2ts CLI (via postbuild step).
    ///     - Resulting TypeScript-Files are put into this project.
    ///     - The TypeScript Compiler runs as a build step of this project (MSBuild).
    /// </summary>
    public class EndToEndTests
    {
        /// <summary>
        /// Minimal check: Has anything been compiled?
        /// The e2e test will fail if the TypeScript compiler fails.
        /// </summary>
        [Fact]
        public void EndToEnd_HasExecuted()
        {
            // Assume the project directory has the same name as the project (=namespace).
            var currentDirectory = Environment.CurrentDirectory;
            var projectDirectoryName = GetType().Namespace;
            var projectDirectory = new DirectoryInfo(currentDirectory.Substring(
                0, currentDirectory.IndexOf(projectDirectoryName) + projectDirectoryName.Length));

            // Look for Javascript Files (output of the TypeScript compiler).
            var jsFiles = projectDirectory.EnumerateFiles("*.js", SearchOption.AllDirectories);
            Assert.True(jsFiles.Any());
        }
    }
}
