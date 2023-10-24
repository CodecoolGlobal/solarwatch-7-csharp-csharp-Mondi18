namespace SolarWatch.Models
{
    public class SunriseSunsetResults
    {
        public string City { get; set; }
        public DateTime? Date { get; set; }
        public DateTimeOffset Sunrise { get; set; }
        public DateTimeOffset Sunset { get; set; }

    }
}

