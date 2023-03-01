namespace openai_api.Models;

public class PromptRequest
{
    public string Persona { get; set; }
    public string Prefix { get; set; }
    public string Model { get; set; }
}