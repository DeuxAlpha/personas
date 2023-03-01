using Microsoft.AspNetCore.Mvc;
using openai_api.Models;
using openai_api.Services;
using OpenAI.GPT3.ObjectModels.RequestModels;
using Serilog;
using Serilog.Core;

namespace openai_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : Controller
{
    readonly IDiagnosticContext _diagnosticContext;

    public ImagesController(IDiagnosticContext diagnosticContext)
    {
        _diagnosticContext = diagnosticContext ??
                             throw new ArgumentNullException(nameof(diagnosticContext));
    }

    [HttpPost]
    public async Task<ActionResult> GetImage([FromBody] ImageRequest request)
    {
        try
        {
            var openAiService = OpenAiServiceProvider.ProvideAiService();
            const int maxLength = 200;
            var truncated = request.Prompt.Length > maxLength ? request.Prompt[..maxLength] : request.Prompt;
            _diagnosticContext.Set("Truncated Value", truncated);
            _diagnosticContext.Set("Full Value", request.Prompt);
            
            var result = await openAiService.CreateImage(new ImageCreateRequest
            {
                Prompt = truncated
            });

            if (!result.Successful)
            {
                if (result.Error != null)
                    return BadRequest(result.Error.Message);
                else
                    return BadRequest("Unhandled error.");
            }

            var imgUrl = result.Results.FirstOrDefault()?.Url;
            return Ok(imgUrl);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}