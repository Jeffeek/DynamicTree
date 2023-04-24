namespace DynamicTree.SharedKernel.Query;

public interface ISortedQuery<TSource> : IQuery<TSource>
{
    Func<IQueryable<TSource>, IOrderedQueryable<TSource>> GetSortingExpression();
}