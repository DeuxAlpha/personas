namespace openai_api.Models;

public class ChatChoice
{
    public int Index { get; set; }
    public IEnumerable<ChatMessage> Messages { get; set; }
    public string Finish_Reason { get; set; }
}