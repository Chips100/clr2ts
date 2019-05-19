namespace Clr2Ts.Transpiler.Tests.SampleAssembly
{
    /// <summary>
    /// Sample class for unit tests.
    /// </summary>
    [Sample]
    public class SampleClass
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
        /// Some sample nullable integer property.
        /// </summary>
        public int? SampleNullableInt { get; set; }

        /// <summary>
        /// Example for a reference to a generic type.
        /// </summary>
        public GenericClass<string, string> GenericDependency { get; set; }

        /// <summary>
        /// Sample property that holds an enum value.
        /// </summary>
        public SampleEnum EnumProperty { get; set; }

        /// <summary>
        /// Sample property that holds an enum value with a different underlying type.
        /// </summary>
        public SampleLongEnum LongEnumProperty { get; set; }

        /// <summary>
        /// Sample property that references an interface type.
        /// </summary>
        public SampleInterface InterfaceProperty { get; set; }
    }
}