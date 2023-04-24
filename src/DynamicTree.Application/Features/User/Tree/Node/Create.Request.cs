using MediatR;

namespace DynamicTree.Application.Features.User.Tree.Node;

public class CreateRequest : IRequest<Unit>
{
    public string TreeName { get; set; } = default!;
    public long ParentNodeId { get; set; }
    public string NodeName { get; set; } = default!;
}