using Mellon.Server.Services;
using Microsoft.AspNetCore.Mvc;

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

        // GET api/weather?cityName=London
        // Delegates to the typed client which appends the API key as a query parameter.
        [HttpGet]
        public async Task<IActionResult> GetWeather(string cityName)
        {
            var payload = await _weatherClient.GetWeatherAsync(cityName);
            return Content(payload, "application/json");
        }
    }
}
