using Mellon.api.Helpers;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Mellon.api.tests.Helpers
{
    public class WeatherHelperTests
    {
        [Fact]
        public void UrlConstruct_ShouldAppendApiKey_WhenPresent()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Weather:Key", "test-api-key" }
                })
                .Build();
            var weatherHelper = new WeatherHelper(configuration);
            var baseUrl = "https://api.openweathermap.org/data/2.5/weather?q=London";

            // Act
            var result = weatherHelper.UrlConstruct(baseUrl);

            // Assert
            Assert.Contains("appid=test-api-key", result);
        }

        [Fact]
        public void UrlConstruct_ShouldNotAppendApiKey_WhenNotPresent()
        {
            // Arrange
            var configuration = new ConfigurationBuilder().Build();
            var weatherHelper = new WeatherHelper(configuration);
            var baseUrl = "https://api.openweathermap.org/data/2.5/weather?q=London";
            // Act
            var result = weatherHelper.UrlConstruct(baseUrl);
            // Assert
            Assert.DoesNotContain("appid=", result);
        }

        [Fact]
        public void UrlConstruct_ShouldThrowArgumentNullException_WhenUrlIsNull()
        {
            // Arrange
            var configuration = new ConfigurationBuilder().Build();
            var weatherHelper = new WeatherHelper(configuration);
            string? baseUrl = null;
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => weatherHelper.UrlConstruct(baseUrl!));
        }
    }