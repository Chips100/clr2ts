namespace Clr2Ts.Transpiler.Tests.SampleAssembly.SubNamespace
{
    /// <summary>
    /// Sample class from a subnamespace in the target assembly.
    /// </summary>
    [Sample]
    public sealed class SomeClassFromSubNamespace
    {
        /// <summary>
        /// Some sample string property.
        /// </summary>
        public string SampleString { get; set; }

        /// <summary>
        /// Some sample integer property.
        /// </summary>
        public int SampleInt { get; set; }

        /// <summary>
        /// Example for a reference to a generic type.
        /// </summary>
        public GenericClass<string, string> GenericDependency { get; set; }
    }
}