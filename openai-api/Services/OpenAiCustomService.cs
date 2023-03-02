using Microsoft.Extensions.Options;
using openai_api.Models;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using RestSharp;

namespace openai_api.Services;

public class OpenAiCustomService : OpenAIService
{
    private readonly RestClient _chatGptClient;

    public OpenAiCustomService(HttpClient httpClient, IOptions<OpenAiOptions> settings) : base(httpClient, settings)
    {
    }

    public OpenAiCustomService(OpenAiOptions settings, HttpClient? httpClient = null) : base(settings, httpClient)
    {
    }
    
    

    private RestClient GetChatGptClient()
    {
        return new RestClient("https://api.openai.com/");
    }

    public ChatResponse CreateChatResponse(ChatRequest request)
    {
        var apiRequest = new RestRequest("v1/chat/completions", Method.Post);
    }
}