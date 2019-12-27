namespace Clr2Ts.Transpiler.Transpilation.Configuration
{
    /// <summary>
    /// Defines strategies for adding default values to TypeScript properties.
    /// </summary>
    public enum DefaultValueStrategy
    {
        /// <summary>
        /// Always assigning null to all properties (default).
        /// </summary>
        AlwaysNull,

        /// <summary>
        /// No assignment of a default value.
        /// </summary>
        None,

        /// <summary>
        /// Assign default values for primitive types, like Boolean or Int32.
        /// All other types will be assigned null.
        /// </summary>
        PrimitiveDefaults
    }
}