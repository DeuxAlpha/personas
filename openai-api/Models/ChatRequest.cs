namespace openai_api.Models;

public class ChatRequest
{
    public string Model { get; set; }
    public IEnumerable<ChatMessage> Messages { get; set; }
}