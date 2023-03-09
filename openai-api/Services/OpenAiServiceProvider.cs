using OpenAI.GPT3;
using OpenAI.GPT3.Managers;

namespace openai_api.Services;

public class OpenAiServiceProvider
{
    private const string ApiKey = "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";
    
    public static OpenAIService ProvideAiService()
    {
        var openAiService = new OpenAIService(new OpenAiOptions
        {
            ApiKey = ApiKey
        });

        return openAiService;
    }

    public static OpenAiCustomService ProvideCustomAiService()
    {
        var customAiService = new OpenAiCustomService(new OpenAiOptions
        {
            ApiKey = ApiKey
        });

        return customAiService;
    }
}