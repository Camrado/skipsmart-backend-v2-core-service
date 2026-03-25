namespace SkipSmart.Application.Abstractions.Authentication;

public interface IUserContext {
    Guid UserId { get; }
    
    Guid GroupId { get; }
    
    string Email { get; }
    
    bool IsEmailVerified { get; }
}