
using SolarWatch.Contracts;
using SolarWatch.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using SolarWatch.Models.Data;
using SolarWatch.Models.DataClasses;
using SolarWatch.Repositories;
using Newtonsoft.Json;
using System.Net;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace IntegrationTest
{
    public class Tests
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            // Create an instance of WebApplicationFactory for your ASP.NET application.
            var factory = new WebApplicationFactory<Program>();
            string connectionString = "Server=localhost,1433;Database=SolarWatch;User Id=sa;Password=Kiskutyafüle32!;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            Environment.SetEnvironmentVariable("CONNECTION_STRING", connectionString);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

           
            _client = factory.CreateClient();

            AuthRequest authRequest = new AuthRequest("admin@admin.com", "admin123");
            var jsonString = JsonSerializer.Serialize(authRequest);
            var jsonStringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = _client.PostAsync("/Auth/Login", jsonStringContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var desContent = JsonSerializer.Deserialize<AuthResponse>(content,options);
            var token = desContent.Token;
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
        [Test] 
        public async Task GetSunriseSunsetAsync_ReturnsSunriseSunsetData()
        {
            // Arrange
            
            var cityName = "Budapest"; 
            var date =DateTime.Today; // Replace with a valid test date

            // Act
            var response = await _client.GetAsync($"/Sunrise/api/solar?cityName={cityName}&date={date}");

            // Assert
            response.EnsureSuccessStatusCode(); // Status code 200-299
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // You can add more specific assertions based on the expected behavior of your controller action
            // For example, check the response content, content type, or JSON deserialization.

            // Example: Deserialize the response content to a model class
            var responseContent = await response.Content.ReadAsStringAsync();
            var sunriseSunsetData = JsonConvert.DeserializeObject<SunriseSunsetResults>(responseContent);

            // Add your specific assertions here
            Assert.NotNull(sunriseSunsetData);
            Assert.AreEqual(cityName, sunriseSunsetData.City); // Replace with the property name of your data model
            Assert.AreEqual(date, sunriseSunsetData.Date); // Replace with the property name of your data model
        }

        [Test]
        public async Task GetSunriseSunsetAsync_ReturnsOk()
        {
            // Arrange
            var cityName = "Budapest";
            var date = "2022-12-12";

            // Act
            var response = await _client.GetAsync($"SunRise/{cityName}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            // Add more assertions to ensure the response is as expected for invalid data.
        }
        [Test]
        public async Task AddCityAsync_ValidCity_ShouldReturnOk()
        {
            // Arrange
            var city = new City
            {
                Id= 0,
                Name = "New York",
              Coordinates = new Coordinates(){Lat = 10,Lon = 10}
              
             };

            // Serialize the city object to JSON.
            var cityJson = JsonConvert.SerializeObject(city);
            var content = new StringContent(cityJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/city/add", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.AreEqual("City added successfully.", responseContent);
        }


    }
}