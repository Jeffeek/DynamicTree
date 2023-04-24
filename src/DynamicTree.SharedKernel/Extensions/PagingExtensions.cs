using DynamicTree.SharedKernel.Paging;

namespace DynamicTree.SharedKernel.Extensions;

public static class PagingExtensions
{
    public static Task<PagedList<T>> ApplyPagingAsync<T>(this IQueryable<T> items, int skip, int take, CancellationToken cancellationToken)
        => PagedList<T>.InitializeAsync(items, skip, take, cancellationToken);
}