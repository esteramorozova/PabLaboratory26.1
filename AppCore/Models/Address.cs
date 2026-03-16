namespace AppCore.Models;

public class Address : EntityBase
{
    public required string Street { get; set; }
    public required string City { get; set; }
    public required string PostalCode { get; set; }
    public required string Country { get; set; }

    public AddressType Type { get; set; }
}