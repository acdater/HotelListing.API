using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace HotelListing.API.Data.Configurations
{
    public class CountriesConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
                new Country
                {
                    Id = 1,
                    Name = "Moldova",
                    ShortName = "MD"
                },
                new Country
                {
                    Id = 2,
                    Name = "Ucraine",
                    ShortName = "UC"
                },
                new Country
                {
                    Id = 3,
                    Name = "Romania",
                    ShortName = "RO"
                }
            );
        }
    }
}
