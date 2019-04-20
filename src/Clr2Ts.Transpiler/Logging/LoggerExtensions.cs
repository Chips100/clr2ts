using System;
using System.Diagnostics.Tracing;

namespace Clr2Ts.Transpiler.Logging
{
    /// <summary>
    /// Extension methods for instances of <see cref="ILogger"/>.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Writes an error message to the log.
        /// </summary>
        /// <param name="logger">Logger that should be used to write the message.</param>
        /// <param name="message">Message that should be written.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger"/> is null.</exception>
        public static void WriteError(this ILogger logger, string message)
        {
            if (logger is null) throw new ArgumentNullException(nameof(logger));

            logger.Write(EventLevel.Error, message);
        }

        /// <summary>
        /// Writes a warning message to the log.
        /// </summary>
        /// <param name="logger">Logger that should be used to write the message.</param>
        /// <param name="message">Message that should be written.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger"/> is null.</exception>
        public static void WriteWarning(this ILogger logger, string message)
        {
            if (logger is null) throw new ArgumentNullException(nameof(logger));

            logger.Write(EventLevel.Warning, message);
        }

        /// <summary>
        /// Writes an informational message to the log.
        /// </summary>
        /// <param name="logger">Logger that should be used to write the message.</param>
        /// <param name="message">Message that should be written.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="logger"/> is null.</exception>
        public static void WriteInformation(this ILogger logger, string message)
        {
            if (logger is null) throw new ArgumentNullException(nameof(logger));

            logger.Write(EventLevel.Informational, message);
        }
    }
}