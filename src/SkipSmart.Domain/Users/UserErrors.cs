using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Domain.Users;

public static class UserErrors {
    public static Error NotFound = new(
        "User.NotFound",
        "The user with the specified identifier was not found");

    public static Error InvalidCredentials = new(
        "User.InvalidCredentials",
        "The provided credentials were invalid");
    
    public static Error InvalidEmailVerificationCode = new(
        "User.InvalidEmailVerificationCode",
        "The provided email verification code was invalid");
    
    public static Error EmailVerificationCodeExpired = new(
        "User.EmailVerificationCodeExpired",
        "The email verification code has expired");
    
    public static Error JwtTokenWasNotCreated = new(
        "User.JwtTokenWasNotCreated",
        "The JWT token was not created");
    
    public static Error CouldNotSendVerificationEmail = new(
        "User.CouldNotSendVerificationEmail",
        "The verification email could not be sent");
    
    public static Error EmailIsAlreadyTaken = new(
        "User.EmailIsAlreadyTaken",
        "The email is already taken");
}