using Config2;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;

namespace openai_api.Services;

public class OpenAiServiceProvider
{
    private static string ApiKey => StaticConfig.GetValue<string>("OpenAi:ApiKey");
    
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