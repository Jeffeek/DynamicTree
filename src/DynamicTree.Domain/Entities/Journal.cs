using DynamicTree.Domain.Interfaces;

namespace DynamicTree.Domain.Entities;

public class Journal : IAuditableEntity, IIdentityEntity<long>
{
    public long Id { get; set; }
    public long EventId { get; set; }
    public string Text { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}