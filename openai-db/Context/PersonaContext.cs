using Microsoft.EntityFrameworkCore;
using openai_db.Models;

namespace openai_db.Context;

public class PersonaContext : DbContext
{
     public DbSet<Persona> Personas { get; set; }
     
     public PersonaContext(DbContextOptions<PersonaContext> options) : base(options) {}
}