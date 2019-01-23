using System;

namespace Clr2Ts.Transpiler.Configuration
{
    /// <summary>
    /// Thrown when an invalid configuration is detected.
    /// </summary>
    public sealed class ConfigurationException: ApplicationException
    {
        /// <summary>
        /// Creates a <see cref="ConfigurationException"/>.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="innerException">Optional inner exception that describes the underlying cause.</param>
        public ConfigurationException(string message, Exception innerException = null)
            : base(message, innerException)
        { }
    }
}