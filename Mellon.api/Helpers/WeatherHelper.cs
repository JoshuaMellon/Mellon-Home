using System.Text;

namespace Mellon.api.Helpers
{
    public class WeatherHelper(IConfiguration configuration)
    {
        private readonly string? _apiKey = configuration["Weather:Key"] ?? configuration["weather-key"] ?? configuration["WeatherKey"];

        public string UrlConstruct(string url)
        {
            var sb = new StringBuilder();
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

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
