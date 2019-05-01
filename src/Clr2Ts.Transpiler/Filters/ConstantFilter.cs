namespace Clr2Ts.Transpiler.Filters
{
    /// <summary>
    /// Creates a filter that matches either all or no items.
    /// </summary>
    public static class ConstantFilter
    {
        /// <summary>
        /// Creates a filter that matches either all or no items.
        /// </summary>
        /// <typeparam name="T">Type of the items that are matched by this filter.</typeparam>
        /// <param name="result">True, if all items should be matched; otherwise false.</param>
        /// <returns>A filter that matches all items if <paramref name="result"/> was true; otherwise a filter that matches no items.</returns>
        public static IFilter<T> Create<T>(bool result)
            => new ConstantFilterImplementation<T>(result);

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