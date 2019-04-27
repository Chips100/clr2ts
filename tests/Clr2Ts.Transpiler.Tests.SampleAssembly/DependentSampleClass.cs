using System.Collections.Generic;

namespace Clr2Ts.Transpiler.Tests.SampleAssembly
{
    /// <summary>
    /// Example for a type that references another transpiled type.
    /// </summary>
    [Sample]
    public class DependentSampleClass
    {
        /// <summary>
        /// Property that references another type.
        /// </summary>
        public IEnumerable<SampleClass> Dependency { get; set; }

        /// <summary>
        /// Property with a dictionary.
        /// </summary>
        public IDictionary<string, SampleClass> DictionaryDependencies { get; set; }

        /// <summary>
        /// Normal property.
        /// </summary>
        public string OtherProperty { get; set; }
    }
}