using MailKit.Net.Smtp;
using MimeKit;
using SkipSmart.Application.Abstractions.Email;
using SkipSmart.Domain.Abstractions;
using EmailErrors = SkipSmart.Domain.Shared.EmailErrors;

namespace SkipSmart.Infrastructure.Email;

internal sealed class EmailService : IEmailService {
    private readonly string _fromMail;
    private readonly string _password;
    private readonly string _host;
    private readonly int _port;
    
    public EmailService() {
        // TODO: Add fromMail and password and "smtp-mail.outlook.com" and 587 to .env file
        _fromMail = Environment.GetEnvironmentVariable("EMAIL_ADDRESS") ?? throw new InvalidOperationException("Email address not found");
        _password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD") ?? throw new InvalidOperationException("Email password not found");
        _host = Environment.GetEnvironmentVariable("EMAIL_HOST") ?? throw new InvalidOperationException("Email host not found");
        _port = int.Parse(Environment.GetEnvironmentVariable("EMAIL_PORT") ?? throw new InvalidOperationException("Email port not found"));
    }
    
    public async Task<Result> SendEmailAsync(Domain.Users.Email recipientEmail, string subject, string body) {
        var message = new MimeMessage();
        
        message.From.Add(new MailboxAddress("SkipSmart Support", _fromMail));
        message.To.Add(new MailboxAddress("New SkipSmart User", recipientEmail.Value));
        message.Subject = subject;
        message.Body = new TextPart("html") {
            Text = body
        };
        
        using var client = new SmtpClient();

        try {
            await client.ConnectAsync(_host, _port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_fromMail, _password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            return Result.Success();
        } catch {
            return Result.Failure(EmailErrors.CouldNotSendEmail);
        }
    }
}