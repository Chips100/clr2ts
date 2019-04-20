using Clr2Ts.Transpiler.Configuration;

namespace Clr2Ts.Transpiler.Logging
{
    /// <summary>
    /// Represents the logging-specific configuration section for a transpilation process.
    /// </summary>
    [ConfigurationSection("log")]
    public sealed class LoggingConfiguration
    {
        /// <summary>
        /// Creates a <see cref="LoggingConfiguration"/>.
        /// </summary>
        /// <param name="console">True, if log messages should be written to the console.</param>
        public LoggingConfiguration(bool console)
        {
            Console = console;
        }

        /// <summary>
        /// Gets a value indicating if log messages should be written to the console.
        /// </summary>
        public bool Console { get; }
    }
}