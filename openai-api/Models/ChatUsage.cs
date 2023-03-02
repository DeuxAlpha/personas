namespace openai_api.Models;

public class ChatUsage
{
    public int Prompt_Tokens { get; set; }
    public int Completion_Tokens { get; set; }
    public int Total_Tokens { get; set; }
}