namespace DynamicTree.Application.Features.User.Journal.Models;

public class JournalInfo
{
    public long Id { get; set; }
    public long EventId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Text { get; set; } = default!;
}