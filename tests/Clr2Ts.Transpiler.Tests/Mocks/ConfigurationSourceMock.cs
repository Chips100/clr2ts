using Clr2Ts.Transpiler.Configuration;
using System.Linq;

namespace Clr2Ts.Transpiler.Tests.Mocks
{
    /// <summary>
    /// Mock-implementation of an <see cref="IConfigurationSource"/> that provides predefined values.
    /// </summary>
    public sealed class ConfigurationSourceMock: IConfigurationSource
    {
        private readonly object[] _configurationSections;

        /// <summary>
        /// Creates a <see cref="ConfigurationSourceMock"/>.
        /// </summary>
        /// <param name="configurationSections">Instances of configuration sections that should be used.</param>
        public ConfigurationSourceMock(params object[] configurationSections)
        {
            _configurationSections = configurationSections;
        }

        /// <summary>
        /// Gets a section from this configuration source.
        /// </summary>
        /// <typeparam name="T">Type of the section that should be looked up.</typeparam>
        /// <returns>The section as it is configured in this source.</returns>
        public T GetSection<T>() => _configurationSections.OfType<T>().Single();
    }
}