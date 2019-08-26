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
        /// <param name="file">Path to a file that should be written to.</param>
        public LoggingConfiguration(bool console, string file)
        {
            Console = console;
            File = file;
        }

        /// <summary>
        /// Gets a value indicating if log messages should be written to the console.
        /// </summary>
        public bool Console { get; }

        /// <summary>
        /// Gets the path to a file that should be written to; or null if no file logging should be used.
        /// </summary>
        public string File { get; }
    }
}