namespace SkipSmart.Application.Users.GetLoggedInUser;

public class UserResponse {
    public Guid Id { get; init; }
    
    public string Email { get; init; }
    
    public string FirstName { get; init; }

    public string LastName { get; init; }
    
    public bool IsEmailVerified { get; init; }
    
    public string GroupName { get; init; }
}