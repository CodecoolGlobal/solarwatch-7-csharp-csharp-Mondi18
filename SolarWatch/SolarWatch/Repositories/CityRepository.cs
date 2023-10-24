using Microsoft.EntityFrameworkCore;
using SolarWatch.Models.DataClasses;

namespace SolarWatch.Repositories
{
    public class CityRepository : ICityRepository
    {
        public async Task<IEnumerable<City>> GetAllCitiesAsync() 
        {
            using var dbContext = new AppDbContext();
            return await dbContext.Cities.ToListAsync();
        }
        public async Task<City> GetCityByNameAsync(string cityName) 
        {
            using var dbContext = new AppDbContext();
            return await dbContext.Cities.FirstOrDefaultAsync(c=> c.Name == cityName);
        }
        public async Task DeleteCityAsync (City city) 
        {
            using var dbContext = new AppDbContext();
            dbContext.Remove(city);
            await dbContext.SaveChangesAsync();
        }
        public async Task UpdateCityAsync(City city) 
        {
            using var dbContext = new AppDbContext();
            dbContext.Update(city);
            dbContext.SaveChanges();
        }
        public async Task AddCityAsync(City city) 
        {
            using var dbContext = new AppDbContext();
            dbContext.Add(city);
            await dbContext.SaveChangesAsync();
        }
        public async Task<City> GetCityByIdAsync(int cityId)
        {
            using var dbContext = new AppDbContext();
            return await dbContext.Cities
                .SingleOrDefaultAsync(c => c.Id == cityId);
        }
    }
}
