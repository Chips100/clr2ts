using Clr2Ts.EndToEnd.Targets.Attributes;

namespace Clr2Ts.EndToEnd.Targets
{
    /// <summary>
    /// Example for an attributed class.
    /// </summary>
    [ClassLevel("AttributedExample")]
    [MyDisplay("DisplayNameForAttributedExampleClass", "Description for attributed example class")]
    public sealed class AttributedExampleClass
    {
        /// <summary>
        /// Some attributed example string property.
        /// </summary>
        [MyDisplay("DisplaNameForSomeProperty", "Description for some property")]
        public string SomeProperty { get; set; }

        /// <summary>
        /// Some example string property without any attributes.
        /// </summary>
        public string UnattributedProperty { get; set; }
    }
}
