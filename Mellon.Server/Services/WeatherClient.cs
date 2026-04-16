using api.Helpers;
using System.Text.Json;

namespace Mellon.Server.Services
{
    // Minimal, testable interface for calling the external weather API.
    public interface IWeatherClient
    {
        Task<string> GetWeatherAsync(string cityName);
        // Return geographic coordinates for a city name.
        Task<(double lat, double lon)> GetGeoDataAsync(string cityName);
        Task<string> GetHourlyForecastAsync(double lat, double lon);
    }

    // Typed HTTP client that knows the base address and how to attach the API key
    // as a query parameter. Reads the key from configuration (user-secrets/env) via WeatherHelper.
    public class WeatherClient : IWeatherClient
    {
        private readonly HttpClient _httpClient;
        private readonly WeatherHelper _weatherHelper;

        public WeatherClient(HttpClient httpClient, WeatherHelper weatherHelper)
        {
            _httpClient = httpClient;
            _weatherHelper = weatherHelper;
        }

        public async Task<string> GetWeatherAsync(string CityName)
        {
            // Build relative URL and append API key as a query parameter if present.
            string url = $"data/2.5/weather?q={Uri.EscapeDataString(CityName ?? string.Empty)}";
            string fullUrl = _weatherHelper.UrlConstruct(url);

            var response = await _httpClient.GetAsync(fullUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetHourlyForecastAsync(double lat, double lon)
        {
            string url = $"data/2.5/forecast?lat={lat}&lon={lon}";
            string fullUrl = _weatherHelper.UrlConstruct(url);

            var response = await _httpClient.GetAsync(fullUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<(double lat, double lon)> GetGeoDataAsync(string CityName)
        {
            string url = $"geo/1.0/direct?q={Uri.EscapeDataString(CityName ?? string.Empty)}";
            string fullUrl = _weatherHelper.UrlConstruct(url);

            var response = await _httpClient.GetAsync(fullUrl);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<List<GeoResult>>(json);
            if (data == null || data.Count == 0)
            {
                throw new InvalidOperationException("No geocoding results returned for city.");
            }

            return (data[0].Lat, data[0].Lon);
        }
    }
}
