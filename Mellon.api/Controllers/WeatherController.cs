using Mellon.api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mellon.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        // Use the typed IWeatherClient to keep controller logic thin and testable.
        private readonly IWeatherClient _weatherClient;

        public WeatherController(IWeatherClient weatherClient)
        {
            _weatherClient = weatherClient;
        }

        // Delegates to the typed client which appends the API key as a query parameter.
        [HttpGet("current-forecast")]
        public async Task<IActionResult> GetWeatherByCity(string cityName)
        {
            var payload = await _weatherClient.GetWeatherAsync(cityName);
            return Content(payload, "application/json");
        }

        [HttpGet("hourly-forecast-four-days")]
        public async Task<IActionResult> HourlyForecastFourDays(string cityName)
        {
            var (lat, lon) = await _weatherClient.GetGeoDataAsync(cityName);

            var payload = await _weatherClient.GetHourlyForecastAsync(lat, lon);

            return Content(payload, "application/json");
        }

        [HttpGet("geo-data")]
        public async Task<IActionResult> GetGeoData(string cityName)
        {
            var (lat, lon) = await _weatherClient.GetGeoDataAsync(cityName);
            // Return coordinates as JSON. Controller keeps responsibility for HTTP shaping.
            return Ok(new { lat, lon });
        }
    }
}