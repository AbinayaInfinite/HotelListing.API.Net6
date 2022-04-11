using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace HotelListing.API.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _usermanager;
        private readonly IConfiguration _configuration;
        private ApiUser _user;

        public const string _loginProvider = "HotelListingApi";
        public const string _refreshToken = "RefreshToken";

        public AuthManager(IMapper mapper, UserManager<ApiUser> usermanager,IConfiguration configuration)
        {
            this._mapper = mapper;
            this._usermanager = usermanager;
            this._configuration = configuration;
        }

        public async Task<AuthResponsedto> Login(Logindto logindto)
        {
            _user = await _usermanager.FindByEmailAsync(logindto.Email);
            bool isValidUser = await _usermanager.CheckPasswordAsync(_user, logindto.Password);


            if (_user == null || isValidUser == false)
            {
                return null;
            }
            var token = await GenerateToken();
            return new AuthResponsedto
            {
                Token = token,
                UserId = _user.Id
            };
        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserdto userdto)
        {
            var user = _mapper.Map<ApiUser>(userdto);
            user.UserName = userdto.Email;

            var result = await _usermanager.CreateAsync(user, userdto.Password);

            if (result.Succeeded)
            {
                await _usermanager.AddToRoleAsync(user, "User");
            }

            return result.Errors;
        }
        public async Task<string> GenerateToken()
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var roles = await _usermanager.GetRolesAsync(_user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _usermanager.GetClaimsAsync(_user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,_user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,Guid.NewGuid().ToString()),

            }.Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettins:DurationInMinutes"])),
                signingCredentials : credentials                
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public async Task<string> CreateRefreshToken()
        {
            await _usermanager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);
            var newRefreshToken = await _usermanager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);
            var result = await _usermanager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, newRefreshToken);
            return newRefreshToken;
        }

        public async Task<AuthResponsedto> VerifyRefreshToken(AuthResponsedto request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var username = tokenContent.Claims.ToList().FirstOrDefault(q=>q.Type == JwtRegisteredClaimNames.Email)?.Value;
            _user = await _usermanager.FindByNameAsync(username);
            if (_user == null)
            {
                return null;
            }
            var isValidRefreshToken = await _usermanager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, request.RefreshToken);
            if (isValidRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthResponsedto
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = await CreateRefreshToken()
                };
            }
            await _usermanager.UpdateSecurityStampAsync(_user);
            return null;
        }
    }
}
