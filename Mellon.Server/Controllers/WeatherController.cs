using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController(IHttpClientFactory httpClientFactory) : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        [HttpGet]
        public async Task<IActionResult> GetWeather(string cityName)
        {
            var client = _httpClientFactory.CreateClient("weather");

            var response = await client.GetAsync($"weather?city={cityName}");


            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to retrieve weather data");
            }

            var payload = await response.Content.ReadAsStringAsync();

            return Content(payload, "application/json");
        }
    }
}
