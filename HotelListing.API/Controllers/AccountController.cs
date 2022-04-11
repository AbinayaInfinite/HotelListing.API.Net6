using HotelListing.API.Contracts;
using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authmanager;
        private readonly ILogger _logger;

        public AccountController(IAuthManager authmanager, ILogger _logger)
        {
            this._authmanager = authmanager;
            this._logger = _logger;

        }
        //POST: api/Account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult>Register([FromBody] ApiUserdto apiuserdto)
        {
            _logger.LogInformation($"Registration Attemp for {apiuserdto.Email}");
            try
            {
                var errors = await _authmanager.Register(apiuserdto);
                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                    
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in {nameof(Register)} - User registration attempt for {apiuserdto.Email} ");
                return Problem($"Something went wrong in {nameof(Register)}.",statusCode:500);
            }
        }
        //POST: api/Account/Login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Login([FromBody] Logindto logindto)
        {
            var authResponse = await _authmanager.Login(logindto);
            if (authResponse == null)
            {
                return Unauthorized();//[401 - not authenticated user] [403 - forbidden - not authorized user]
            }
            return Ok();
        }

        //POST: api/Account/RefreshToken
        [HttpPost]
        [Route("refreshToken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RefreshToken([FromBody] AuthResponsedto request)
        {
            var authResponse = await _authmanager.VerifyRefreshToken(request);
            if (authResponse == null)
            {
                return Unauthorized();//[401 - not authenticated user] [403 - forbidden - not authorized user]
            }
            return Ok(authResponse);
        }

    }
}
