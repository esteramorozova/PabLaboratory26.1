namespace AppCore.Dto;
using AppCore.Models;

public record PersonDto : ContactBaseDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public string? Position { get; init; }
    public DateTime? BirthDate { get; init; }
    public Gender Gender { get; init; }
    public Guid? EmployerId { get; init; }
    public List<Note> Notes { get; init; } = new();

    public static PersonDto FromEntity(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);

        return new PersonDto
        {
            Id = person.Id,
            Email = person.Email,
            Phone = person.Phone,
            Address = IsPlaceholderAddress(person.Address)
                ? null
                : new AddressDto(
                    person.Address.Street,
                    person.Address.City,
                    person.Address.PostalCode,
                    person.Address.Country,
                    person.Address.Type
                ),
            Status = person.Status,
            Tags = person.Tags.Select(t => t.Name).ToList(),
            CreatedAt = person.CreatedAt,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Position = person.Position,
            BirthDate = person.BirthDate,
            Gender = person.Gender,
            EmployerId = person.Employer?.Id,
            Notes = person.Notes
        };
    }

    public static Person ToEntity(CreatePersonDto dto, Guid? id = null)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var addressEntity = dto.Address is null
                ? CreatePlaceholderAddress()
                : new Address
            {
                Id = Guid.NewGuid(),
                Street = dto.Address.Street,
                City = dto.Address.City,
                PostalCode = dto.Address.PostalCode,
                Country = dto.Address.Country,
                Type = dto.Address.Type
            };

        return new Person
        {
            Id = id ?? Guid.Empty,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = addressEntity,
            CreatedAt = DateTime.UtcNow,
            Status = ContactStatus.Active,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Position = dto.Position,
            BirthDate = dto.BirthDate, 
            Gender = dto.Gender, 
            Employer = null,
            Organization = null
        };
    }
    
    private static Address CreatePlaceholderAddress() =>
        new()
        {
            Id = Guid.NewGuid(),
            Street = string.Empty,
            City = string.Empty,
            PostalCode = string.Empty,
            Country = string.Empty,
            Type = AddressType.Main
        };

    private static bool IsPlaceholderAddress(Address address) =>
        address.Street.Length == 0
        && address.City.Length == 0
        && address.PostalCode.Length == 0
        && address.Country.Length == 0;
}

public record CreatePersonDto(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string? Position,
    DateTime? BirthDate,
    Gender Gender,
    Guid? EmployerId,
    AddressDto? Address
);

public record UpdatePersonDto(
    Guid Id,
    string? FirstName,
    string? LastName,
    string? Email,
    string? Phone,
    string? Position,
    DateTime? BirthDate,
    Gender? Gender,
    Guid? EmployerId,
    AddressDto? Address,
    ContactStatus? Status
);