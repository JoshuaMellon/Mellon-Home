using System.Text;

namespace api.Helpers
{
    public class WeatherHelper
    {
        private readonly string? _apiKey;

        public WeatherHelper(IConfiguration configuration)
        {
            // Support multiple configuration keys commonly used for secrets.
            _apiKey = configuration["Weather:Key"] ?? configuration["weather-key"] ?? configuration["WeatherKey"];
        }

        public string UrlConstruct(string url)
        {
            var sb = new StringBuilder();
            sb.Append(url);

            if (!string.IsNullOrEmpty(_apiKey))
            {
                sb.Append("&appid=");
                sb.Append(Uri.EscapeDataString(_apiKey));
            }

            return sb.ToString();
        }
    }
}
