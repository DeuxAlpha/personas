namespace openai_db.Models;

public class Persona
{
    public int Id { get; set; }
    // public string OwnerId { get; set; }
    // public bool Shared { get; set; }
    public string Description { get; set; }
    public string? ImgUrl { get; set; }
}