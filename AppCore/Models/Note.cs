namespace AppCore.Models;

public class Note : EntityBase
{
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
}