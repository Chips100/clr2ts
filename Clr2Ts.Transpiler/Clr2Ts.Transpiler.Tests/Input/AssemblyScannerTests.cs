using Clr2Ts.Transpiler.Input;
using System.Linq;
using Xunit;

namespace Clr2Ts.Transpiler.Tests.Input
{
    /// <summary>
    /// Tests for <see cref="AssemblyScanner"/>.
    /// </summary>
    public class AssemblyScannerTests
    {
        /// <summary>
        /// GetTypesForTranspilation should currently just return all
        /// types defined in the specified assembly.
        /// </summary>
        [Fact]
        public void AssemblyScanner_GetTypesForTranspilation_ReturnsTypes()
        {
            // Just check for presence of this test type when scanning the current assembly.
            var sut = new AssemblyScanner();
            Assert.True(sut.GetTypesForTranspilation(GetType().Assembly.Location)
                .Any(t => t.Name == nameof(AssemblyScannerTests)));
        }

        /// <summary>
        /// The <see cref="AssemblyScanner"/> should support relative paths for assemblies.
        /// </summary>
        [Fact]
        public void AssemblyScanner_GetTypesForTranspilation_SupportsRelativePaths()
        {
            // Just check for presence of this test type when scanning the current assembly.
            var sut = new AssemblyScanner();
            Assert.True(sut.GetTypesForTranspilation("Clr2Ts.Transpiler.Tests.dll")
                .Any(t => t.Name == nameof(AssemblyScannerTests)));
        }
    }
}