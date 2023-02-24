using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace HotelListing.API.Data.Configurations
{
    public class HotelsConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Redisson MD",
                    Address = "St Cel Mare",
                    CountryId = 1,
                    Rating = 4.8
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Jinchik MD",
                    Address = "St Pavel",
                    CountryId = 2,
                    Rating = 3.8
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Buch Vest",
                    Address = "Columna",
                    CountryId = 3,
                    Rating = 4.9
                }
            );
        }
    }
}
