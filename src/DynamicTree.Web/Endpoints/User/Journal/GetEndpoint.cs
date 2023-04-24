using DynamicTree.Application.Features.User.Journal;
using DynamicTree.Application.Features.User.Journal.Models;
using DynamicTree.SharedKernel.Endpoints;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DynamicTree.Web.Endpoints.User.Journal;

public class GetEndpoint : BaseEndpoint<GetRequest, JournalInfo, GetEndpoint>
{
    [HttpPost("user.journal.getSingle")]
    [OpenApiOperation(
        operationId: "User.Journal.GetSingle",
        summary: "Returns the information about an particular event by ID.",
        description: "Returns the information about an particular event by ID."
    )]
    [OpenApiTags("user.journal")]
    [Produces(typeof(JournalInfo))]
    public override Task<ActionResult<JournalInfo>> HandleAsync([FromQuery] GetRequest request, CancellationToken cancellationToken = default)
        => base.HandleAsync(request, cancellationToken);
}