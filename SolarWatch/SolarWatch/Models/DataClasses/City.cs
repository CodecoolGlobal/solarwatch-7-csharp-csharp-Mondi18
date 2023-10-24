using SolarWatch.Models.Data;

namespace SolarWatch.Models.DataClasses
{
    public class City
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public Coordinates Coordinates { get; set; }
    }
}
