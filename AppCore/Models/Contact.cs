namespace AppCore.Models;

public abstract class Contact : EntityBase
{
    public required string Email { get; set; }
    public required string Phone { get; set; }

    public required Address Address { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ContactStatus Status { get; set; }
    
    public string? CreatedByUserId { get; set; }

    public List<Tag> Tags { get; set; } = new();
    public List<Note> Notes { get; set; } = new();

    public abstract string GetDisplayName();
}