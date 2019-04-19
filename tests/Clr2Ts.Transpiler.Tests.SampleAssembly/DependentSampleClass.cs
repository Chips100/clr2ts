namespace Clr2Ts.Transpiler.Tests.SampleAssembly
{
    /// <summary>
    /// Example for a type that references another transpiled type.
    /// </summary>
    public class DependentSampleClass
    {
        /// <summary>
        /// Property that references another type.
        /// </summary>
        public SampleClass Dependency { get; set; }

        /// <summary>
        /// Normal property.
        /// </summary>
        public string OtherProperty { get; set; }
    }
}