using DynamicTree.Application.Features.User.Journal;
using DynamicTree.Application.Features.User.Journal.Models;
using DynamicTree.SharedKernel.Endpoints;
using DynamicTree.SharedKernel.Paging;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DynamicTree.Web.Endpoints.User.Journal;

public class GetRangeEndpoint : BaseEndpoint<GetRangeRequest, PagedList<JournalViewInfo>, GetRangeEndpoint>
{
    [HttpPost("user.journal.getRange")]
    [OpenApiOperation(
        operationId: "User.Journal.GetRange",
        summary: "Provides the pagination API. Skip means the number of items should be skipped by server. Take means the maximum number items should be returned by server. All fields of the filter are optional.",
        description: "Provides the pagination API. Skip means the number of items should be skipped by server. Take means the maximum number items should be returned by server. All fields of the filter are optional."
    )]
    [OpenApiTags("user.journal")]
    [Produces(typeof(PagedList<JournalViewInfo>))]
    public override Task<ActionResult<PagedList<JournalViewInfo>>> HandleAsync([FromQuery] GetRangeRequest request, CancellationToken cancellationToken = default)
        => base.HandleAsync(request, cancellationToken);
}