namespace openai_db.Models;

public class Conversation
{
    public int Id { get; set; }
    public string Owner { get; set; } /** Either 'User' or 'Bot' */
    public string UserId { get; set; }
    public string PersonaId { get; set; }
    public DateTime UtcTimestamp { get; set; }
    public string Message { get; set; }
}