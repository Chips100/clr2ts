namespace Clr2Ts.Transpiler.Tests.SampleAssembly
{
    /// <summary>
    /// Example of an enum type.
    /// </summary>
    public enum SampleEnum
    {
        /// <summary>
        /// Documentation of Value1.
        /// </summary>
        Value1 = 1,

        /// <summary>
        /// Documentation of Value2.
        /// </summary>
        Value2 = 2,

        // Arbitrary order with explicit values.

        /// <summary>
        /// Documentation of Value4.
        /// </summary>
        Value4 = 4,

        /// <summary>
        /// Documentation of Value3.
        /// </summary>
        Value3 = 3,

        /// <summary>
        /// Documentation of OtherNameForValue4.
        /// </summary>
        OtherNameForValue4 = 4
    }
}