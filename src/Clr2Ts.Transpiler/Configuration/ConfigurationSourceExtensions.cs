using System;

namespace Clr2Ts.Transpiler.Configuration
{
    /// <summary>
    /// Extension methods for instances of <see cref="IConfigurationSource"/>.
    /// </summary>
    public static class ConfigurationSourceExtensions
    {
        /// <summary>
        /// Gets a required configuration section, ensuring it is not missing.
        /// </summary>
        /// <typeparam name="T">Type of the section that should be looked up.</typeparam>
        /// <param name="configurationSource">Configuration source with the section that should be looked up.</param>
        /// <returns>The section as it is configured in this source.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="configurationSource"/> is null.</exception>
        /// <exception cref="ConfigurationException">Thrown if the section is missing from the provided <paramref name="configurationSource"/>.</exception>
        public static T GetRequiredSection<T>(this IConfigurationSource configurationSource)
            where T : class
        {
            if (configurationSource is null) throw new ArgumentNullException(nameof(configurationSource));

            var section = configurationSource.GetSection<T>();
            if (section is null)
            {
                throw new ConfigurationException($"Missing required section in configuration: " +
                    $"{ConfigurationSectionAttribute.GetSectionName<T>()}.");
            }

            return section;
        }
    }
}