using System.Security.Claims;

namespace Finance.Api.Middlewares;

public class UserIdMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("X-User-Id", out var userIdValue) &&
            Guid.TryParse(userIdValue, out var userId))
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var identity = new ClaimsIdentity(claims, "X-User-Id");
            context.User = new ClaimsPrincipal(identity);
        }

        await next(context);
    }
}