using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clr2Ts.Transpiler.Tests.SampleAssembly
{
    /// <summary>
    /// Sample class for unit tests.
    /// </summary>
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
    }
}