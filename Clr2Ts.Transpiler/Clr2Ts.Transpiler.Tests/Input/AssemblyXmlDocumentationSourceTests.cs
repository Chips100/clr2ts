using Clr2Ts.Transpiler.Input;
using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace Clr2Ts.Transpiler.Tests.Input
{
    /// <summary>
    /// Tests for <see cref="AssemblyXmlDocumentationSource"/>
    /// </summary>
    public class AssemblyXmlDocumentationSourceTests
    {
        /// <summary>
        /// GetDocumentationText should throw an <see cref="ArgumentNullException"/>
        /// when the provided member is null.
        /// </summary>
        [Fact]
        public void AssemblyXmlDocumentationSource_GetDocumentationText_ThrowsException_WhenMemberNull()
        {
            var sut = new AssemblyXmlDocumentationSource();
            Assert.Throws<ArgumentNullException>(() => sut.GetDocumentationText(null));
        }

        /// <summary>
        /// GetDocumentationText should return a null value
        /// when there is no XML documentation file for the assembly.
        /// </summary>
        [Fact]
        public void AssemblyXmlDocumentationSource_GetDocumentationText_ReturnsNull_WhenXmlFileMissing()
        {
            // Use the test project as a reference without XML documentation file.
            var sut = new AssemblyXmlDocumentationSource();
            Assert.Null(sut.GetDocumentationText(typeof(AssemblyXmlDocumentationSourceTests)));
        }

        /// <summary>
        /// GetDocumentationText should return the documentation text
        /// for a type.
        /// </summary>
        [Fact]
        public void AssemblyXmlDocumentationSource_GetDocumentationText_SupportsTypeDocumentation()
        {
            // Use the sample assembly.
            var assembly = Assembly.LoadFile(SampleAssemblyInfo.Location.FullName);
            var type = assembly.GetType("Clr2Ts.Transpiler.Tests.SampleAssembly.SampleClass");

            var sut = new AssemblyXmlDocumentationSource();
            Assert.Equal("Sample class for unit tests.", sut.GetDocumentationText(type));
        }

        /// <summary>
        /// GetDocumentationText should return the documentation text
        /// for a property.
        /// </summary>
        [Fact]
        public void AssemblyXmlDocumentationSource_GetDocumentationText_SupportsPropertyDocumentation()
        {
            // Use the sample assembly.
            var assembly = Assembly.LoadFile(SampleAssemblyInfo.Location.FullName);
            var type = assembly.GetType("Clr2Ts.Transpiler.Tests.SampleAssembly.SampleClass");
            var property = type.GetProperty("SampleString");

            var sut = new AssemblyXmlDocumentationSource();
            Assert.Equal("Some sample string property.", sut.GetDocumentationText(property));
        }
    }
}