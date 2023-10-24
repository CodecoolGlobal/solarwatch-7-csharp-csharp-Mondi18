using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models.Data;
using SolarWatch.Services;
using SolarWatch.Services.Json;
using SolarWatch.Repositories;
using SolarWatch.Models.DataClasses;

namespace SolarWatch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SunriseController : Controller
    {
        private readonly ILogger<SunriseController> _logger;
        private readonly ICityRepository _cityRepository;
        private readonly IWeatherDataProvider _weatherDataProvider;
        private readonly IJsonProcessor _jsonProcessor;
        private readonly ISunriseSunsetRepository _sunriseSunsetRepository;

        public SunriseController(ILogger<SunriseController> logger, IWeatherDataProvider weatherDataProvider,
            IJsonProcessor jsonProcessor, ICityRepository cityRepository,
            ISunriseSunsetRepository sunriseSunsetRepository)
        {
            _logger = logger;
            _weatherDataProvider = weatherDataProvider;
            _jsonProcessor = jsonProcessor;
            _cityRepository = cityRepository;
            _sunriseSunsetRepository = sunriseSunsetRepository;
        }

        [HttpGet]
        [Route("api/solar")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SunriseSunsetResults>> GetSunriseSunsetAsync(string cityName, DateTime date)
        {
            var city = await _cityRepository.GetCityByNameAsync(cityName);
            if (city == null)
            {
                var GeoData = await _weatherDataProvider.GetLatLonAsync(cityName);
                var GeoResult = await _jsonProcessor.GetGeocodingApiResponseAsync(GeoData);

                var lat = GeoResult.Coord.Lat;
                var lon = GeoResult.Coord.Lon;


                city = new City { Name = cityName, Coordinates = new Coordinates { Lat = lat, Lon = lon } };
                await _cityRepository.AddCityAsync(city);

                Console.WriteLine("-------------");
                Console.WriteLine($"| City:{city.Name} was created in the DB |");
                Console.WriteLine("-------------");
                Console.WriteLine();

            }


            var sunriseSunset = await _sunriseSunsetRepository.GetSunriseSunsetDataAsync(city.Id, date);
            if (sunriseSunset == null)
            {
                // Fetch sunrise/sunset data from external API
                var weatherData =
                    await _weatherDataProvider.GetSunriseSunsetAsync(city.Coordinates.Lat, city.Coordinates.Lon, date);
                var sunriseSunsetData = await _jsonProcessor.ProcessAsync(weatherData, cityName, date);
                Console.WriteLine($"JSON PROCESS SUCCESS: {sunriseSunsetData.Sunrise}");
                //await _sunriseSunsetRepository.AddAsync(sunriseSunsetData);
                await _sunriseSunsetRepository.AddSunriseSunsetDataAsync(new SunriseSunsetData
                {
                    CityId = city.Id,
                    Sunrise = sunriseSunsetData.Sunrise,
                    Sunset = sunriseSunsetData.Sunset,
                    Date = date
                });

                Console.WriteLine("-------------");
                Console.WriteLine($"| City:{sunriseSunsetData.City} was isnerted into the DB |");
                Console.WriteLine("-------------");
                Console.WriteLine();
                return Ok(sunriseSunsetData);
            }

            return Ok(sunriseSunset);

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{cityName}")]
        public async Task<IActionResult> GetCityByNameAsync(string cityName)
        {
            var city = await _cityRepository.GetCityByNameAsync(cityName);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{cityId}")]
        public async Task<IActionResult> DeleteCityAsync(int cityId)
        {
            var existingCity = await _cityRepository.GetCityByIdAsync(cityId);

            if (existingCity == null)
            {
                return NotFound();
            }

            await _cityRepository.DeleteCityAsync(existingCity);
            return Ok($"City with {cityId} was succesful deleted!");
        }

       
        [HttpPost("/city/add")]
        public async Task<IActionResult> AddCityAsync(City city)
        {
            if (city == null)
            {
                return BadRequest("Invalid input. City object is null.");
            }
            bool isCityAdded = await _sunriseSunsetRepository.AddCityAsync(city);
            try
            {
               
                Console.WriteLine(isCityAdded);
               
                if (isCityAdded)
                {
                    return Ok("City added successfully.");
                }
                else
                {
                    return StatusCode(500, "Failed to add the city.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(isCityAdded);
                
                // Log the exception for debugging and return a 500 Internal Server Error.
                return StatusCode(500, "An error occurred while adding the city.");
            }
        }
    }
}

