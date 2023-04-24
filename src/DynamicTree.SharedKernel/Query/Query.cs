using System.Linq.Expressions;

namespace DynamicTree.SharedKernel.Query;

public abstract class Query<TSource> : IQuery<TSource>
{
    public virtual Expression<Func<TSource, bool>> GetExpression() => uniqueEntity => true;
}