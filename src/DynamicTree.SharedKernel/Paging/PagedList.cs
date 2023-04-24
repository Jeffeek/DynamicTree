using Microsoft.EntityFrameworkCore;

namespace DynamicTree.SharedKernel.Paging;

public class PagedList<T>
{
    public List<T> Items { get; private init; } = default!;
    public int Count { get; private init; }
    public int Skip { get; private init; }

    public static async Task<PagedList<T>> InitializeAsync(IQueryable<T> items, int skip, int take, CancellationToken cancellationToken = default)
        => new()
        {
            Items = await items.Skip(skip).Take(take).ToListAsync(cancellationToken),
            Count = await items.CountAsync(cancellationToken),
            Skip = skip
        };
}