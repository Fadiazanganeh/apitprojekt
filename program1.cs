using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Lägg till Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();

// Använd Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

// Definiera tillåtna ord
var allowedWords = new[] { "hamburger", "tacos", "fries", "icecream", "candy", "popcorn" };

// Krypterar endpoint
app.MapGet("/encrypt", (EncryptionService encryptionService, string plaintext) =>
{
    return Results.Ok(new { encryptedText = encryptionService.Encrypt(plaintext) });
});

// Avkrypterar endpoint
app.MapGet("/decrypt", (EncryptionService encryptionService, string encryptedText) =>
{
    return Results.Ok(new { decryptedText = encryptionService.Decrypt(encryptedText) });
});

    var encryptedText = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
    context.Response.StatusCode = StatusCodes.Status200OK;
    await context.Response.WriteAsJsonAsync(new { encryptedText });
});

// Avkryptera endpoint
app.MapGet("/decrypt", async (HttpContext context) =>
{
    var encryptedText = await new StreamReader(context.Request.Body).ReadToEndAsync();
    try
    {
        var decryptedText = Encoding.UTF8.GetString(Convert.FromBase64String(encryptedText));
        if (!allowedWords.Contains(decryptedText.ToLower()))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Decrypted text is not allowed.");
            return;
        }
        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsJsonAsync(new { decryptedText });
    }
    catch
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("Invalid encrypted text format.");
    }
});

app.Run();
