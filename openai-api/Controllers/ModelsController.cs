using Microsoft.AspNetCore.Mvc;
using openai_api.Services;
using OpenAI.GPT3.ObjectModels.ResponseModels.ModelResponseModels;

namespace openai_api.Controllers;

[ApiController]
[Route("api/openai-models")]
public class ModelsController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ModelResponse>>> GetOpenAiModels()
    {
        var openAiService = OpenAiServiceProvider.ProvideAiService();
        var modelList = await openAiService.Models.ListModel();
        return Ok(modelList.Models.OrderByDescending(m => m.CreatedTime).ToList());
    }
}