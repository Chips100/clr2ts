using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;

namespace Clr2Ts.Transpiler.Logging
{
    /// <summary>
    /// Creates a composite logger that delegates to multiple other loggers.
    /// </summary>
    public sealed class CompositeLogger : ILogger
    {
        private readonly IEnumerable<ILogger> _loggers;

        /// <summary>
        /// Creates a <see cref="CompositeLogger"/>.
        /// </summary>
        /// <param name="loggers">Underlying loggers that should be used.</param>
        public CompositeLogger(IEnumerable<ILogger> loggers)
        {
            if (loggers is null) throw new ArgumentNullException(nameof(loggers));

            // Create own private copy of the sequence.
            _loggers = loggers.ToList();
        }

        /// <summary>
        /// Writes a message to the log.
        /// </summary>
        /// <param name="eventLevel">Event Level of the message.</param>
        /// <param name="message">Message that should be written to the log.</param>
        public void Write(EventLevel eventLevel, string message)
        {
            // Let each writer write the message.
            foreach (var logger in _loggers) logger.Write(eventLevel, message);
        }
    }
}