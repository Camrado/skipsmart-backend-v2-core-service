using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Domain.Shared;

public static class EmailErrors {
    public static Error VerificationEmailWasNotSent = new(
        "Email.VerificationEmailWasNotSent",
        "The verification email was not sent");
    
    public static Error EmailIsAlreadyVerified = new(
        "Email.EmailIsAlreadyVerified",
        "The email is already verified");
}