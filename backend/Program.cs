using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://+:8080");

var app = builder.Build();

var api = app.MapGroup("/api");
api.MapPost("/send-email", async (SendEmailRequest request, IConfiguration config) =>
{
    if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Text))
        return Results.BadRequest(new { error = "Укажите email и текст." });

    if (!IsValidEmail(request.Email))
        return Results.BadRequest(new { error = "Некорректный адрес email." });

    var smtp = config.GetSection("Smtp");
    var host = smtp["Host"];
    var user = smtp["User"];
    var password = smtp["Password"];
    Console.WriteLine($"smpt: {host} {user} {password}");
    if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password))
        return Results.Json(new { error = "SMTP не настроен. Заполните Smtp в appsettings." }, statusCode: 500);

    var port = int.TryParse(smtp["Port"], out var p) ? p : 587;
    var from = string.IsNullOrWhiteSpace(smtp["From"]) ? user : smtp["From"];

    try
    {
        using var client = new SmtpClient(host, port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(user, password)
        };

        using var message = new MailMessage(from!, request.Email.Trim())
        {
            Subject = "Сообщение с формы",
            Body = request.Text,
            IsBodyHtml = false
        };

        await client.SendMailAsync(message);
        return Results.Ok(new { message = "Письмо отправлено." });
    }
    catch (Exception ex)
    {
        return Results.Json(new { error = $"Ошибка отправки: {ex.Message}" }, statusCode: 500);
    }
});

api.MapGet("/users", () =>
{
    return Results.Json(new { users = new[] { "user1s", "user2", "user3" } });
});

Console.WriteLine("Сервер запущен: http://localhost:8080/");

app.Run();

static bool IsValidEmail(string email) =>
    Regex.IsMatch(email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

record SendEmailRequest(string? Email, string? Text);

