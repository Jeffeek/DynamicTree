using DynamicTree.Domain.Interfaces;

namespace DynamicTree.Domain.Entities;

public class TreeNode : IIdentityEntity<long>
{
    public long Id { get; set; }
    public string Name { get; set; } = default!;
    public long? ParentNodeId { get; set; }

    public virtual TreeNode? ParentNode { get; set; }
    public virtual ICollection<TreeNode> Children { get; set; } = default!;
}