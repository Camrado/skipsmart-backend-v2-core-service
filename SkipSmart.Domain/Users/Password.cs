namespace SkipSmart.Domain.Users;

public record Password(string PasswordHash, string PasswordSalt);