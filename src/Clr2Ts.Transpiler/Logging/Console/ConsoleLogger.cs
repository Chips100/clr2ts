using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using OutConsole = System.Console;

namespace Clr2Ts.Transpiler.Logging.Console
{
    /// <summary>
    /// Logger that writes to the console output.
    /// </summary>
    public sealed class ConsoleLogger : ILogger
    {
        private readonly IDictionary<EventLevel, ConsoleColor> _eventLevelColors = new Dictionary<EventLevel, ConsoleColor>
        {
            { EventLevel.Warning, ConsoleColor.Yellow },
            { EventLevel.Error, ConsoleColor.Red },
        };

        /// <summary>
        /// Writes a message to the log.
        /// </summary>
        /// <param name="eventLevel">Event Level of the message.</param>
        /// <param name="message">Message that should be written to the log.</param>
        public void Write(EventLevel eventLevel, string message)
        {
            SetConsoleColorForEventLevel(eventLevel);
            OutConsole.WriteLine($"{message}");
            OutConsole.ResetColor();
        }

        private void SetConsoleColorForEventLevel(EventLevel eventLevel)
        {
            if (_eventLevelColors.TryGetValue(eventLevel, out var color))
            {
                OutConsole.ForegroundColor = color;
            }
            else
            {
                OutConsole.ResetColor();
            }
        }
    }
}