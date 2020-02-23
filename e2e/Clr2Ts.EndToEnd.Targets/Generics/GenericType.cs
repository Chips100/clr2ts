namespace Clr2Ts.EndToEnd.Targets.Generics
{
    /// <summary>
    /// Example for a generic type.
    /// </summary>
    /// <typeparam name="T1">First type parameter.</typeparam>
    /// <typeparam name="T2">Second type parameter.</typeparam>
    public class GenericType<T1, T2>
    {
        /// <summary>
        /// Property referencing the first type parameter.
        /// </summary>
        public T1 Type1 { get; set; }

        /// <summary>
        /// Property referencing the second type parameter.
        /// </summary>
        public T2 Type2 { get; set; }
    }
}