namespace Clr2Ts.Transpiler.Tests.SampleAssembly
{
    /// <summary>
    /// Type that will not be transpiled because it matched the filter criteria,
    /// but rather because it is referenced by a type that was matched.
    /// </summary>
    public sealed class TransitiveDependency
    {
        /// <summary>
        /// Example property.
        /// </summary>
        public string SomeValue { get; set; }

        /// <summary>
        /// Transitive dependency on another type that does not match the filter criteria.
        /// </summary>
        public TransitiveDependency2 Other { get; set; }
    }
}