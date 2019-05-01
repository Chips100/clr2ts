namespace Clr2Ts.Transpiler.Tests.SampleDependency
{
    /// <summary>
    /// Sample dependency that is defined in another assembly
    /// that is not configured to be picked up automatically.
    /// </summary>
    public sealed class SampleDependencyFromOtherAssembly
    {
        /// <summary>
        /// Some example value.
        /// </summary>
        public string SomeValue { get; set; }
    }
}