using Clr2Ts.EndToEnd.Targets.Attributes;

namespace Clr2Ts.EndToEnd.Targets
{
    /// <summary>
    /// An example enum with display attributes.
    /// </summary>
    public enum ExampleEnum
    {
        /// <summary>
        /// First value.
        /// </summary>
        [MyDisplay("DisplayName1", "DisplayDescription1")]
        Value1 = 0,

        /// <summary>
        /// Second value.
        /// </summary>
        [MyDisplay("DisplayName2", "DisplayDescription2")]
        Value2 = 1,

        /// <summary>
        /// Third value.
        /// </summary>
        [MyDisplay("DisplayName3", "DisplayDescription3")]
        Value3 = 4
    }
}