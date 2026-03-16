namespace AppCore.Models;

public class Person : Contact
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? MiddleName { get; set; }

    public DateTime? BirthDate { get; set; }

    public Gender Gender { get; set; }
    public string? Position { get; set; }

    public Organization? Organization { get; set; }
    public Company? Employer { get; set; }

    public override string GetDisplayName()
    {
        return $"{FirstName} {LastName}";
    }
}