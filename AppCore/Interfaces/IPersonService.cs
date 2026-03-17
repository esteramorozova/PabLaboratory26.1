using AppCore.Dto;

namespace AppCore.Interfaces;

public interface IPersonService
{
    Task<PagedResult<PersonDto>> FindAllPeoplePaged(int page, int size);
    Task<PersonDto?> FindByIdAsync(Guid id);

    Task<PersonDto> CreateAsync(CreatePersonDto dto);
    Task<PersonDto> UpdateAsync(Guid id, UpdatePersonDto dto);
    Task RemoveByIdAsync(Guid id);

    Task<IAsyncEnumerable<PersonDto>> FindPeopleFromCompany(Guid companyId);
    Task<IAsyncEnumerable<PersonDto>> FindPeopleFromOrganization(Guid organizationId);

    Task AddNoteAsync(Guid personId, string content, string createdBy);
    Task AddTagAsync(Guid personId, string tag);
    Task RemoveTagAsync(Guid personId, string tag);
}

