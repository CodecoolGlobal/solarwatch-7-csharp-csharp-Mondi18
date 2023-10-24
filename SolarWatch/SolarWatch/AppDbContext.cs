using Microsoft.EntityFrameworkCore;
using SolarWatch.Models;
using SolarWatch.Models.Data;
using SolarWatch.Models.DataClasses;


namespace SolarWatch
{
    public class AppDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<SunriseSunsetData> SunriseSunsetTimes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=localhost,1433;Database=SolarWatch;User Id=sa;Password=Kiskutyafüle32!;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<City>()
                .HasIndex(u => u.Id)
                .IsUnique();

            modelBuilder.Entity<City>()
                .OwnsOne(c => c.Coordinates, cb =>
                {
                    cb.Property(co => co.Lat).HasColumnName("Coordinates_Lat");
                    cb.Property(co => co.Lon).HasColumnName("Coordinates_Lon");
                });
           

        }


    }
}
