using DynamicTree.Application.Features.User.Tree.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DynamicTree.Application.Features.User.Tree;

public class GetRequest : IRequest<TreeNodeInfo>
{
    [FromQuery] public string Name { get; set; } = default!;
}