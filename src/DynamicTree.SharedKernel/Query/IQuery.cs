using System.Linq.Expressions;

namespace DynamicTree.SharedKernel.Query;

public interface IQuery<TSource>
{
    Expression<Func<TSource, bool>> GetExpression();
}