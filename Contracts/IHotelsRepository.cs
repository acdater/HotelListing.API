using HotelListing.API.Data;

namespace HotelListing.API.Contracts
{
    public interface IHotelsRepository : IGenericRepository<Hotel>
    {
        public Task<Hotel> GetDetailsAsync(int id);
    }
}
