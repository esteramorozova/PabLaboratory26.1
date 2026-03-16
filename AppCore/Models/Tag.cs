namespace AppCore.Models;

public class Tag : EntityBase
{
    public required string Name { get; set; }
    public required string Color { get; set; }
}