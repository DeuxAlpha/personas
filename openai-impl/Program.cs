
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;

var openAiService = new OpenAIService(new OpenAiOptions
{
    ApiKey = "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"
});
openAiService.SetDefaultEngineId("text-davinci-003");

var result = await openAiService.CreateEdit(new EditCreateRequest
{
    Instruction = "Make this sound more appealing to a wider audience.",
    Input = "The product is a new type of computer chip that is designed to be used in a new type of computer.",
    Model = "text-davinci-edit-001"
});

Console.WriteLine(result.Choices.FirstOrDefault());

result = await openAiService.CreateEdit(new EditCreateRequest
{
    Instruction = "Don't make this sound more appealing to a wider audience.",
    Input = "You're a maggot in my eyes.",
    Model = "text-davinci-edit-001"
});

Console.WriteLine(result.Choices.FirstOrDefault());

var prompt = "The following is a conversation with an AI assistant. The assistant is helpful, creative, clever, and very friendly.\n\nHuman: Hello, who are you?\nAI: I am an AI created by OpenAI. How can I help you today?\nHuman: ";

var completionResult = await openAiService.Completions.CreateCompletion(new CompletionCreateRequest
{
    Prompt = $"{prompt} Once upon a time",
    MaxTokens = 50,
    
});

if (completionResult.Successful)
{
    foreach (var choice in completionResult.Choices)
    {
        Console.WriteLine(choice);
    }
}

else
{
    if (completionResult.Error == null)
    {
        throw new Exception("Unknown Error");
    }
    Console.WriteLine($"{completionResult.Error.Code}: {completionResult.Error.Message}");
}