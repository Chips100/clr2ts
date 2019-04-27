using System.Collections.Generic;

namespace Clr2Ts.Transpiler.Tests.SampleAssembly
{
    /// <summary>
    /// Example for a generic type.
    /// </summary>
    /// <typeparam name="T1">First type paramter.</typeparam>
    /// <typeparam name="T2">Second type parameter.</typeparam>
    public sealed class GenericClass<T1, T2>
    {
        /// <summary>
        /// Property of generic type 1.
        /// </summary>
        public T1 Property1 { get; set; }

        /// <summary>
        /// Property of generic type 2.
        /// </summary>
        public T2 Property2 { get; set; }

        /// <summary>
        /// Dictionary with the generic types.
        /// </summary>
        public IDictionary<string, T2> Dictionary { get; set; }

        /// <summary>
        /// Example for a generic reference with one type parameter from this type.
        /// </summary>
        public GenericClass2<T1, string> GenericDependency { get; set; }

        /// <summary>
        /// Example for a generic reference with one type parameter from this type.
        /// </summary>
        public GenericClass2<T1, T2> GenericDependency2 { get; set; }

        /// <summary>
        /// Example for a generic reference with supplied type parameters.
        /// </summary>
        public GenericClass2<SampleClass, SampleClass> GenericDependency3 { get; set; }
    }
}