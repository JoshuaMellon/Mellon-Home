using Mellon.api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace api.Controllers
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
            try
            {
                var payload = await _weatherClient.GetWeatherAsync(cityName);
                return Content(payload, "application/json");
            }
            catch (HttpRequestException)
            {
                // Translate HTTP errors from the external API (e.g., 404 for unknown city)
                // into a 400 Bad Request to match test expectations.
                return BadRequest();
            }
        }

        [HttpGet("hourly-forecast-four-days")]
        public async Task<IActionResult> HourlyForecastFourDays(string cityName)
        {
            try
            {
                var (lat, lon) = await _weatherClient.GetGeoDataAsync(cityName);

                var payload = await _weatherClient.GetHourlyForecastAsync(lat, lon);

                return Content(payload, "application/json");
            }
            catch (HttpRequestException)
            {
                return BadRequest();
            }
            catch (InvalidOperationException)
            {
                // No geocoding results - treat as bad request for invalid city.
                return BadRequest();
            }
        }

        [HttpGet("geo-data")]
        public async Task<IActionResult> GetGeoData(string cityName)
        {
            try
            {
                var (lat, lon) = await _weatherClient.GetGeoDataAsync(cityName);
                // Return coordinates as JSON. Controller keeps responsibility for HTTP shaping.
                return Ok(new { lat, lon });
            }
            catch (HttpRequestException)
            {
                return BadRequest();
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
        }
    }
}