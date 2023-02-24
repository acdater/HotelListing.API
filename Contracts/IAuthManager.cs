using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Contracts
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto);

        Task<AuthResponseDto> Login(LoginUserDto loginUserDto);

        Task<string> CreateRefereshToken();

        Task<AuthResponseDto> VerefyRefershToken(AuthResponseDto request);
    }
}
