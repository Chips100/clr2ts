﻿namespace Clr2Ts.Transpiler.Tests.SampleAssembly
{
    /// <summary>
    /// Another example for a generic type.
    /// </summary>
    /// <typeparam name="T1">First type paramter.</typeparam>
    /// <typeparam name="T2">Second type parameter.</typeparam>
    public sealed class GenericClass2<T1, T2>
    {
        /// <summary>
        /// Property of generic type 1.
        /// </summary>
        public T1 Property1 { get; set; }

        /// <summary>
        /// Property of generic type 2.
        /// </summary>
        public T2 Property2 { get; set; }
    }
}