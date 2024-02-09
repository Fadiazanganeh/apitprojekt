using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Lägg till Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Använd Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Definiera tillåtna ord
var allowedWords = new[] { "hamburger", "tacos", "fries", "icecream", "candy", "popcorn" };

// Kryptera endpoint
app.MapPost("/encrypt", (string text) => {
    if (string.IsNullOrEmpty(text) || !allowedWords.Contains(text.ToLower()))
    {
        return Results.BadRequest("Text is either empty or not allowed.");
    }

    var encryptedText = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
    return Results.Ok(new { encryptedText });
});

// Avkryptera endpoint
app.MapPost("/decrypt", (string encryptedText) => {
    try
    {
        var decryptedText = Encoding.UTF8.GetString(Convert.FromBase64String(encryptedText));
        if (!allowedWords.Contains(decryptedText.ToLower()))
        {
            return Results.BadRequest("Decrypted text is not allowed.");
        }
        return Results.Ok(new { decryptedText });
    }
    catch
    {
        return Results.BadRequest("Invalid encrypted text format.");
    }
});

app.Run();