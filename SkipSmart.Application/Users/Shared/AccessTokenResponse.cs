namespace SkipSmart.Application.Users.Shared;

public record AccessTokenResponse(string AccessToken, DateOnly ExpirationDate);