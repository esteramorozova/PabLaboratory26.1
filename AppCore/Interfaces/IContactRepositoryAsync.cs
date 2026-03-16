using AppCore.Dto;
using AppCore.Models;

namespace AppCore.Interfaces;

public interface IContactRepositoryAsync : IGenericRepositoryAsync<Contact>
{
    Task<PagedResult<Contact>> SearchAsync(ContactSearchDto search);
    Task<IEnumerable<Contact>> FindByTagAsync(string tag);

    Task<Note> AddNoteAsync(Guid contactId, Note note);
    Task<IEnumerable<Note>> GetNotesAsync(Guid contactId);

    Task AddTagAsync(Guid contactId, string tag);
    Task RemoveTagAsync(Guid contactId, string tag);
}

