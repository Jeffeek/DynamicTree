using DynamicTree.SharedKernel.Query;

namespace DynamicTree.SharedKernel.Extensions;

public static class QueryableExtensions
{
    public static IOrderedQueryable<T> ApplyQuery<T>(this IQueryable<T> items, ISortedQuery<T> query) where T : class
    {
        var result = items.Where(query.GetExpression());

        return query.GetSortingExpression()(result);
    }

    public static IQueryable<T> ApplyQuery<T>(this IQueryable<T> items, IQuery<T> query) where T : class
    {
        var result = items.Where(query.GetExpression());

        return result;
    }
}