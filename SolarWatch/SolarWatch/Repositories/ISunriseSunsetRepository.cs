using SolarWatch.Models.DataClasses;

namespace SolarWatch.Repositories
{
    public interface ISunriseSunsetRepository
    {
        Task<SunriseSunsetData> GetSunriseSunsetDataAsync(int cityId, DateTime date);
        Task AddSunriseSunsetDataAsync(SunriseSunsetData sunriseSunsetData);

        Task<IEnumerable<SunriseSunsetData>> GetAllSunriseSunsetDataAsync(int cityId);
        Task<bool> AddCityAsync(City city);
    }
}
