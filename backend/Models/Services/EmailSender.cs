using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;
using backend.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace backend.Models.Services;

public class EmailSender(IConfiguration configuration) : IEmailSender<User>
{
    private readonly IConfiguration configuration = configuration;
    private readonly string publicUrl = configuration["App:PublicUrl"]?.TrimEnd('/')
        ?? throw new InvalidOperationException("App:PublicUrl is not configured.");

    public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink) =>
        SendEmailAsync(
            email,
            "Confirm your email",
            $"{user.Name}, please confirm your account by <a href='{ToPublicUrl(confirmationLink)}'>clicking here</a>.");

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink) =>
        SendEmailAsync(
            email,
            "Reset your password",
            $"{user.Name}, please reset your password by <a href='{ToPublicUrl(resetLink)}'>clicking here</a>.");

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode) =>
        SendEmailAsync(
            email,
            "Your password reset code",
            $"{user.Name}, please reset your password by <a href='{publicUrl}/reset-password?resetCode={WebUtility.HtmlEncode(resetCode)}&email={WebUtility.HtmlEncode(email)}'>clicking here</a>");

    private string ToPublicUrl(string link)
    {
        var decodedLink = WebUtility.HtmlDecode(link);
        if (!Uri.TryCreate(decodedLink, UriKind.Absolute, out var uri))
        {
            return HtmlEncoder.Default.Encode(link);
        }

        return HtmlEncoder.Default.Encode($"{publicUrl}{uri.PathAndQuery}");
    }

    private async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var host = configuration["Smtp:Host"]
            ?? throw new InvalidOperationException("Smtp:Host is not configured.");
        var port = configuration.GetValue("Smtp:Port", 587);
        var user = configuration["Smtp:User"]
            ?? throw new InvalidOperationException("Smtp:User is not configured.");
        var password = configuration["Smtp:Password"]
            ?? throw new InvalidOperationException("Smtp:Password is not configured.");
        var from = configuration["Smtp:From"] ?? user;

        using var client = new SmtpClient(host, port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(user, password),
        };

        using var message = new MailMessage(from, email, subject, htmlMessage)
        {
            IsBodyHtml = true,
        };

        await client.SendMailAsync(message);
    }
}
