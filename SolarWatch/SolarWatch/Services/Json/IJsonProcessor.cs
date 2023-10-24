using SolarWatch.Models.Data;
using SolarWatch.Models;

namespace SolarWatch.Services.Json
{
    public interface IJsonProcessor
    {
        Task<GeocodingApiResponse> GetGeocodingApiResponseAsync(string data);

        Task<SunriseSunsetResults> ProcessAsync(string data, string city, DateTime date);
    }
}
