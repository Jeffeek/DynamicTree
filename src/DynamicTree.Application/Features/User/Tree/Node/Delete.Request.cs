using MediatR;

namespace DynamicTree.Application.Features.User.Tree.Node;

public class DeleteRequest : IRequest<Unit>
{
    public string TreeName { get; set; } = default!;
    public long NodeId { get; set; }
}