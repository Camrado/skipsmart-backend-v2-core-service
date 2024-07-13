namespace SkipSmart.Domain.Users;

public record Password(string HashedPassword, string PasswordSalt);