using AppCore.Dto;
using AppCore.Models;

namespace AppCore.Interfaces;

public interface IPersonService
{
    Task<PagedResult<PersonDto>> FindAllPeoplePaged(int page, int size);
    Task<PersonDto> AddPerson(CreatePersonDto personDto);
    Task<Person> UpdatePerson(UpdatePersonDto personDto);
    Task<PersonDto?> GetById(Guid id);
    Task<PersonDto?> FindByIdAsync(Guid id);

    Task<PersonDto> CreateAsync(CreatePersonDto dto);
    Task<PersonDto> UpdateAsync(Guid id, UpdatePersonDto dto);
    Task RemoveByIdAsync(Guid id);

    Task<IAsyncEnumerable<PersonDto>> FindPeopleFromCompany(Guid companyId);
    Task<IAsyncEnumerable<PersonDto>> FindPeopleFromOrganization(Guid organizationId);

    Task AddNoteAsync(Guid personId, string content, string createdBy);
    Task AddTagAsync(Guid personId, string tag);
    Task RemoveTagAsync(Guid personId, string tag);
    
    Task<Note> AddNoteToPerson(Guid personId, CreateNoteDto noteDto);
    Task RemoveNoteFromPerson(Guid personId, Guid noteId);
    Task<PersonDto> GetPerson(Guid personId);
}

