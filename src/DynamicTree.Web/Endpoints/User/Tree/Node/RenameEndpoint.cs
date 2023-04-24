using DynamicTree.Application.Features.User.Tree.Node;
using DynamicTree.SharedKernel.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DynamicTree.Web.Endpoints.User.Tree.Node;

public class RenameEndpoint : BaseEndpoint<RenameRequest, Unit, RenameEndpoint>
{
    [HttpPost("user.tree.node.rename")]
    [OpenApiOperation(
        operationId: "User.Tree.Node.Rename",
        summary: "Rename an existing node in your tree. You must specify a node ID that belongs your tree. A new name of the node must be unique across all siblings.",
        description: "Rename an existing node in your tree. You must specify a node ID that belongs your tree. A new name of the node must be unique across all siblings."
    )]
    [OpenApiTags("user.tree.node")]
    [Produces(typeof(Unit))]
    public override Task<ActionResult<Unit>> HandleAsync([FromQuery] RenameRequest request, CancellationToken cancellationToken = default)
        => base.HandleAsync(request, cancellationToken);
}