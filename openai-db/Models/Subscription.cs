namespace openai_db.Models;

public class Subscription
{
    public int Id { get; set; }
    public string SubscriptionEndDate { get; set; } // If this exists, and is in the future, then the user is subscribed.
    public string ApiKey { get; set; } // Encrypted API Key. Decrypt with application's private key.
}