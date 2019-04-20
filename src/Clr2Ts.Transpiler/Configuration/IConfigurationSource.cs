namespace Clr2Ts.Transpiler.Configuration
{
    /// <summary>
    /// Source that can be used to look up configuration values.
    /// </summary>
    public interface IConfigurationSource
    {
        /// <summary>
        /// Gets a section from this configuration source.
        /// </summary>
        /// <typeparam name="T">Type of the section that should be looked up.</typeparam>
        /// <returns>The section as it is configured in this source.</returns>
        T GetSection<T>();
    }
}