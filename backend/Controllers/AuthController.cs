using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using backend.Models.Dtos;
using backend.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace backend.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController(IConfiguration configuration) : ControllerBase
    {
        private static readonly EmailAddressAttribute EmailAddressAttribute = new();

        [HttpPost("register", Order = -1)]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequestDto request,
            [FromServices] UserManager<User> userManager,
            [FromServices] IEmailSender<User> emailSender)
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

            await SendConfirmationEmailAsync(user, userManager, emailSender, request.Email);

            return Ok();
        }

        private async Task SendConfirmationEmailAsync(
            User user,
            UserManager<User> userManager,
            IEmailSender<User> emailSender,
            string email)
        {
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var publicUrl = configuration["App:PublicUrl"]?.TrimEnd('/');
            if (string.IsNullOrEmpty(publicUrl))
            {
                throw new InvalidOperationException("App:PublicUrl is not configured.");
            }

            var userId = await userManager.GetUserIdAsync(user);
            var confirmEmailPath = QueryHelpers.AddQueryString(
                "/api/confirmEmail",
                new Dictionary<string, string?>
                {
                    ["userId"] = userId,
                    ["code"] = code,
                });
            var confirmEmailUrl = $"{publicUrl}{confirmEmailPath}";

            await emailSender.SendConfirmationLinkAsync(
                user,
                email,
                HtmlEncoder.Default.Encode(confirmEmailUrl));
        }
    }
}
