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

    public static PersonDto FromEntity(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);

        return new PersonDto
        {
            Id = person.Id,
            Email = person.Email,
            Phone = person.Phone,
            Address = new AddressDto(
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
            EmployerId = person.Employer?.Id
        };
    }

    public static Person ToEntity(CreatePersonDto dto, Guid? id = null)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (dto.Address is null) throw new ArgumentException("Address is required.", nameof(dto));

        return new Person
        {
            Id = id ?? Guid.Empty,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = new Address
            {
                Id = Guid.NewGuid(),
                Street = dto.Address.Street,
                City = dto.Address.City,
                PostalCode = dto.Address.PostalCode,
                Country = dto.Address.Country,
                Type = dto.Address.Type
            },
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