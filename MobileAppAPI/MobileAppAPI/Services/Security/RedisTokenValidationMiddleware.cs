namespace MobileAppAPI.Services.Security
{
    public class RedisTokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RedisService _redisService; 

        public RedisTokenValidationMiddleware(RequestDelegate next, RedisService redisService)
        {
            _next = next;
            _redisService = redisService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string token = null;
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                token = authorizationHeader.Substring("Bearer ".Length).Trim();
            }
            if (context.User.Identity.IsAuthenticated)
            {
                var isValid = await _redisService.IsSessionValid(token);

                if (!isValid)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
            }

            await _next(context);
        }
    }
}
