using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Input;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Clr2Ts.Transpiler.Tests.Input
{
    /// <summary>
    /// Tests for <see cref="InputConfiguration"/>.
    /// </summary>
    public class InputConfigurationTests
    {
        /// <summary>
        /// The constructor should throw a <see cref="ConfigurationException"/>
        /// when no assembly files are provided (sequence is null or empty).
        /// </summary>
        [Fact]
        public void InputConfiguration_ThrowsConfigurationException_WhenAssemblyFilesNullOrEmpty()
        {
            Assert.Throws<ConfigurationException>(() => new InputConfiguration(null));
            Assert.Throws<ConfigurationException>(() => new InputConfiguration(Enumerable.Empty<string>()));
        }

        /// <summary>
        /// The constructor should copy the current content of the provided
        /// sequence for assembly files (for immutability).
        /// </summary>
        [Fact]
        public void InputConfiguration_CopiesAssemblyFilesSequence()
        {
            var original = new List<string> { "1", "2", "3" };
            var inputConfiguration = new InputConfiguration(original);

            original.Add("4");
            Assert.Equal(3, inputConfiguration.AssemblyFiles.Count());
        }

        /// <summary>
        /// If the typefilter section has been omitted in the input configuration,
        /// the filter should return true for any type.
        /// </summary>
        [Fact]
        public void InputConfiguration_EmptyTypeFilter_ReturnsTrue()
        {
            var inputConfiguration = new InputConfiguration(new[] { "Assembly" }, null);

            Assert.True(inputConfiguration.TypeFilter.IsMatch(GetType()));
        }
    }
}