using Clr2Ts.Transpiler.Tests.SampleDependency;

namespace Clr2Ts.Transpiler.Tests.SampleAssembly
{
    /// <summary>
    /// Type that has a reference to a type defined in another assembly
    /// that is not picked up automatically by the configuration.
    /// </summary>
    [Sample]
    public sealed class DependentOnOtherAssemblyClass
    {
        /// <summary>
        /// Dependency from another assembly.
        /// </summary>
        public SampleDependencyFromOtherAssembly Dependency { get; set; }
    }
}