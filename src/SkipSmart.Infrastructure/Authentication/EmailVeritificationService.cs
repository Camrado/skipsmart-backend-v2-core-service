using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Clock;
using SkipSmart.Application.Abstractions.Email;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Users;

namespace SkipSmart.Infrastructure.Authentication;

internal sealed class EmailVerificationService : IEmailVerificationService {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public EmailVerificationService(IUnitOfWork unitOfWork, IEmailService emailService, IDateTimeProvider dateTimeProvider) {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async Task<Result> SendVerificationEmailAsync(User user, CancellationToken cancellationToken = default) {
        int verificationCode = new Random().Next(100_000, 1_000_000);
        DateTime sentAt = _dateTimeProvider.UtcNow;
        
        user.SetEmailVerificationCode(verificationCode, sentAt);

        string message = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Verify Your Email</title>
    <style>
        /* CSS reset for email */
        body, table, td, a {{
            -webkit-text-size-adjust: 100%;
            -ms-text-size-adjust: 100%;
        }}
        table, td {{
            mso-table-lspace: 0pt;
            mso-table-rspace: 0pt;
        }}
        img {{
            -ms-interpolation-mode: bicubic;
        }}
        body {{
            font-family: Arial, sans-serif;
            background-color: #f7f7f7;
            margin: 0;
            padding: 0;
            width: 100%;
            height: 100%;
            -webkit-text-size-adjust: none;
            -ms-text-size-adjust: none;
        }}
        .container {{
            width: 100%;
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            padding: 0 10px;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }}
        .header {{
            text-align: center;
            padding: 10px 0;
        }}
        .header img {{
            max-width: 150px !important;
        }}
        .content {{
            text-align: center;
        }}
        .content h1 {{
            color: #333333;
        }}
        .content p {{
            color: #666666;
            line-height: 1.5;
        }}
        .verification-code {{
            display: inline-block;
            padding: 10px 20px;
            margin: 20px 0;
            font-size: 24px;
            font-weight: bold;
            color: #ffffff;
            background-color: #007BFF;
            border-radius: 5px;
            cursor: pointer !important;
        }}
        .copy-message {{
            display: none;
            color: #28a745;
            font-size: 14px;
            margin-top: 10px;
        }}
        .footer {{
            text-align: center;
            padding: 10px 0;
            color: #999999;
            font-size: 12px;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <img src=""https://i.ibb.co/GVDPQyz/logo-round.png"" alt=""SkipSmart Logo"">
        </div>
        <div class=""content"">
            <h1>Welcome to SkipSmart!</h1>
            <p>Thank you for registering with us. To complete your registration, please verify your email address by using the verification code below:</p>
            <div class=""verification-code"" onclick=""copyToClipboard(this)"">{verificationCode}</div>
            <div class=""copy-message"" id=""copyMessage"">Verification code copied to clipboard!</div>
            <p>If you did not register for this account, please ignore this email.</p>
        </div>
        <div class=""footer"">
            <p>&copy; SkipSmart 2024. All rights reserved.</p>
        </div>
    </div>
</body>
</html>
";
        
        var emailResult = await _emailService.SendEmailAsync(user.Email, "Email Verification - SkipSmart", message);

        if (emailResult.IsFailure) {
            return Result.Failure(UserErrors.CouldNotSendVerificationEmail);
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}