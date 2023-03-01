using Microsoft.EntityFrameworkCore;
using openai_db.Context;
using Serilog;
using Serilog.Core;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
// Add services to the container.
builder.Services.AddLogging(configure => configure.AddSerilog());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PersonaConnection");
if (string.IsNullOrWhiteSpace(connectionString)) throw new NullReferenceException(nameof(connectionString));
builder.Services.AddDbContext<PersonaContext>(options => options.UseSqlServer(connectionString));

// Add cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policyBuilder =>
        {
            policyBuilder.AllowAnyOrigin();
            policyBuilder.AllowAnyMethod();
            policyBuilder.AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Use cors
app.UseCors("AllowAllOrigins");

app.MapControllers();

app.Run();

app.UseSerilogRequestLogging();