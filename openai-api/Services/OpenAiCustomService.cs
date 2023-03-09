using Microsoft.Extensions.Options;
using openai_api.Models;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using RestSharp;

namespace openai_api.Services;

public class OpenAiCustomService : OpenAIService
{
    private readonly RestClient _chatGptClient;
    private readonly string _apiKey;

    public OpenAiCustomService(HttpClient httpClient, IOptions<OpenAiOptions> settings) : base(httpClient, settings)
    {
        _apiKey = httpClient.DefaultRequestHeaders.Authorization.Parameter;
        _chatGptClient = GetChatGptClient();
    }

    public OpenAiCustomService(OpenAiOptions settings, HttpClient? httpClient = null) : base(settings, httpClient)
    {
        _apiKey = settings.ApiKey;
        _chatGptClient = GetChatGptClient();
    }
    

    private RestClient GetChatGptClient()
    {
        return new RestClient("https://api.openai.com/");
    }

    public async Task<ChatResponse> CreateChatResponse(ChatRequest request)
    {
        var apiRequest = new RestRequest("v1/chat/completions", Method.Post);
        apiRequest.AddHeader("content-type", "application/json");
        apiRequest.AddHeader("Authorization", $"Bearer {_apiKey}");
        apiRequest.AddBody(request);
        var response = await _chatGptClient.ExecuteAsync<ChatResponse>(apiRequest);
        if (response.IsSuccessful && response.Data != null)
        {
            return response.Data;
        }
        throw new Exception("Error creating chat response");
    }
}