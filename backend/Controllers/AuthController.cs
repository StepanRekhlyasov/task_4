using System.ComponentModel.DataAnnotations;
using backend.Models.Dtos;
using backend.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private static readonly EmailAddressAttribute EmailAddressAttribute = new();

        [HttpPost("register", Order = -1)]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequestDto request,
            [FromServices] UserManager<User> userManager)
        {
            if (!EmailAddressAttribute.IsValid(request.Email))
            {
                return BadRequest(new { error = "Invalid email address" });
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                return BadRequest(new { error = "Name is required" });
            }

            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                Name = request.Name.Trim()
            };

            var result = await userManager.CreateAsync(user, request.Password);

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
    }
}
