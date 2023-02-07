using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Contracts
{
    public interface ICountriesRepository : IGenericRepository<Country>
    {
        public Task<Country> GetDetails(int id);
    }
}
