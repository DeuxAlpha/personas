using Microsoft.AspNetCore.Mvc;
using openai_api.Models;
using openai_api.Services;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace openai_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompletionsController : Controller
{
    [HttpPost]
    public async Task<ActionResult> CreateCompletion([FromBody] CompletionRequest request)
    {
        var openAiService = OpenAiServiceProvider.ProvideAiService(request.EngineId);
        var prompt = string.IsNullOrWhiteSpace(request.Prefix) ? 
            $"This is you: {request.Persona} Respond to this in first person: {request.Prompt}" : 
            $"This is you: {request.Persona} Respond to this in first person: {request.Prefix} {request.Prompt}";
        if (request.EngineId == "code-davinci-002")
        {
            prompt =
                // $"Write code to fulfill this request:\r\n\"{request.Prompt}\"\r\nWrap it in <code> brackets. Use your preferred programming language and style.";
                $"Fulfill this request: \"{request.Prompt}\"";
        }

        var completion = await openAiService.Completions.CreateCompletion(new CompletionCreateRequest
        {
            Temperature = request.Temperature,
            MaxTokens = request.MaxTokens,
            // PromptAsList = new List<string>
            // {
                // $"This is your persona: {request.Persona}",
                // $"Respond: {request.Prompt}"
            // },
            Prompt = prompt,
            Echo = request.Echo,
            FrequencyPenalty = request.FrequencyPenalty,
            PresencePenalty = request.PresencePenalty,
            N = request.N,
        });
        
        return Ok(completion);
    }
    
    [HttpPost("prompt")]
    public async Task<ActionResult> GetPrompt([FromBody] PromptRequest request)
    {
        var openAiService = OpenAiServiceProvider.ProvideAiService(request.Model);
        var prompt = string.IsNullOrWhiteSpace(request.Prefix) ? 
            $"Generate an interesting prompt to generate an outlandish response from the AI. Your persona is the following: {request.Persona}" :
            $"Generate an interesting prompt to generate an outlandish response from the AI. Your persona is the following: {request.Persona} - The story so far: {request.Prefix}";
        
        var completion = await openAiService.Completions.CreateCompletion(new CompletionCreateRequest
        {
            Prompt = prompt,
            MaxTokens = 100
        });
        
        return Ok(completion);
    }
}