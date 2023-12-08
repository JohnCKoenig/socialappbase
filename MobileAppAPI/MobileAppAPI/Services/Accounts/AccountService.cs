using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MobileAppAPI.ControllerModels.Accounts.Input;
using MobileAppAPI.ControllerModels.Accounts.Response;
using MobileAppAPI.ControllerModels.GeneralResponses;
using MobileAppAPI.DBModels;
using MobileAppAPI.DBModels.Accounts;
using MobileAppAPI.Helpers;
using MobileAppAPI.Services.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MobileAppAPI.Services.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private static IConfiguration _configuration;

        public AccountService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        /// <summary>
        /// Authenticates a user on the service
        /// </summary>
        /// <param name="user">The username and password to authenticate</param>
        /// <returns>A response containing refresh/access token</returns>
        public async Task<GeneralResponseModel<object>> Authenticate(SignInModel model)
        {

            // Find the user by username (or email, whichever they are using)
            var user = _context.Users.SingleOrDefault(u => u.username == model.Username || u.email == model.Username);

            // If the user is not found or the password doesn't match, return unauthorized
            if (user == null || !PasswordHelper.VerifyPassword(model.Password, user.password_hash))
            {
                return GeneralResponseModel<object>.ErrorResponse("No account found");
            }

            // User is authenticated, generate an access token and a refresh token
            var accessToken = await this.GenerateAccessToken(user);
            var refreshToken = this.GenerateRefreshToken(user);

            // Store the refresh token in the database
            var refreshTokenEntity = new RefreshTokenModel
            {
                userid = user.userid,
                token = refreshToken.RefreshToken,
                expirydate = DateTime.Parse(refreshToken.RefreshTokenExpiry)
            };
            await _context.RefreshTokens.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();

            //Create a response containing both tokens
            TokenResponseModel response = new TokenResponseModel
            {
                AccessToken = accessToken.AccessToken,
                AccessTokenExpiry = accessToken.AccessTokenExpiry,
                RefreshToken = refreshToken.RefreshToken,
                RefreshTokenExpiry = refreshToken.RefreshTokenExpiry,
            };
            return GeneralResponseModel<object>.SuccessResponse(response);
        }
        /// <summary>
        /// Registers a new user on the service
        /// </summary>
        /// <param name="user">The new users information</param>
        /// <returns>A response indicating whether creation was succesful</returns>
        public async Task<GeneralResponseModel<object>> CreateUser(RegistrationModel user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.email == user.Email);
            if (existingUser != null)
            {
                // User with this email already exists, handle accordingly
                return GeneralResponseModel<object>.ErrorResponse(ResponseCode.UserAlreadyExists, "A user with these details already exists.");
            }
            // Hash the password
            string hashedPassword = PasswordHelper.HashPassword(user.Password);

            //Create a new user and save the data
            var newUser = new UserModel
            {
                username = user.Username,
                email = user.Email,
                phone_number = user.PhoneNumber,
                password_hash = hashedPassword
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return GeneralResponseModel<object>.SuccessResponse();
        }

        /// <summary>
        /// Updates a users profile information
        /// </summary>
        /// <param name="user">The user to update</param>
        /// <param name="userid">the id of the user to update</param>
        /// <returns>A response indicating whether the user was updated successfully</returns>
        public async Task<GeneralResponseModel<object>> UpdateUser(UpdateUserModel user, Guid userid)
        {
            // Check for existing email or username
            var emailCheck = await _context.Users.AnyAsync(u => u.email == user.email && u.userid != userid);
            var usernameCheck = await _context.Users.AnyAsync(u => u.username == user.username && u.userid != userid);
            if (emailCheck || usernameCheck)
            {
                return GeneralResponseModel<object>.ErrorResponse(ResponseCode.UserAlreadyExists,"A user with these details already exists.");
            }

            // Find the user by ID
            var selectedUser = await _context.Users.FindAsync(userid);
            if (selectedUser == null)
            {
                return GeneralResponseModel<object>.ErrorResponse("No user found");
            }

            // Update the user properties
            selectedUser.username = user.username;
            selectedUser.email = user.email;
            selectedUser.phone_number = user.phone_number;

            // Save changes
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return GeneralResponseModel<object>.SuccessResponse();
            }
            else
            {
                return GeneralResponseModel<object>.ErrorResponse("Failed to update user");
            }

        }

        /// <summary>
        /// Given a user ID, remove the user and associated resources
        /// </summary>
        /// <param name="userid">the id of the user to remove</param>
        /// <returns>A response indicating whether the user was removed succesfully</returns>
        public async Task<GeneralResponseModel<object>> DeleteUser(Guid userid)
        {
            // Find the user by ID
            var selectedUser = await _context.Users.FindAsync(userid);
            if (selectedUser == null)
            {
                return GeneralResponseModel<object>.ErrorResponse("User not found");
            }
            var userRefreshTokens = _context.RefreshTokens.Where(rt => rt.userid == selectedUser.userid).ToList();
            _context.RefreshTokens.RemoveRange(userRefreshTokens);
            _context.Users.Remove(selectedUser);
            if (await _context.SaveChangesAsync() >= 1)
            {
                return GeneralResponseModel<object>.SuccessResponse();
            }


            return GeneralResponseModel<object>.ErrorResponse("Failed to delete user");

        }

        /// <summary>
        /// Gets a users information by their ID
        /// </summary>
        /// <param name="user">The userid of the user to retrieve</param>
        /// <returns>A model containing the user information visible</returns>
        public async Task<GeneralResponseModel<UserResponseModel>> GetUser(Guid userid)
        {
            // Find the user by ID
            var selectedUser = await _context.Users.FindAsync(userid);
            if (selectedUser == null)
            {
                return GeneralResponseModel<UserResponseModel>.ErrorResponse("User not found");
            }
            UserResponseModel response = new UserResponseModel
            {
                username = selectedUser.username,
                email = selectedUser.email,
                phonenumber = selectedUser.phone_number,

            };


            return GeneralResponseModel<UserResponseModel>.SuccessResponse(response);

        }

        /// <summary>
        /// Generates a new JWT token
        /// </summary>
        /// <param name="user">The user to create the token for</param>
        /// <returns>A string containing the token</returns>
        public async Task<TokenResponseModel> GenerateAccessToken(UserModel user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]); // Using _configuration

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                   new Claim(ClaimTypes.NameIdentifier, user.userid.ToString()),
                   new Claim(ClaimTypes.Name, user.username),
                   new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(SecurityConstants.ACCESS_TOKEN_EXPIRY),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);
            RedisService rs = new RedisService(_configuration);
            await rs.SetSessionAsync(tokenHandler.WriteToken(token), user.userid.ToString(), TimeSpan.FromHours(SecurityConstants.ACCESS_TOKEN_EXPIRY));
            TokenResponseModel trm = new TokenResponseModel
            {
                AccessToken = tokenHandler.WriteToken(token),
                AccessTokenExpiry = tokenDescriptor.Expires.ToString()
            };
            return trm;
        }
        /// <summary>
        /// Generates a refresh token for a user
        /// </summary>
        /// <param name="user">The user to generate the refresh token for</param>
        /// <returns>A string containing the refresh token</returns>
        public TokenResponseModel GenerateRefreshToken(UserModel user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["RefreshToken:Key"]); // Using _configuration

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                   new Claim(ClaimTypes.NameIdentifier, user.userid.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(SecurityConstants.REFRESH_TOKEN_EXPIRY),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            TokenResponseModel trm = new TokenResponseModel
            {
                RefreshToken = tokenHandler.WriteToken(token),
                RefreshTokenExpiry = tokenDescriptor.Expires.ToString()
            };

            return trm;
        } 

        /// <summary>
        /// Verifies the existence of a given refresh token
        /// </summary>
        /// <param name="refreshToken">The token to verify</param>
        /// <returns>True if the token exists, false if the token does not exist</returns>
        public async Task<bool> ValidateRefreshToken(string refreshToken)
        {
            // Check if the refresh token exists in the RefreshTokens table
            return await _context.RefreshTokens.AnyAsync(rt => rt.token == refreshToken);
        }

        /// <summary>
        /// Gets a User given a refresh token
        /// </summary>
        /// <param name="refreshToken">The refresh token to look up</param>
        /// <returns>A User</returns>
        public async Task<GeneralResponseModel<UserModel>> GetUserByRefreshToken(string refreshToken)
        {
            // Retrieve the user associated with the refresh token from the Users and RefreshTokens tables
            var user = await _context.Users
                .Join(
                    _context.RefreshTokens,
                    u => u.userid,
                    rt => rt.userid,
                    (u, rt) => new { User = u, RefreshToken = rt }
                )
                .Where(joined => joined.RefreshToken.token == refreshToken)
                .Select(joined => joined.User)
                .FirstOrDefaultAsync();

            return GeneralResponseModel<UserModel>.SuccessResponse(user);
        }

        /// <summary>
        /// Updates a User's refresh token
        /// </summary>
        /// <param name="user">The user to update</param>
        /// <param name="refreshToken">The current refresh token</param>
        /// <returns></returns>
        public async Task UpdateUserRefreshToken(UserModel user, string refreshToken)
        {
            // Check if the user already has a refresh token entry in the RefreshTokens table
            var existingRefreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.userid == user.userid);

            if (existingRefreshToken != null)
            {

                existingRefreshToken.token = refreshToken;
            }
            else
            {

                var newRefreshToken = new RefreshTokenModel
                {
                    userid = user.userid,
                    token = refreshToken
                };
                _context.RefreshTokens.Add(newRefreshToken);
            }

            await _context.SaveChangesAsync();
        }


    }
}
