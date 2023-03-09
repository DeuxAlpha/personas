using Microsoft.AspNetCore.Mvc;
using openai_api.Models;
using openai_api.Services;

namespace openai_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : Controller
{
    [HttpPost]
    public async Task<ActionResult> CreateChat([FromBody] ChatRequest request)
    {
        try
        {
            var openAiService = OpenAiServiceProvider.ProvideCustomAiService();
            var chatResponse = await openAiService.CreateChatResponse(request);
            return Ok(chatResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}