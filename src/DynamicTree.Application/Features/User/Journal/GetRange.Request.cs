using DynamicTree.Application.Features.User.Journal.Models;
using DynamicTree.SharedKernel.Paging;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DynamicTree.Application.Features.User.Journal;

public class GetRangeRequest : IRequest<PagedList<JournalViewInfo>>
{
    [FromQuery] public int Skip { get; set; }
    [FromQuery] public int Take { get; set; }
    [FromBody] public GetRangeFilter? Filter { get; set; }
}

public class GetRangeFilter
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public string? Search { get; set; }
}