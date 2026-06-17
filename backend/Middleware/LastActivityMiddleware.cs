using System.Security.Claims;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Middleware;

public sealed class LastActivityMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, AuthDbContext db)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                var now = DateTime.UtcNow;

                await db.Users
                    .Where(user => user.Id == userId)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(user => user.LastActivityAt, now));
            }
        }

        await next(context);
    }
}
