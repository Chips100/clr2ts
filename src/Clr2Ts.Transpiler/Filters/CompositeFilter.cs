using System;
using System.Collections.Generic;
using System.Linq;

namespace Clr2Ts.Transpiler.Filters
{
    /// <summary>
    /// Combines multiple filters to a single filter.
    /// </summary>
    public static class CompositeFilter
    {
        /// <summary>
        /// Creates a filter that evaluates to <see langword="true" /> iff at least one of underlying filters evaluates to <see langword="true" />.
        /// </summary>
        /// <typeparam name="T">Type of the items that the filter can be applied to.</typeparam>
        /// <param name="filters">Underlying filters that should be combined.</param>
        /// <returns>A filter that is a combination of the specified filters.</returns>
        public static IFilter<T> Or<T>(IEnumerable<IFilter<T>> filters)
            => new CompositeFilterImplementation<T>(filters, x => x.Any(y => y()));

        /// <summary>
        /// Creates a filter that evaluates to <see langword="true" /> iff all underlying filters evaluate to <see langword="true" />.
        /// </summary>
        /// <typeparam name="T">Type of the items that the filter can be applied to.</typeparam>
        /// <param name="filters">Underlying filters that should be combined.</param>
        /// <returns>A filter that is a combination of the specified filters.</returns>
        public static IFilter<T> And<T>(IEnumerable<IFilter<T>> filters)
            => new CompositeFilterImplementation<T>(filters, x => x.All(y => y()));

        private class CompositeFilterImplementation<T>: IFilter<T>
        {
            private readonly IEnumerable<IFilter<T>> _filters;
            private readonly Func<IEnumerable<Func<bool>>, bool> _aggregation;

            public CompositeFilterImplementation(IEnumerable<IFilter<T>> filters, Func<IEnumerable<Func<bool>>, bool> aggregation)
            {
                if (filters == null) throw new ArgumentNullException(nameof(filters));

                _filters = filters.ToList();
                _aggregation = aggregation;
            }

            public bool IsMatch(T item) => _aggregation(_filters.Select<IFilter<T>, Func<bool>>(
                f => () => f.IsMatch(item)));
        }
    }
}