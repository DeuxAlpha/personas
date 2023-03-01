using OpenAI.GPT3;
using OpenAI.GPT3.Managers;

namespace openai_api.Services;

public class OpenAiServiceProvider
{
    public static OpenAIService ProvideAiService(string engineId = "text-davinci-003")
    {
        var openAiService = new OpenAIService(new OpenAiOptions
        {
            ApiKey = "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"
        });
        openAiService.SetDefaultEngineId(engineId);

        return openAiService;
    }
}