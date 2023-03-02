namespace openai_api.Models;

public class ChatResponse
{
    public string Id { get; set; }
    public string Object { get; set; }
    public string Created { get; set; }
    public IEnumerable<ChatChoice> Choices { get; set; }
    public ChatUsage Usage { get; set; }
}