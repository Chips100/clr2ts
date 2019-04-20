﻿using Clr2Ts.Transpiler.Input;
using Clr2Ts.Transpiler.Tests.Mocks;
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
        public void AssemblyScanner_GetTypesFromAssembly_ReturnsTypes()
        {
            // Just check for presence of this test type when scanning the current assembly.
            var sut = new AssemblyScanner(new LoggerMock());
            Assert.Contains(sut.GetTypesFromAssembly(GetType().Assembly.Location), 
                t => t.Name == nameof(AssemblyScannerTests));
        }

        /// <summary>
        /// The <see cref="AssemblyScanner"/> should support relative paths for assemblies.
        /// </summary>
        [Fact]
        public void AssemblyScanner_GetTypesFromAssembly_SupportsRelativePaths()
        {
            // Just check for presence of this test type when scanning the current assembly.
            var sut = new AssemblyScanner(new LoggerMock());
            Assert.Contains(sut.GetTypesFromAssembly("Clr2Ts.Transpiler.Tests.dll"),
                t => t.Name == nameof(AssemblyScannerTests));
        }
    }
}