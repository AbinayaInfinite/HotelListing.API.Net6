using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Contracts
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserdto userdto);
        Task<AuthResponsedto> Login(Logindto logindto);
        Task<String> CreateRefreshToken();
        Task<AuthResponsedto> VerifyRefreshToken(AuthResponsedto request);  
    }
}
