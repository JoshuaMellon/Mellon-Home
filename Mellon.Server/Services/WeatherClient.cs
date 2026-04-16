using api.Helpers;

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
        private readonly WeatherHelper _weatherHelper;

        public WeatherClient(HttpClient httpClient, WeatherHelper weatherHelper)
        {
            _httpClient = httpClient;
            _weatherHelper = weatherHelper;
        }

        public async Task<string> GetWeatherAsync(string cityName)
        {
            // Build relative URL and append API key as a query parameter if present.
            string url = $"weather?q={cityName}";
            string fullUrl = _weatherHelper.UrlConstruct(url);

            var response = await _httpClient.GetAsync(fullUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
