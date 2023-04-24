using DynamicTree.Application.Features.User.Tree.Node;
using DynamicTree.SharedKernel.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DynamicTree.Web.Endpoints.User.Tree.Node;

public class DeleteEndpoint : BaseEndpoint<DeleteRequest, Unit, CreateEndpoint>
{
    [HttpPost("user.tree.node.delete")]
    [OpenApiOperation(
        operationId: "User.Tree.Node.Delete",
        summary: "Delete an existing node in your tree. You must specify a node ID that belongs your tree.",
        description: "Delete an existing node in your tree. You must specify a node ID that belongs your tree."
    )]
    [OpenApiTags("user.tree.node")]
    [Produces(typeof(Unit))]
    public override Task<ActionResult<Unit>> HandleAsync([FromQuery] DeleteRequest request, CancellationToken cancellationToken = default)
        => base.HandleAsync(request, cancellationToken);
}