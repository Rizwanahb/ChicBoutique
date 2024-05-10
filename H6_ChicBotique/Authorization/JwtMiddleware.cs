using H6_ChicBotique.Helpers;
using H6_ChicBotique.Services;
using Microsoft.Extensions.Options;

namespace H6_ChicBotique.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtUtils.ValidateJwtToken(token);
            if (userId != null)
            {
                // attach customer to context on successful jwt validation
                context.Items["Member"] = await userService.GetById(userId.Value);
            }

            await _next(context);
        }
    }
}
