namespace Clr2Ts.Transpiler.Filters
{
    /// <summary>
    /// Represents a filter to match a subset of items from a specific type.
    /// </summary>
    /// <typeparam name="T">Type of items that can be matched by this filter.</typeparam>
    public interface IFilter<in T>
    {
        /// <summary>
        /// Determines if the specified item is matched by this filter.
        /// </summary>
        /// <param name="item">Item to check.</param>
        /// <returns>True, if the item is matched by this filter; otherwise false.</returns>
        bool IsMatch(T item);
    }
}