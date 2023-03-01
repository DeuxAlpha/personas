using Microsoft.AspNetCore.Mvc;
using openai_api.Services;

namespace openai_api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ManagementController : Controller
{
    [HttpGet("models")]
    public async Task<ActionResult> GetModels()
    {
        try
        {
            var openAiService = OpenAiServiceProvider.ProvideAiService();

            var models = await openAiService.ListModel();

            return Ok(models);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}