using backend.Data;
using backend.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("api")]
    public class UsersController(AuthDbContext dbContext, UserManager<User> userManager) : ControllerBase
    {
        [HttpGet("users")]
        [Authorize]
        public IActionResult GetUsers()
        {
            var users = dbContext.Users
                .Select(user => new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.EmailConfirmed,
                    user.IsActive,
                    user.CreatedAt,
                    user.UpdatedAt,
                    user.LastActivityAt
                })
                .ToList();

            return Ok(users);
        }

        [HttpPost("users/{id}/block")]
        [Authorize]
        public async Task<IActionResult> BlockUser(string id, bool isBlocked)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (isBlocked != user.IsActive)
            {
                return Conflict(new { error = "User is already " + (isBlocked ? "blocked" : "unblocked") + "." });
            }

            user.IsActive = !isBlocked;
            user.UpdatedAt = DateTime.UtcNow;

            await userManager.SetLockoutEnabledAsync(user, isBlocked);
            await userManager.SetLockoutEndDateAsync(user, isBlocked ? DateTimeOffset.MaxValue : null);

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return ValidationProblem(new ValidationProblemDetails
                {
                    Errors = result.Errors
                        .GroupBy(error => error.Code)
                        .ToDictionary(
                            group => group.Key,
                            group => group.Select(error => error.Description).ToArray())
                });
            }

            return Ok();
        }

        [HttpDelete("users/unconfirmed")]
        [Authorize]
        public async Task<IActionResult> DeleteUnconfirmedUsers()
        {
            var unconfirmedUsers = await dbContext.Users
                .Where(user => !user.EmailConfirmed)
                .ToListAsync();

            var deletedCount = 0;

            foreach (var user in unconfirmedUsers)
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    deletedCount++;
                }
            }

            return Ok(new { deletedCount });
        }
    }
}
