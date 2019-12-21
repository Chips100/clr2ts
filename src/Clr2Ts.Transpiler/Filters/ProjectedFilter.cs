using System;

namespace Clr2Ts.Transpiler.Filters
{
    /// <summary>
    /// Creates a filter that makes use of an existing filter designed 
    /// for other item types by projecting items passed to the filter.
    /// </summary>
    public static class ProjectedFilter
    {
        /// <summary>
        /// Creates a filter that makes use of an existing filter designed 
        /// for other item types by projecting items passed to the filter.
        /// </summary>
        /// <param name="originalFilter">Filter that defines the underlying filter criteria.</param>
        /// <param name="projection">Projection to apply to items to match them against <paramref name="originalFilter"/>.</param>
        public static IFilter<TProjected> Create<TOriginal, TProjected>(IFilter<TOriginal> originalFilter, Func<TProjected, TOriginal> projection)
            => new ProjectedFilterImplementation<TOriginal, TProjected>(originalFilter, projection);

        private sealed class ProjectedFilterImplementation<TOriginal, TProjected> : IFilter<TProjected>
        {
            private readonly IFilter<TOriginal> _originalFilter;
            private readonly Func<TProjected, TOriginal> _projection;

            public ProjectedFilterImplementation(IFilter<TOriginal> originalFilter, Func<TProjected, TOriginal> projection)
            {
                _originalFilter = originalFilter ?? throw new ArgumentNullException(nameof(originalFilter));
                _projection = projection ?? throw new ArgumentNullException(nameof(projection));
            }

            public bool IsMatch(TProjected item)
                => _originalFilter.IsMatch(_projection(item));
        }
    }
}