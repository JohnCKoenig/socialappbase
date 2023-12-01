using MobileAppAPI.ControllerModels.Accounts.Input;
using MobileAppAPI.ControllerModels.Accounts.Response;
using MobileAppAPI.ControllerModels.GeneralResponses;
using MobileAppAPI.DBModels.Accounts;

namespace MobileAppAPI.Services.Accounts
{
    public interface IAccountService
    {
        /// <summary>
        /// Registers a new user on the service
        /// </summary>
        /// <param name="user">The information of the user to create</param>
        /// <returns>A response indicating whether user creation was succesful</returns>
        Task<GeneralResponseModel> CreateUser(RegistrationModel user);

        /// <summary>
        /// Updates a user's profile details
        /// </summary>
        /// <param name="user">The user to update</param>
        /// <returns>A response idndicating whether user update was succesful</returns>
        Task<GeneralResponseModel> UpdateUser(UpdateUserModel user, Guid userid);

        /// <summary>
        /// Deletes a user from the service
        /// </summary>
        /// <param name="user">The user to delete</param>
        /// <returns>A response indicating whether delete was succesful</returns>
        Task<GeneralResponseModel> DeleteUser(Guid userid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<UserResponseModel> GetUser (Guid userid);

        /// <summary>
        /// Authenticates a user on the service
        /// </summary>
        /// <param name="user">The username and password to authenticate</param>
        /// <returns>A response containing refresh/access token</returns>
        Task<TokenResponseModel> Authenticate(SignInModel user);

        /// <summary>
        /// Creates a new JWT token for a user
        /// </summary>
        /// <param name="user">The user to generate a JWT token for</param>
        /// <returns>A token response</returns>
       Task<TokenResponseModel> GenerateAccessToken(UserModel user);

        /// <summary>
        /// Generates a refresh token for a user
        /// </summary>
        /// <param name="user">The user to generate a refresh token for</param>
        /// <returns>A refresh token response</returns>
        TokenResponseModel GenerateRefreshToken(UserModel user);

        /// <summary>
        /// Verify the existence of a token in a database
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <returns>A bool indidcating whether the token is valid</returns>
        Task<bool> ValidateRefreshToken(string refreshToken);

        /// <summary>
        /// Gets a User by their refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <returns>A User object, or null if no matches</returns>
        Task<UserModel> GetUserByRefreshToken(string refreshToken);

        /// <summary>
        /// Updates a users refresh token, or generates a new one if there is none
        /// </summary>
        /// <param name="user">The user requesting refresh</param>
        /// <param name="refreshToken">The users refresh token</param>
        /// <returns></returns>
        Task UpdateUserRefreshToken(UserModel user, string refreshToken);
    }
}
