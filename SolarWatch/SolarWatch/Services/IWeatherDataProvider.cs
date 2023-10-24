namespace SolarWatch.Services
{
    public interface IWeatherDataProvider
    {
        Task<string> GetLatLonAsync(string city);
        Task<string> GetSunriseSunsetAsync(double lat, double lon, DateTime date);
    }
}
