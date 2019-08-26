using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Logging.Console;
using Clr2Ts.Transpiler.Logging.File;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Clr2Ts.Transpiler.Logging
{
    /// <summary>
    /// Factory that creates instances of <see cref="ILogger"/>.
    /// </summary>
    public static class LoggerFactory
    {
        /// <summary>
        /// Creates an <see cref="ILogger"/> for the specified configuration.
        /// </summary>
        /// <param name="configurationSource">Source for the configuration that should be used.</param>
        /// <returns>An instance of <see cref="ILogger"/> for the specified configuration.</returns>
        public static ILogger FromConfiguration(IConfigurationSource configurationSource)
        {
            if (configurationSource == null) throw new ArgumentNullException(nameof(configurationSource));

            // Allow omitting the log configuration section.
            var configuration = configurationSource.GetSection<LoggingConfiguration>();
            if (configuration == null) return new CompositeLogger(Enumerable.Empty<ILogger>());

            var loggers = new List<ILogger>();

            if (configuration.Console)
            {
                loggers.Add(new ConsoleLogger());
            }

            if (!string.IsNullOrWhiteSpace(configuration.File))
            {
                loggers.Add(new FileLogger(configuration.File));
            }

            return new CompositeLogger(loggers);
        }
    }
}