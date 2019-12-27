using System;

namespace Clr2Ts.EndToEnd.Targets
{
    /// <summary>
    /// Simple example class type.
    /// </summary>
    public class ExampleClass1
    {
        /// <summary>
        /// Simple string property.
        /// </summary>
        public string SomeProperty { get; set; }

        /// <summary>
        /// Property with type Guid.
        /// </summary>
        public Guid SomeGuidProperty { get; set; }

        /// <summary>
        /// Property with type boolean.
        /// </summary>
        public bool SomeBooleanProperty { get; set; }

        /// <summary>
        /// Property with type DateTime.
        /// </summary>
        public DateTime SomeDateTimeProperty { get; set; }

        /// <summary>
        /// Property with type Int32.
        /// </summary>
        public int SomeIntegerProperty { get; set; }
    }
}
