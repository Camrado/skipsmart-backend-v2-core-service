namespace SkipSmart.Infrastructure.Authentication;

public sealed class AuthenticationOptions {
    public string Issuer { get; set; } = string.Empty;
    
    public string Audience { get; set; } = string.Empty;
}