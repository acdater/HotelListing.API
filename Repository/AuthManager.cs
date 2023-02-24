using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace HotelListing.API.Repository
{
    public class AuthManager : IAuthManager
    {
        private const string _loginProvider = "HotelListingApi";
        private const string _refereshToken = "RefreshToken";
        private IMapper _mapper;
        private UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private ApiUser _user;

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            this._mapper = mapper;
            this._userManager = userManager;
            this._configuration = configuration;
        }

        public async Task<AuthResponseDto> Login(LoginUserDto loginUserDto)
        {
            _user = await _userManager.FindByEmailAsync(loginUserDto.Email);
            var isValidUser = await _userManager.CheckPasswordAsync(_user, loginUserDto.Password);

            if (_user == null || isValidUser == false)
            {
                return null;
            }
            
            var token = await GenerateTokenAsync();
            return new AuthResponseDto
            {
                Token = token,
                UserId = _user.Id,
                RefreshToken = await CreateRefereshToken()
            };
        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            _user = _mapper.Map<ApiUser>(userDto);
            _user.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(_user, userDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, "User");
            }

            return result.Errors;
        }

        private async Task<string> GenerateTokenAsync()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credintials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(_user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            var userClaims = await _userManager.GetClaimsAsync(_user);

            var claims = new List<Claim> 
            { 
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim("uid", _user.Id)
            }
            .Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration
                    ["JwtSettings:DurationInMinutes"])),
                signingCredentials: credintials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateRefereshToken()
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refereshToken);
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refereshToken);
            var result = await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refereshToken,
                newRefreshToken);

            return newRefreshToken;
        }

        public async Task<AuthResponseDto> VerefyRefershToken(AuthResponseDto request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var userEmail = tokenContent.Claims.ToList().FirstOrDefault(c =>
                c.Type == JwtRegisteredClaimNames.Email)?.Value;

            _user = await _userManager.FindByEmailAsync(userEmail);

            if (_user == null || !_user.Id.Equals(request.UserId)) 
                return null;

            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(
                _user, _loginProvider, _refereshToken, request.RefreshToken);

            if (isValidRefreshToken)
            {
                var token = await GenerateTokenAsync();
                return new AuthResponseDto
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = await CreateRefereshToken()
                };
            }

            await _userManager.UpdateSecurityStampAsync(_user);

            return null;
        }
    }
}
