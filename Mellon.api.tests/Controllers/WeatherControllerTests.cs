using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mellon.api.tests.Controllers
{
    public class WeatherControllerTests
    {
        [Fact]
        public async Task Get_Current_Forecast_ShouldReturnWeatherData()
        {
            // Arrange
            var factory = new WebApplicationFactory<Program>(); // create a test server for the application
            var client = factory.CreateClient();
            // Act
            var response = client.GetAsync("api/weather/current-forecast?cityName=London");
            // Assert
            response.Result.EnsureSuccessStatusCode(); // check if the response is successful
            var content = await response.Result.Content.ReadAsStringAsync();
            Assert.Contains("London", content);
        }

        [Fact]
        public async Task Get_Current_Forecast_ShouldReturnBadRequest_ForInvalidCity()
        {
            // Arrange
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();
            // Act
            var response = await client.GetAsync("api/weather/current-forecast?cityName=InvalidCity");
            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode); // check if the response is BadRequest
        }

        [Fact]
        public async Task Get_Hourly_Forecast_ShouldReturnWeatherData()
        {
            // Arrange
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();
            // Act
            var response = await client.GetAsync("api/weather/hourly-forecast-four-days?cityName=London");
            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("London", content);
        }

        [Fact]
        public async Task Get_Hourly_Forecast_ShouldReturnBadRequest_ForInvalidCity()
        {
            // Arrange
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();
            // Act
            var response = await client.GetAsync("api/weather/hourly-forecast-four-days?cityName=InvalidCity");
            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Get_Geo_Data_ShouldReturnCoordinates()
        {
            // Arrange
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();
            // Act
            var response = await client.GetAsync("api/weather/geo-data?cityName=London");
            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("lat", content);
            Assert.Contains("lon", content);
        }

        [Fact]
        public async Task Get_Geo_Data_ShouldReturnBadRequest_ForInvalidCity()
        {
            // Arrange
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();
            // Act
            var response = await client.GetAsync("api/weather/geo-data?cityName=InvalidCity");
            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}