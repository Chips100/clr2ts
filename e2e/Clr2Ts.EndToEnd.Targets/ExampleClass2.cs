using Clr2Ts.EndToEnd.Targets.Generics;

namespace Clr2Ts.EndToEnd.Targets
{
    /// <summary>
    /// Simple example class type.
    /// </summary>
    public class ExampleClass2
    {
        /// <summary>
        /// Simple string property.
        /// </summary>
        public string SomeProperty { get; set; }

        /// <summary>
        /// Example for a dependency on another type.
        /// </summary>
        public ExampleClass1 DependencyOtherType { get; set; }

        /// <summary>
        /// Example for a dependency on the type itself.
        /// </summary>
        public ExampleClass2 DependencySelfRecursive { get; set; }

        /// <summary>
        /// Example for another dependency on another type.
        /// </summary>
        public ExampleClass3 AnotherDependency { get; set; }

        /// <summary>
        /// Example for a nested constructed generic type reference.
        /// </summary>
        public GenericType<ExampleClass2, GenericType<ExampleClass1, int>> NestedGenericReference { get; set; }

        /// <summary>
        /// Property with type array of bytes.
        /// </summary>
        public byte[] SomeByteArrayProperty { get; set; }

        /// <summary>
        /// Property with an array of a potentially mapped type.
        /// </summary>
        public ExampleClass3[] SomeOtherArray { get; set; }
    }
}
