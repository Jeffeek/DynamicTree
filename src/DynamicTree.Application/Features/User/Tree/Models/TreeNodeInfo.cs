namespace DynamicTree.Application.Features.User.Tree.Models;

public class TreeNodeInfo
{
    public long Id { get; set; }
    public string Name { get; set; } = default!;
    public List<TreeNodeInfo> Children { get; set; } = default!;
}