using AppCore.Interfaces;
using AppCore.Models;
using Infrastructure.Memory;

namespace UnitTest;

public class MemoryGenericRepositoryTest
{
    private static Person CreateValidPerson(Guid? id = null)
    {
        return new Person
        {
            Id = id ?? Guid.Empty,
            FirstName = "Adam",
            LastName = "Nowak",
            Email = "adam.nowak@example.com",
            Phone = "123-456-789",
            Address = new Address
            {
                Id = Guid.NewGuid(),
                Street = "Main St 1",
                City = "Krakow",
                PostalCode = "30-001",
                Country = "PL",
                Type = AddressType.Main
            },
            CreatedAt = DateTime.UtcNow,
            Status = ContactStatus.Active,
            Gender = Gender.Male
        };
    }

    [Fact]
    public async Task AddPerson_AssignsId_WhenEmpty()
    {
        IGenericRepositoryAsync<Person> repo = new MemoryGenericRepository<Person>();
        var expected = CreateValidPerson(id: Guid.Empty);

        await repo.AddAsync(expected);

        Assert.NotEqual(Guid.Empty, expected.Id);
        var actual = await repo.FindByIdAsync(expected.Id);
        Assert.Same(expected, actual);
        Assert.Equal(expected.Id, actual?.Id);
    }

    [Fact]
    public async Task FindById_ReturnsNull_WhenMissing()
    {
        IGenericRepositoryAsync<Person> repo = new MemoryGenericRepository<Person>();

        var actual = await repo.FindByIdAsync(Guid.NewGuid());

        Assert.Null(actual);
    }

    [Fact]
    public async Task FindAll_ReturnsAllItems()
    {
        IGenericRepositoryAsync<Person> repo = new MemoryGenericRepository<Person>();
        var p1 = CreateValidPerson();
        var p2 = CreateValidPerson();

        await repo.AddAsync(p1);
        await repo.AddAsync(p2);

        var all = (await repo.FindAllAsync()).ToList();
        Assert.Equal(2, all.Count);
        Assert.Contains(p1, all);
        Assert.Contains(p2, all);
    }

    [Fact]
    public async Task FindPaged_ReturnsCorrectPage()
    {
        IGenericRepositoryAsync<Person> repo = new MemoryGenericRepository<Person>();
        var people = Enumerable.Range(1, 10).Select(_ => CreateValidPerson()).ToList();
        foreach (var p in people)
        {
            await repo.AddAsync(p);
        }

        var page2 = await repo.FindPagedAsync(page: 2, pageSize: 3);

        Assert.Equal(10, page2.TotalCount);
        Assert.Equal(2, page2.Page);
        Assert.Equal(3, page2.PageSize);
        Assert.Equal(4, page2.TotalPages);
        Assert.Equal(3, page2.Items.Count);
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 0)]
    [InlineData(-1, 10)]
    [InlineData(1, -5)]
    public async Task FindPaged_Throws_OnInvalidArgs(int page, int pageSize)
    {
        IGenericRepositoryAsync<Person> repo = new MemoryGenericRepository<Person>();

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => repo.FindPagedAsync(page, pageSize));
    }

    [Fact]
    public async Task Update_Throws_WhenIdEmpty()
    {
        IGenericRepositoryAsync<Person> repo = new MemoryGenericRepository<Person>();
        var p = CreateValidPerson(id: Guid.Empty);

        await Assert.ThrowsAsync<ArgumentException>(() => repo.UpdateAsync(p));
    }

    [Fact]
    public async Task Update_Throws_WhenMissing()
    {
        IGenericRepositoryAsync<Person> repo = new MemoryGenericRepository<Person>();
        var p = CreateValidPerson(id: Guid.NewGuid());

        await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.UpdateAsync(p));
    }

    [Fact]
    public async Task Update_ReplacesEntity_WhenExists()
    {
        IGenericRepositoryAsync<Person> repo = new MemoryGenericRepository<Person>();
        var original = CreateValidPerson();
        await repo.AddAsync(original);

        var updated = CreateValidPerson(id: original.Id);
        updated.FirstName = "Ewa";

        await repo.UpdateAsync(updated);

        var actual = await repo.FindByIdAsync(original.Id);
        Assert.Same(updated, actual);
        Assert.Equal("Ewa", actual?.FirstName);
    }

    [Fact]
    public async Task Remove_DeletesEntity_WhenExists()
    {
        IGenericRepositoryAsync<Person> repo = new MemoryGenericRepository<Person>();
        var p = CreateValidPerson();
        await repo.AddAsync(p);

        await repo.RemoveByIdAsync(p.Id);

        var actual = await repo.FindByIdAsync(p.Id);
        Assert.Null(actual);
    }

    [Fact]
    public async Task Remove_Throws_WhenMissing()
    {
        IGenericRepositoryAsync<Person> repo = new MemoryGenericRepository<Person>();

        await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.RemoveByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task Add_Throws_OnDuplicateId()
    {
        IGenericRepositoryAsync<Person> repo = new MemoryGenericRepository<Person>();
        var id = Guid.NewGuid();
        var p1 = CreateValidPerson(id);
        var p2 = CreateValidPerson(id);

        await repo.AddAsync(p1);

        await Assert.ThrowsAsync<InvalidOperationException>(() => repo.AddAsync(p2));
    }
}
