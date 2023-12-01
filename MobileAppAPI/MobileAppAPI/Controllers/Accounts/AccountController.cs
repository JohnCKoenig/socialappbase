using Microsoft.AspNetCore.Mvc;
using MobileAppAPI.ControllerModels;
using MobileAppAPI.ControllerModels.Accounts.Input;
using MobileAppAPI.ControllerModels.Accounts.Response;
using MobileAppAPI.DBModels;
using MobileAppAPI.DBModels.Accounts;
using MobileAppAPI.Helpers;
using MobileAppAPI.Services.Accounts;
using MobileAppAPI.ControllerModels.GeneralResponses;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Configuration.UserSecrets;
using MobileAppAPI.Services.Security;

namespace MobileAppAPI.Controllers.Accounts
{
    [Route("accounts")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static IConfiguration _configuration;
        public AccountController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }
        /// <summary>
        /// Registers a new user on the service
        /// </summary>
        /// <param name="model">The new user model</param>
        /// <returns>200 Ok for valid success, 400 bad request if invalid data</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel model)
        {
            //Check if user filled out correct data
            if (!ModelState.IsValid)
            {
                return BadRequest(new GeneralResponseModel(ResponseCode.Failure, "Invalid request"));
            }
            AccountService actsrv = new AccountService(_context, _configuration);
            var response = await actsrv.CreateUser(model);

            if (response.Code == ResponseCode.Failure)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Signs in a user to the service
        /// </summary>
        /// <param name="model">The sign-in model</param>
        /// <returns>400 Bad Request if invalid, 200 Ok if credentials match, 401 unauthorized if credentials don't match</returns>
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GeneralResponseModel(ResponseCode.Failure, "Invalid request"));
            }
            //Create an accountservice with the config
            AccountService actsrv = new AccountService(_context, _configuration);

            //try authenticating with provided details
            var response = await actsrv.Authenticate(model);

            //if we didn't get an acccess token, the account doens't exist
            if (!(response.AccessToken == null))
            {
                return Ok(response);
            }
            return Unauthorized(new GeneralResponseModel(ResponseCode.InvalidCredentials, "User does not exist"));

        }
        /// <summary>
        /// Deletes a user from the service
        /// </summary>
        /// <returns>A result indicating whether deletion was succesful</returns>
        [Authorize]
        [HttpDelete("deleteuser")]
        public async Task<IActionResult> DeleteUser()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GeneralResponseModel(ResponseCode.Failure, "Invalid request"));
            }
            var response = new GeneralResponseModel(ResponseCode.Failure, "Delete operation failed");
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                AccountService actsrv = new AccountService(_context, _configuration);
                var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
                response = await actsrv.DeleteUser(userId);
            }
            if (!(response.Code == ResponseCode.Failure))
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        /// <summary>
        /// Get information about the currently authenticated user
        /// </summary>
        /// <returns>Payload containing the user info</returns>
        [Authorize]
        [HttpGet("getuser")]
        public async Task<IActionResult> GetUser()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GeneralResponseModel(ResponseCode.Failure, "Invalid request"));
            }
            UserResponseModel response = new UserResponseModel();
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                AccountService actsrv = new AccountService(_context, _configuration);
                var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
                response = await actsrv.GetUser(userId);

            }
            if (response.username!=null)
            {
                return Ok(response);
            }
            return BadRequest(new GeneralResponseModel(ResponseCode.Failure, "This user does not exist"));
        }

        /// <summary>
        /// Updates a users profile information
        /// </summary>
        /// <returns>A response indicating whether the update was succesful</returns>
        [Authorize]
        [HttpPatch("updateuser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GeneralResponseModel(ResponseCode.Failure, "Invalid request"));
            }
            var response = new GeneralResponseModel(ResponseCode.Failure, "Update operation failed");
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                AccountService actsrv = new AccountService(_context, _configuration);
                var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
                response = await actsrv.UpdateUser(model, userId);
            }
            if (!(response.Code == ResponseCode.Failure))
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        /// <summary>
        /// Gets a new access and refresh token given a valid refresh token
        /// </summary>
        /// <param name="refreshTokenModel">The users current refresh token</param>
        /// <returns>A new access and refresh token</returns>
        [HttpPost("refreshtoken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshAccessTokenModel refreshTokenModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GeneralResponseModel(ResponseCode.Failure, "Invalid request"));
            }
            AccountService actsrv = new AccountService(_context, _configuration);

            // Validate refresh token
            if (await actsrv.ValidateRefreshToken(refreshTokenModel.refreshToken))
            {
                UserModel user = await actsrv.GetUserByRefreshToken(refreshTokenModel.refreshToken);

                if (user != null)
                {
                    var newAccessToken = await actsrv.GenerateAccessToken(user);
                    var newRefreshToken = actsrv.GenerateRefreshToken(user);

                    // Update the user's refresh token in the database
                    await actsrv.UpdateUserRefreshToken(user, newRefreshToken.RefreshToken);

                    var tokenResponse = new TokenResponseModel
                    {
                        AccessToken = newAccessToken.AccessToken,
                        AccessTokenExpiry = newAccessToken.AccessTokenExpiry,
                        RefreshToken = newRefreshToken.RefreshToken,
                        RefreshTokenExpiry = newRefreshToken.RefreshTokenExpiry

                    };

                    return Ok(tokenResponse);
                }
            }

            return BadRequest(new GeneralResponseModel(ResponseCode.Failure, "Invalid refresh token"));
        }
        [HttpPost("InvalidateToken")]
        public async Task<IActionResult> InvalidateToken([FromBody] RefreshAccessTokenModel refreshTokenModel)
        {
            RedisService rs = new RedisService(_configuration);
            await rs.InvalidateSessionAsync(refreshTokenModel.refreshToken);
            return Ok("test");
        }
    }

   
}
