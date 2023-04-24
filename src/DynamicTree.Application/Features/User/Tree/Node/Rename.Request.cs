using MediatR;

namespace DynamicTree.Application.Features.User.Tree.Node;

public class RenameRequest : IRequest<Unit>
{
    public string TreeName { get; set; } = default!;
    public long NodeId { get; set; }
    public string NewNodeName { get; set; } = default!;
}