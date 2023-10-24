using SolarWatch.Models.DataClasses;

namespace SolarWatch.Repositories
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetAllCitiesAsync();
        Task<City> GetCityByNameAsync(string cityName);
        Task DeleteCityAsync(City city);
        Task UpdateCityAsync(City city);
        Task AddCityAsync(City city);
        Task<City> GetCityByIdAsync(int cityId);
    }
}
