using Clr2Ts.Transpiler.Configuration;

namespace Clr2Ts.Transpiler.Transpilation
{
    /// <summary>
    /// Represents the transpilation-specific configuration section for a transpilation process.
    /// </summary>
    [ConfigurationSection("transpilation")]
    public sealed class TranspilationConfiguration
    {
        /// <summary>
        /// Creates an <see cref="TranspilationConfiguration"/>.
        /// </summary>
        /// <param name="camelCase">If set to true, property names should be converted to camelCase.</param>
        public TranspilationConfiguration(bool? camelCase)
        {
            CamelCase = camelCase ?? true; // Defaults to true.
        }

        /// <summary>
        /// If set to true, property names should be converted to camelCase.
        /// </summary>
        public bool CamelCase { get; set; }

        /// <summary>
        /// Returns a default configuration for the transpilation
        /// that should be used if the section has been omitted.
        /// </summary>
        public static TranspilationConfiguration Default => new TranspilationConfiguration(true);
    }
}