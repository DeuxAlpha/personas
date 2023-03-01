using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace openai_db.Context;

public class DesignTimePersonaContextFactory : IDesignTimeDbContextFactory<PersonaContext>
{
    public PersonaContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        
        var optionsBuilder = new DbContextOptionsBuilder<PersonaContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("PersonaConnection"));
        return new PersonaContext(optionsBuilder.Options);
    }
}