using System;
using System.Diagnostics.Tracing;

namespace Clr2Ts.Transpiler.Logging.File
{
    /// <summary>
    /// Logger that writes to a text file.
    /// </summary>
    public sealed class FileLogger : ILogger
    {
        /// <summary>
        /// Creates a <see cref="FileLogger"/>.
        /// </summary>
        /// <param name="logFilePath">Path to the file that should be written to.</param>
        public FileLogger(string logFilePath)
        {
            LogFilePath = logFilePath;
        }

        /// <summary>
        /// Path of the file that this logger writes to.
        /// </summary>
        public string LogFilePath { get; }

        /// <summary>
        /// Writes a message to the log.
        /// </summary>
        /// <param name="eventLevel">Event Level of the message.</param>
        /// <param name="message">Message that should be written to the log.</param>
        public void Write(EventLevel eventLevel, string message)
        {
            System.IO.File.AppendAllText(LogFilePath,
                $"{eventLevel}: {message}" + Environment.NewLine);
        }
    }
}