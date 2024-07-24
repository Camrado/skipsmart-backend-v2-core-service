namespace SkipSmart.Api.Controllers.Users;

public sealed record RegisterUserRequest(
    string Email, 
    string FirstName, 
    string LastName, 
    int LanguageSubgroup, 
    int FacultySubgroup, 
    string Password, 
    Guid GroupId);