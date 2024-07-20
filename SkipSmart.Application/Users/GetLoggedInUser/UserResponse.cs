namespace SkipSmart.Application.Users.GetLoggedInUser;

public class UserResponse {
    public Guid Id { get; init; }
    
    public string Email { get; init; }
    
    public string FirstName { get; init; }

    public string LastName { get; init; }
    
    public bool IsEmailVerified { get; init; }
    
    public int LanguageSubgroup { get; init; }
    
    public int FacultySubgroup { get; init; }
    
    public string GroupName { get; init; }
    
    public Guid GroupId { get; init; }
}