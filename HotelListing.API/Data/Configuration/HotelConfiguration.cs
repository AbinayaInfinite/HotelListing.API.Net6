using HotelListing.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.Configuration
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "BlueWaters",
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
