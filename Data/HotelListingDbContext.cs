using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data
{
    public class HotelListingDbContext : DbContext
    {
        public HotelListingDbContext(DbContextOptions options) : base(options) 
        {
            
        }

        public DbSet<Hotel> Hotels { get; set; }    

        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().HasData(
                new Country {
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

            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Redisson MD",
                    Address = "St Cel Mare",
                    CountryId= 1,
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
