namespace Clr2Ts.Transpiler.Filters
{
    /// <summary>
    /// Creates a filter that matches either all or no items.
    /// </summary>
    public static class ConstantFilter
    {
        /// <summary>
        /// Creates a filter that matches all items.
        /// </summary>
        /// <typeparam name="T">Type of the items that are matched by this filter.</typeparam>
        /// <returns>A filter that matches all items.</returns>
        public static IFilter<T> MatchAll<T>()
            => new ConstantFilterImplementation<T>(true);

        /// <summary>
        /// Creates a filter that matches no items.
        /// </summary>
        /// <typeparam name="T">Type of the items that are matched by this filter.</typeparam>
        /// <returns>A filter that matches no items.</returns>
        public static IFilter<T> MatchNone<T>()
            => new ConstantFilterImplementation<T>(false);


        private class ConstantFilterImplementation<T> : IFilter<T>
        {
            private readonly bool _result;

            public ConstantFilterImplementation(bool result)
            {
                _result = result;
            }

            public bool IsMatch(T item) => _result;
        }
    }
}