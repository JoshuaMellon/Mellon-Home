using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Read API key from configuration (user-secrets, environment variable, or appsettings).
var weatherApiKey = builder.Configuration["weather-key"];


builder.Services.AddHttpClient("weather", client =>
{
    client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    if (!string.IsNullOrEmpty(weatherApiKey))
    {
        client.DefaultRequestHeaders.Add("x-api-key", weatherApiKey);
    }
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
