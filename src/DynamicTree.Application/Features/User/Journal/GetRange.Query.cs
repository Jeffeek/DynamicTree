using System.Linq.Expressions;
using DynamicTree.SharedKernel.Extensions;
using DynamicTree.SharedKernel.Query;

namespace DynamicTree.Application.Features.User.Journal;

public class GetRangeQuery : SortedQuery<Domain.Entities.Journal>
{
    public GetRangeFilter? Filter { get; set; }

    public override Expression<Func<Domain.Entities.Journal, bool>> GetExpression()
    {
        var result = base.GetExpression();

        if (Filter == null) return result;

        if (Filter.From.HasValue)
            result = result.And(x => x.CreatedAt >= Filter.From);

        if (Filter.To.HasValue)
            result = result.And(x => x.CreatedAt <= Filter.To);

        if (!string.IsNullOrEmpty(Filter.Search))
            result = Filter.Search.Split().Aggregate(result, (current, term) => current.And(c => c.Text.Contains(term)));

        return result;
    }

    public override Func<IQueryable<Domain.Entities.Journal>, IOrderedQueryable<Domain.Entities.Journal>> GetSortingExpression()
        => x => x.OrderBy(c => c.CreatedAt);
}