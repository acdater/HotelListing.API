using HotelListing.API.Contracts;
using HotelListing.API.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repository
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        public CountriesRepository(HotelListingDbContext context) : base(context)
        {

        }

        public async Task<Country> GetDetails(int id)
        {
            return await _dbContext.Countries.Include(c => c.Hotels)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
