using DynamicTree.Application.Features.User.Tree.Node;
using DynamicTree.SharedKernel.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DynamicTree.Web.Endpoints.User.Tree.Node;

public class CreateEndpoint : BaseEndpoint<CreateRequest, Unit, CreateEndpoint>
{
    [HttpPost("user.tree.node.create")]
    [OpenApiOperation(
        operationId: "User.Tree.Node.Create",
        summary: "Create a new node in your tree. You must to specify a parent node ID that belongs to your tree. A new node name must be unique across all siblings.",
        description: "Create a new node in your tree. You must to specify a parent node ID that belongs to your tree. A new node name must be unique across all siblings."
    )]
    [OpenApiTags("user.tree.node")]
    [Produces(typeof(Unit))]
    public override Task<ActionResult<Unit>> HandleAsync([FromQuery] CreateRequest request, CancellationToken cancellationToken = default)
        => base.HandleAsync(request, cancellationToken);
}