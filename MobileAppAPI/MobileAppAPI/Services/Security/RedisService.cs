using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
namespace MobileAppAPI.Services.Security
{
    public class RedisService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly StackExchange.Redis.IDatabase _db;

        public RedisService(IConfiguration configuration)
        {

            string connectionString = configuration["RedisConnection:LocalDB"];
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _db = _redis.GetDatabase();
        }

        public async Task SetSessionAsync(string token, string userid, TimeSpan? expiry = null)
        {
            var sessionID = GetTokenId(token);

            var userSessionsKey = $"user_sessions:{userid}";

            // Add the session ID to the user's set of sessions
            await _db.SetAddAsync(userSessionsKey, sessionID);

            await _db.KeyExpireAsync(sessionID, expiry);

        }
        public async Task<List<string>> GetSessionsAsync(string userId)
        {
            var userSessionsKey = $"user_sessions:{userId}";
            var sessionIds = await _db.SetMembersAsync(userSessionsKey);

            var sessions = new List<string>();
            foreach (var sessionId in sessionIds)
            {

                sessions.Add(sessionId.ToString());
            }

            return sessions;
        }

        public async Task InvalidateSessionAsync(string token)
        {
            var sessionID = GetTokenId(token);
            var tokenExpiry = GetTokenExpiryDuration(token);

            if (sessionID != null)
            {
                var blacklistKey = "blacklisted_sessions";
                await _db.SetAddAsync(blacklistKey, sessionID);

                if (tokenExpiry.HasValue)
                {
                    await _db.KeyExpireAsync(blacklistKey, tokenExpiry.Value);
                }
            }
        }
        public async Task<bool> IsSessionValid(string token)
        {
            var sessionID = GetTokenId(token);
            if (sessionID != null)
            {
                var blacklistKey = "blacklisted_sessions";
                bool isBlacklisted = await _db.SetContainsAsync(blacklistKey, sessionID);
                return !isBlacklisted;
            }
            return false;
        }
        private string GetTokenId(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);
                var jtiClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Jti);
                return jtiClaim?.Value;
            }

            return null;
        }
        private TimeSpan? GetTokenExpiryDuration(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);
                var expiryClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Exp)?.Value;
                if (DateTimeOffset.TryParse(expiryClaim, out var expiryDate))
                {
                    var expiryDuration = expiryDate - DateTimeOffset.UtcNow;
                    return expiryDuration > TimeSpan.Zero ? expiryDuration : null;
                }
            }

            return null;
        }


    }
}
