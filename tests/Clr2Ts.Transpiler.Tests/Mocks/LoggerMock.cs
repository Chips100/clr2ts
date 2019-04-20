using Clr2Ts.Transpiler.Logging;
using System.Diagnostics.Tracing;

namespace Clr2Ts.Transpiler.Tests.Mocks
{
    /// <summary>
    /// Mock-implementation of an <see cref="ILogger"/> that does nothing.
    /// </summary>
    public sealed class LoggerMock : ILogger
    {
        /// <summary>
        /// Writes a message to the log.
        /// </summary>
        /// <param name="eventLevel">Event Level of the message.</param>
        /// <param name="message">Message that should be written to the log.</param>
        public void Write(EventLevel eventLevel, string message) { }
    }
}