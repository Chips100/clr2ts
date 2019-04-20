using System.Diagnostics.Tracing;

namespace Clr2Ts.Transpiler.Logging
{
    /// <summary>
    /// Log that can be used for writing messages.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Writes a message to the log.
        /// </summary>
        /// <param name="eventLevel">Event Level of the message.</param>
        /// <param name="message">Message that should be written to the log.</param>
        void Write(EventLevel eventLevel, string message);
    }
}