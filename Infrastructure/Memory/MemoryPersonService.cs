using AppCore.Dto;
using AppCore.Exceptions;
using AppCore.Interfaces;
using AppCore.Models;

namespace Infrastructure.Memory;

public class MemoryPersonService(IContactUnitOfWork unitOfWork) : IPersonService
{
    public async Task<Note> AddNoteToPerson(Guid personId, CreateNoteDto noteDto)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(personId);

        if (person is null) 
            throw new ContactNotFoundException($"Person with id={personId} not found!");
        
        person.Notes ??= new List<Note>();
        
        var note = new Note
        {
            Id = Guid.NewGuid(),
            Content = noteDto.Content,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System" 
        };
        
        person.Notes.Add(note);
        
        await unitOfWork.Persons.UpdateAsync(person);
        await unitOfWork.SaveChangesAsync();
        
        return note;
    }

    public async Task RemoveNoteFromPerson(Guid personId, Guid noteId)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(personId);
        if (person is null)
            throw new ContactNotFoundException($"Person with id={personId} not found!");

        person.Notes ??= new List<Note>();

        var note = person.Notes.FirstOrDefault(n => n.Id == noteId);
        if (note is null)
            throw new Exception($"Note with id={noteId} not found for person with id={personId}!");

        person.Notes.Remove(note);
        await unitOfWork.Persons.UpdateAsync(person);
        await unitOfWork.SaveChangesAsync();
    }
    
    public async Task<PersonDto> GetPerson(Guid personId)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(personId);
        if (person is null) 
            throw new Exception($"Person with id '{personId}' was not found.");

        return PersonDto.FromEntity(person);
    }
    
    public async Task<PersonDto> AddPerson(CreatePersonDto personDto)
    {
        var entity = PersonDto.ToEntity(personDto);

        if (personDto.EmployerId is Guid employerId)
        {
            var employer = await unitOfWork.Companies.FindByIdAsync(employerId);
            if (employer is null) throw new KeyNotFoundException($"Company with id '{employerId}' was not found.");
            entity.Employer = employer;
        }

        entity = await unitOfWork.Persons.AddAsync(entity);
        await unitOfWork.SaveChangesAsync();
        return PersonDto.FromEntity(entity);
    }

    public async Task<Person> UpdatePerson(UpdatePersonDto personDto)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(personDto.Id);
        if (person is null) throw new KeyNotFoundException($"Person with id '{personDto.Id}' was not found.");

        if (personDto.FirstName is not null) person.FirstName = personDto.FirstName;
        if (personDto.LastName is not null) person.LastName = personDto.LastName;
        if (personDto.Email is not null) person.Email = personDto.Email;
        if (personDto.Phone is not null) person.Phone = personDto.Phone;
        if (personDto.Position is not null) person.Position = personDto.Position;
        if (personDto.BirthDate is not null) person.BirthDate = personDto.BirthDate;
        if (personDto.Gender is not null) person.Gender = personDto.Gender.Value;
        if (personDto.Status is not null) person.Status = personDto.Status.Value;

        if (personDto.Address is not null)
        {
            person.Address.Street = personDto.Address.Street;
            person.Address.City = personDto.Address.City;
            person.Address.PostalCode = personDto.Address.PostalCode;
            person.Address.Country = personDto.Address.Country;
            person.Address.Type = personDto.Address.Type;
        }

        if (personDto.EmployerId is not null)
        {
            if (personDto.EmployerId.Value == Guid.Empty)
            {
                person.Employer = null;
            }
            else
            {
                var employer = await unitOfWork.Companies.FindByIdAsync(personDto.EmployerId.Value);
                if (employer is null)
                {
                    throw new KeyNotFoundException($"Company with id '{personDto.EmployerId.Value}' was not found.");
                }

                person.Employer = employer;
            }
        }

        person.UpdatedAt = DateTime.UtcNow;
        var updated = await unitOfWork.Persons.UpdateAsync(person);
        await unitOfWork.SaveChangesAsync();
        return updated;
    }

    public async Task<PersonDto?> GetById(Guid id)
    {
        var person = await unitOfWork.Persons.FindByIdAsync(id);
        return person is null ? null : PersonDto.FromEntity(person);
    }

    public async Task<PagedResult<PersonDto>> FindAllPeoplePaged(int page, int size)
    {
        var people = await unitOfWork.Persons.FindPagedAsync(page, size);
        var items = people.Items.Select(PersonDto.FromEntity).ToList();
        return new PagedResult<PersonDto>(items, people.TotalCount, people.Page, people.PageSize);
    }

    public Task<PersonDto?> FindByIdAsync(Guid id) => GetById(id);

    public Task<PersonDto> CreateAsync(CreatePersonDto dto) => AddPerson(dto);

    public async Task<PersonDto> UpdateAsync(Guid id, UpdatePersonDto dto)
    {
        var updated = await UpdatePerson(dto with { Id = id });
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

