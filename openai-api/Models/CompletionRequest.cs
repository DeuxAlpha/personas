namespace openai_api.Models;

public class CompletionRequest
{
    // Gets injected ahead of the prompt.
    public string Persona { get; set; }
    public string Prompt { get; set; }
    public string Prefix { get; set; }
    public int MaxTokens { get; set; } = 500;
    public float Temperature { get; set; } = 0.2f;
    public bool Echo { get; set; } = false;
    public float FrequencyPenalty { get; set; } = 0.0f;
    public float PresencePenalty { get; set; } = 0.0f;

    public string EngineId { get; set; } = "text-davinci-003";
    public int N { get; set; } = 1;
}