using Clr2Ts.Transpiler.Configuration;

namespace Clr2Ts.Transpiler.Tests.Configuration.Files
{
    /// <summary>
    /// Dummy configuration section for test purposes.
    /// </summary>
    [ConfigurationSection("test")]
    public sealed class TestConfiguration
    {
        /// <summary>
        /// Creates a <see cref="TestConfiguration"/>.
        /// </summary>
        /// <param name="value">Sample configuration value.</param>
        public TestConfiguration(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the sample configuration value.
        /// </summary>
        public string Value { get; }
    }
}
