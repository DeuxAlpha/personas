using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using openai_api.Models;
using openai_api.Services;
using openai_db.Context;
using openai_db.Models;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace openai_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonaController : Controller
{
    private readonly PersonaContext _personaContext;

    public PersonaController(PersonaContext personaContext)
    {
        _personaContext = personaContext;
    }

    [HttpPost("random")]
    public async Task<ActionResult> GetRandomPersona()
    {
        var openAiService = OpenAiServiceProvider.ProvideAiService();

        var personaEdit = await openAiService.CreateEdit(new EditCreateRequest
        {
            Instruction =
                "Come up with an interesting persona, providing a name, and an interesting description, and return it in plaintext format.",
            Input = @"{Replace this with the Name}, {Replace this with an embellished description of the individual}.",
            Model = "text-davinci-edit-001"
        });

        return Ok(personaEdit);
    }

    [HttpPost("save-persona")]
    public async Task<ActionResult> SavePersona([FromBody] PersonaSaveRequest request)
    {
        var newPersona = new Persona();
        newPersona.Description = request.Persona;
        newPersona.ImgUrl = request.ImgUrl;
        var result = await _personaContext.Personas.AddAsync(newPersona);
        await _personaContext.SaveChangesAsync();
        return Created(result.Entity.Id.ToString(), result.Entity);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdatePersona([FromRoute] int id, [FromBody] PersonaSaveRequest request)
    {
        var persona = await _personaContext.Personas.FirstOrDefaultAsync(x => x.Id == id);
        if (persona == null)
        {
            return NotFound();
        }

        persona.Description = request.Persona;
        persona.ImgUrl = request.ImgUrl;
        await _personaContext.SaveChangesAsync();
        return Ok(persona);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletePersona([FromRoute] int id)
    {
        var persona = await _personaContext.Personas.FirstOrDefaultAsync(p => p.Id == id);
        if (persona == null)
        {
            return NotFound();
        }

        _personaContext.Remove(persona);
        await _personaContext.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult> GetPersonas()
    {
        var personas = await _personaContext.Personas.ToListAsync();
        return Ok(personas.Select(persona =>
            new PersonaResponse
            {
                Id = persona.Id,
                Description = persona.Description,
                ImgUrl = persona.ImgUrl
            }));
    }

    [HttpPost]
    public async Task<ActionResult> GetPersona([FromBody] PersonaRequest request)
    {
        var openAiService = OpenAiServiceProvider.ProvideAiService();
        var prompt =
            "Come up with an interesting persona, Make sure to embellish on the existing Description and Pretext, if present.";
        if (!string.IsNullOrWhiteSpace(request.Description))
            prompt += "\n" + "Description: " + request.Description;
        if (!string.IsNullOrWhiteSpace(request.Pretext))
            prompt += "\n" + "Pretext: " + request.Pretext;

        var personaEdit = await openAiService.CreateEdit(new EditCreateRequest
        {
            Instruction = prompt,
            Input =
                @"{Replace this with the Name}, {Replace this with an embellished description and pretext of the individual}.",
            Model = "text-davinci-edit-001"
        });

        return Ok(personaEdit);
    }

    [HttpPost("redefine")]
    public async Task<ActionResult> RedefinePersona([FromBody] PersonaRedefinitionRequest request)
    {
        var openAiService = OpenAiServiceProvider.ProvideAiService();
        var prompt =
            "Update and improve the existing persona and return it in plaintext.";

        var personaEdit = await openAiService.CreateEdit(new EditCreateRequest
        {
            Instruction = prompt,
            Input = request.Base,
            Model = "text-davinci-edit-001"
        });

        return Ok(personaEdit);
    }
}