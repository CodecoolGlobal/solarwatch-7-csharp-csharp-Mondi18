namespace SolarWatch.Models.DataClasses
{
    public class SunriseSunsetData
    {
        public int Id { get; set; } 
        public DateTimeOffset Sunrise { get; set; }
        public DateTimeOffset Sunset { get; set; }
        public DateTime? Date { get; set; }
        public int CityId { get; set; } 
    }
}
