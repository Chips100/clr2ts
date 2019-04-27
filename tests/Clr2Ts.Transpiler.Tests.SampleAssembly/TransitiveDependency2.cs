namespace Clr2Ts.Transpiler.Tests.SampleAssembly
{
    /// <summary>
    /// Type that will not be transpiled because it matched the filter criteria,
    /// but rather because it is referenced by another type - which was also only transpiled
    /// because it was referenced by a type that matched the filter criteria.
    /// </summary>
    public sealed class TransitiveDependency2
    {
        /// <summary>
        /// Cyclic dependency.
        /// </summary>
        public TransitiveDependency Loop { get; set; }
    }
}
