namespace AppCore.Models;

public class Organization : EntityBase
{
    public string Name { get; set; }
    public OrganizationType Type { get; set; }

    public string? KRS { get; set; }
    public string? Website { get; set; }
    public string? Mission { get; set; }

    public List<Person> Members { get; set; } = new();
    public Person? PrimaryContact { get; set; }

    public string GetDisplayName()
    {
        return Name;
    }
}