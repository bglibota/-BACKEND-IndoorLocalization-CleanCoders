using IndoorLocalization_API.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<IndoorLocalizationContext>();

// Dodajte CORS politiku
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()  // Dozvoljava zahtjeve sa svih domena
              .AllowAnyMethod()  // Dozvoljava sve HTTP metode (GET, POST, PUT, DELETE...)
              .AllowAnyHeader(); // Dozvoljava sve zaglavlja
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddUserSecrets<Program>();



var connectionString = builder.Configuration.GetConnectionString(builder.Environment.IsProduction() ? "serverDatabase" : "localDatabase");
//var connectionString = builder.Configuration.GetConnectionString("serverDatabase");

builder.Services.AddDbContext<IndoorLocalizationContext>(options =>
      options.UseNpgsql(connectionString));
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Dodajte CORS middleware
app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
