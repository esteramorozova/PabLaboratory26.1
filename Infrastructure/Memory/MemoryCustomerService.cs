using AppCore.Interfaces;
using AppCore.Models;

namespace Infrastructure.Memory;

public class MemoryCustomerService: ICustomerService
{
    public IEnumerable<Customer> GetCustomers()
    {
        return [
        new Customer()
            {
                Id = 1,
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "a@wsei.edu.pl",
                Phone = "123-456-789",
                AddressId = 11
            },
            new Customer()
                {
                    Id = 2,
                    FirstName = "Anna",
                    LastName = "Nowak",
                    Email = "b@wsei.edu.pl",
                    Phone = "987-654-321",
                    AddressId = 22
                }
            ];
    }

    public Task<IEnumerable<Customer>> GetCustomersAsync()
    {
        throw new NotImplementedException();
    }
}