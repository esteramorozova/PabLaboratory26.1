namespace AppCore.Models;

public class Note : EntityBase
{
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string CreatedBy { get; set; }
    public Guid ContactId { get; set; }
}