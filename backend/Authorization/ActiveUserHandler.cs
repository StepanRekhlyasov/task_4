using System.Security.Claims;
using backend.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace backend.Authorization;

public sealed class ActiveUserHandler(UserManager<User> userManager)
    : AuthorizationHandler<ActiveUserRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ActiveUserRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated != true) return;

        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return;

        var user = await userManager.FindByIdAsync(userId);
        if (user is { IsActive: true }) context.Succeed(requirement);
    }
}
