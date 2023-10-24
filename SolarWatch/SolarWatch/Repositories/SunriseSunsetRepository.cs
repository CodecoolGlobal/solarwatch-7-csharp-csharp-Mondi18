using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SolarWatch.Models.DataClasses;

namespace SolarWatch.Repositories
{
    public class SunriseSunsetRepository : ISunriseSunsetRepository
    {
        public async Task<SunriseSunsetData> GetSunriseSunsetDataAsync(int cityId, DateTime date)
        {
            using var dbContext = new AppDbContext();
            return await dbContext.SunriseSunsetTimes
                .SingleOrDefaultAsync(ss => ss.CityId == cityId && ss.Date == date);
        }

        public async Task AddSunriseSunsetDataAsync(SunriseSunsetData sunriseSunsetData)
        {
            try
            {
                using var dbContext = new AppDbContext();
                dbContext.SunriseSunsetTimes.Add(sunriseSunsetData);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and take appropriate action
                Console.WriteLine($"Error adding sunrise/sunset data: {ex.Message}");
            }
        }

        public async Task<IEnumerable<SunriseSunsetData>> GetAllSunriseSunsetDataAsync(int cityId)
        {
            using var dbContext = new AppDbContext();
            return await dbContext.SunriseSunsetTimes
                .Where(ss => ss.CityId == cityId)
                .ToListAsync();
        }

        public async Task<bool> AddCityAsync(City city)
        {

            await using var dbContext = new AppDbContext(); // Injected through constructor.


            await dbContext.Cities.AddAsync(city);
            await dbContext.SaveChangesAsync();
            return true;
        }

    }
}


