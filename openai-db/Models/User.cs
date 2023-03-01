namespace openai_db.Models;

public class User
{
    public int Id { get; set; }
    public ICollection<Persona> Personas { get; set; }
    public ICollection<Conversation> Conversations { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public int Attempts { get; set; }
    public int SubscriptionId { get; set; }
    public Subscription Subscription { get; set; }
}