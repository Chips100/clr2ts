﻿using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Output.Files;
using System;
using System.Collections.Generic;

namespace Clr2Ts.Transpiler.Output
{
    /// <summary>
    /// Factory that creates instances of <see cref="ICodeWriter"/>.
    /// </summary>
    public static class CodeWriterFactory
    {
        /// <summary>
        /// Creates an <see cref="ICodeWriter"/> for the specified configuration.
        /// </summary>
        /// <param name="configurationSource">Source for the configuration that should be used.</param>
        /// <returns>An instance of <see cref="ICodeWriter"/> for the specified configuration.</returns>
        public static ICodeWriter FromConfiguration(IConfigurationSource configurationSource)
        {
            if (configurationSource == null) throw new ArgumentNullException(nameof(configurationSource));

            var configuration = configurationSource.GetRequiredSection<OutputConfiguration>();
            var writers = new List<ICodeWriter>();

            if (!string.IsNullOrWhiteSpace(configuration.BundledFile))
            {
                writers.Add(new BundledFileCodeWriter(configuration.BundledFile));
            }

            if (configuration.Files != null)
            {
                writers.Add(new FileCodeWriter(configuration.Files.Directory,
                    configuration.Files.MimicNamespacesWithSubdirectories));
            }

            return new CompositeCodeWriter(writers);
        }
    }
}