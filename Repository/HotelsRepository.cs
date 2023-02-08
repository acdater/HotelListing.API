using HotelListing.API.Contracts;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repository
{
    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {
        public HotelsRepository(HotelListingDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Hotel> GetDetailsAsync(int id)
        {
            return await _dbContext.Hotels.Include(h => h.Country).FirstOrDefaultAsync(h => h.Id == id);
        }
    }
}
