namespace DynamicTree.SharedKernel.Query;

public abstract class SortedQuery<TSource> : Query<TSource>, ISortedQuery<TSource>
{
    public abstract Func<IQueryable<TSource>, IOrderedQueryable<TSource>> GetSortingExpression();
}