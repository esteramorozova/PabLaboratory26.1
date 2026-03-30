using AppCore.Dto;
using AppCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("/api/contacts")]
public class ContactsController(IPersonService service): ControllerBase
{
    [HttpGet]
    public  async Task<IActionResult> GetAllPersons(int page = 1, int size = 10)
    {
        return Ok(await service.FindAllPeoplePaged(page, size));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPerson(Guid id)
    {
        var dto = await service.GetById(id);
        
        if (dto == null)
        {
            return NotFound(); 
        }
        
        return Ok(dto);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreatePersonDto dto)
    {
        var result = await service.AddPerson(dto);
        return CreatedAtAction(nameof(GetPerson), new { id = result.Id }, result);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePersonDto dto)
    {
        
        var person = await service.GetById(id);
    
        if (person is null)
        {
            return NotFound(new { Message = $"Person with ID {id} not found." });
        }

        var updatedPerson = await service.UpdateAsync(id, dto);
        return Ok(updatedPerson);
    }
    
    [HttpPost("{contactId:guid}/notes")]
    [ProducesResponseType(typeof(NoteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddNote(
        [FromRoute] Guid contactId,
        [FromBody] CreateNoteDto dto)
    {
        var note = await service.AddNoteToPerson(contactId, dto);
        return CreatedAtAction(
            nameof(GetNotes),
            new { contactId },
            note);
    }

    [HttpGet("{contactId:guid}/notes")]
    [ProducesResponseType(typeof(IEnumerable<NoteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNotes([FromRoute] Guid contactId)
    {
        var person = await service.GetPerson(contactId);
        return Ok(person.Notes);
    }

    [HttpDelete("{contactId:guid}/notes/{noteId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveNote([FromRoute] Guid contactId, [FromRoute] Guid noteId)
    {
        await service.RemoveNoteFromPerson(contactId, noteId);
        return NoContent();
    }
}

