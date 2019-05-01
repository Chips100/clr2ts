using Clr2Ts.Transpiler.Input;
using Clr2Ts.Transpiler.Tests.Mocks;
using System;
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
        public void AssemblyScanner_GetTypesFromAssembly_ReturnsTypes()
        {
            // Just check for presence of this test type when scanning the current assembly.
            TestWithAssemblyScanner(sut =>
            {
                Assert.Contains(sut.GetTypesFromAssembly(GetType().Assembly.Location),
                    t => t.Name == nameof(AssemblyScannerTests));
            });
        }

        /// <summary>
        /// The <see cref="AssemblyScanner"/> should support relative paths for assemblies.
        /// </summary>
        [Fact]
        public void AssemblyScanner_GetTypesFromAssembly_SupportsRelativePaths()
        {
            // Just check for presence of this test type when scanning the current assembly.
            TestWithAssemblyScanner(sut =>
            {
                Assert.Contains(sut.GetTypesFromAssembly("Clr2Ts.Transpiler.Tests.dll"),
                    t => t.Name == nameof(AssemblyScannerTests));
            });
        }

        /// <summary>
        /// The <see cref="AssemblyScanner"/> should support resolving types from other
        /// assemblies referenced by the types in the scanned assembly.
        /// </summary>
        [Fact]
        public void AssemblyScanner_GetTypesFromAssembly_AllowsResolvingDependencyAssemblies()
        {
            TestWithAssemblyScanner(sut =>
            {
                var types = sut.GetTypesFromAssembly(SampleAssemblyInfo.Location.FullName);

                // Search for the type in the sample assembly by its name.
                var typeWithDependency = types.FirstOrDefault(t => t.Name == "DependentOnOtherAssemblyClass");
                Assert.NotNull(typeWithDependency);

                // Retreiving the type of the property would throw an exception
                // if the other assembly is not resolved correctly.
                var referencedType = typeWithDependency.GetProperties().Single().PropertyType;
            });
        }

        private void TestWithAssemblyScanner(Action<AssemblyScanner> testAction)
        {
            using (var x = new AssemblyScanner(new LoggerMock())) testAction(x);
        }
    }
}