using DynamicTree.Application.Features.User.Tree;
using DynamicTree.Application.Features.User.Tree.Models;
using DynamicTree.SharedKernel.Endpoints;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DynamicTree.Web.Endpoints.User.Tree;

public class GetEndpoint : BaseEndpoint<GetRequest, TreeNodeInfo, GetEndpoint>
{
    [HttpPost("user.tree.get")]
    [OpenApiOperation(
        operationId: "User.Tree.Get",
        summary: "Returns your entire tree. If your tree doesn't exist it will be created automatically.",
        description: "Returns your entire tree. If your tree doesn't exist it will be created automatically."
    )]
    [OpenApiTags("user.tree")]
    [Produces(typeof(TreeNodeInfo))]
    public override Task<ActionResult<TreeNodeInfo>> HandleAsync([FromQuery] GetRequest request, CancellationToken cancellationToken = default)
        => base.HandleAsync(request, cancellationToken);
}