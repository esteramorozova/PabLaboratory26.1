using AppCore.Dto;
using AppCore.Interfaces;
using AppCore.Models;

namespace Infrastructure.Memory;

public class MemoryPersonService(IContactUnitOfWork unitOfWork) : IPersonService
{
    public async Task<PagedResult<PersonDto>> FindAllPeoplePaged(int page, int size)
    {
        var people = await unitOfWork.Persons.FindPagedAsync(page, size);
        var items = people.Items.Select(PersonDto.FromEntity).ToList();
        return new PagedResult<PersonDto>(items, people.TotalCount, people.Page, people.PageSize);
    }

    public async Task<PersonDto?> FindByIdAsync(Guid id)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(id);
        return person is null ? null : PersonDto.FromEntity(person);
    }

    public async Task<PersonDto> CreateAsync(CreatePersonDto dto)
    {
        var entity = PersonDto.ToEntity(dto);

        if (dto.EmployerId is Guid employerId)
        {
            var employer = await unitOfWork.Companies.FindByIdAsync(employerId);
            if (employer is null) throw new KeyNotFoundException($"Company with id '{employerId}' was not found.");
            entity.Employer = employer;
        }

        var created = await unitOfWork.Persons.AddAsync(entity);
        await unitOfWork.SaveChangesAsync();
        return PersonDto.FromEntity(created);
    }

    public async Task<PersonDto> UpdateAsync(Guid id, UpdatePersonDto dto)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(id);
        if (person is null) throw new KeyNotFoundException($"Person with id '{id}' was not found.");

        if (dto.FirstName is not null) person.FirstName = dto.FirstName;
        if (dto.LastName is not null) person.LastName = dto.LastName;
        if (dto.Email is not null) person.Email = dto.Email;
        if (dto.Phone is not null) person.Phone = dto.Phone;
        if (dto.Position is not null) person.Position = dto.Position;
        if (dto.BirthDate is not null) person.BirthDate = dto.BirthDate;
        if (dto.Gender is not null) person.Gender = dto.Gender.Value;
        if (dto.Status is not null) person.Status = dto.Status.Value;

        if (dto.Address is not null)
        {
            person.Address.Street = dto.Address.Street;
            person.Address.City = dto.Address.City;
            person.Address.PostalCode = dto.Address.PostalCode;
            person.Address.Country = dto.Address.Country;
            person.Address.Type = dto.Address.Type;
        }

        if (dto.EmployerId is not null)
        {
            if (dto.EmployerId.Value == Guid.Empty)
            {
                person.Employer = null;
            }
            else
            {
                var employer = await unitOfWork.Companies.FindByIdAsync(dto.EmployerId.Value);
                if (employer is null)
                {
                    throw new KeyNotFoundException($"Company with id '{dto.EmployerId.Value}' was not found.");
                }

                person.Employer = employer;
            }
        }

        person.UpdatedAt = DateTime.UtcNow;

        var updated = await unitOfWork.Persons.UpdateAsync(person);
        await unitOfWork.SaveChangesAsync();
        return PersonDto.FromEntity(updated);
    }

    public async Task RemoveByIdAsync(Guid id)
    {
        await unitOfWork.Persons.RemoveByIdAsync(id);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<IAsyncEnumerable<PersonDto>> FindPeopleFromCompany(Guid companyId)
    {
        var people = await unitOfWork.Persons.FindByEmployerAsync(companyId);
        return MapAsync(people);
    }

    public async Task<IAsyncEnumerable<PersonDto>> FindPeopleFromOrganization(Guid organizationId)
    {
        var people = await unitOfWork.Persons.FindByOrganizationAsync(organizationId);
        return MapAsync(people);
    }

    public async Task AddNoteAsync(Guid personId, string content, string createdBy)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(personId);
        if (person is null) throw new KeyNotFoundException($"Person with id '{personId}' was not found.");

        person.Notes.Add(new Note
        {
            Id = Guid.NewGuid(),
            Content = content,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow
        });

        await unitOfWork.Persons.UpdateAsync(person);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task AddTagAsync(Guid personId, string tag)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(personId);
        if (person is null) throw new KeyNotFoundException($"Person with id '{personId}' was not found.");

        if (person.Tags.Any(t => string.Equals(t.Name, tag, StringComparison.OrdinalIgnoreCase)))
        {
            return;
        }

        person.Tags.Add(new Tag
        {
            Id = Guid.NewGuid(),
            Name = tag,
            Color = "#000000"
        });

        await unitOfWork.Persons.UpdateAsync(person);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveTagAsync(Guid personId, string tag)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(personId);
        if (person is null) throw new KeyNotFoundException($"Person with id '{personId}' was not found.");

        person.Tags.RemoveAll(t => string.Equals(t.Name, tag, StringComparison.OrdinalIgnoreCase));

        await unitOfWork.Persons.UpdateAsync(person);
        await unitOfWork.SaveChangesAsync();
    }

    private static async IAsyncEnumerable<PersonDto> MapAsync(IEnumerable<Person> people)
    {
        foreach (var person in people)
        {
            yield return PersonDto.FromEntity(person);
            await Task.Yield();
        }
    }
}

