namespace AppCore.Dto;
using AppCore.Models;

public abstract record ContactBaseDto
{
    public Guid Id { get; init; }
    public required string Email { get; init; }
    public required string Phone { get; init; }
    public AddressDto? Address { get; init; }
    public ContactStatus Status { get; init; }
    public List<string> Tags { get; init; } = new();
    public DateTime CreatedAt { get; init; }
    public string? CreatedByUserId { get; init; }
}

public record AddressDto(
    string Street,
    string City,
    string PostalCode,
    string Country,
    AddressType Type
);