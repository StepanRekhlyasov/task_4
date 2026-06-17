using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api")]
    public class UsersController : ControllerBase
    {
        private readonly AuthDbContext dbContext;

        public UsersController(AuthDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

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
                    user.IsActive,
                    user.CreatedAt,
                    user.UpdatedAt,
                    user.LastActivityAt
                })
                .ToList();

            return Ok(users);
        }
    }
}
