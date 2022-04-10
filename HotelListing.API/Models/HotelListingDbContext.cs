using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Models
{
    public class HotelListingDbContext:DbContext
    {
        public HotelListingDbContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "India",
                    ShortName = "IN"

                },
                new Country
                {
                    Id = 2,
                    Name = "Bahamas",
                    ShortName = "BS"

                },
                new Country
                {
                    Id = 3,
                    Name = "UnitedArabEmirates",
                    ShortName = "UAE"

                }
                ) ;
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name="BlueWaters",
                    Address = "Chennai",
                    CountryId = 1,
                    Rating = 4.5
                },
                new Hotel
                {
                    Id = 2,
                    Name = "CaveMan",
                    Address = "Nassua",
                    CountryId = 2,
                    Rating = 4.5
                },
                new Hotel
                {
                    Id = 3,
                    Name = "BurjAlArab",
                    Address = "Dubai",
                    CountryId = 3,
                    Rating = 4.9
                },
                new Hotel
                {
                    Id = 4,
                    Name = "Sofetal",
                    Address = "AbuDhabi",
                    CountryId = 3,
                    Rating = 4.8
                }
                );
        }
    }
}
