using System.Text;

namespace Mellon.Server.Services
{
    // Minimal, testable interface for calling the external weather API.
    public interface IWeatherClient
    {
        Task<string> GetWeatherAsync(string cityName);
    }

    // Typed HTTP client that knows the base address and how to attach the API key
    // as a query parameter. Reads the key from configuration (user-secrets/env).
    public class WeatherClient : IWeatherClient
    {
        private readonly HttpClient _httpClient;
        private readonly string? _apiKey;

        public WeatherClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            // Support multiple configuration keys commonly used for secrets.
            _apiKey = configuration["Weather:Key"] ?? configuration["weather-key"] ?? configuration["WeatherKey"];
        }

        public async Task<string> GetWeatherAsync(string cityName)
        {
            // Build relative URL and append API key as a query parameter if present.
            var sb = new StringBuilder();
            sb.Append("weather?q=");
            sb.Append(Uri.EscapeDataString(cityName ?? string.Empty));

            if (!string.IsNullOrEmpty(_apiKey))
            {
                sb.Append("&appid=");
                sb.Append(Uri.EscapeDataString(_apiKey));
            }

            var response = await _httpClient.GetAsync(sb.ToString());
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
