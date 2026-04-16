using api.Helpers;
using Mellon.Server.Services;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Register a typed HTTP client for the weather API. The typed client centralizes
// request logic (including attaching the API key as a query parameter) so
// controllers remain thin. The API key is read from configuration (user-secrets
// or environment variables) inside the typed client implementation.
builder.Services.AddHttpClient<IWeatherClient, WeatherClient>(client =>
{
    client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});
builder.Services.AddSingleton<WeatherHelper>();

// Register services to generate OpenAPI/Swagger using Swashbuckle.
// - AddEndpointsApiExplorer discovers endpoints for controllers.
// - AddSwaggerGen generates the OpenAPI document used by the Swagger UI.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
// Expose interactive OpenAPI docs in Development. MapOpenApi wires up the endpoints/UI provided
// by Microsoft.AspNetCore.OpenApi and Microsoft.AspNetCore.Grpc.Swagger packages.
if (app.Environment.IsDevelopment())
{
    // Enable the middleware to serve generated Swagger as JSON and the Swagger UI.
    // This provides the interactive /swagger page backed by Swashbuckle.
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Mellon API V1");
        options.RoutePrefix = "swagger"; // Serve UI at /swagger
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
