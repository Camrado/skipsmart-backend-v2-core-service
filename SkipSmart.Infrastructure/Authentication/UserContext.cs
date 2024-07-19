using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using SkipSmart.Application.Abstractions.Authentication;

namespace SkipSmart.Infrastructure.Authentication;

internal sealed class UserContext : IUserContext {
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UserContext(IHttpContextAccessor httpContextAccessor) {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId => Guid.Parse(_httpContextAccessor.HttpContext?.User
        .Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value 
                                     ?? throw new ApplicationException("User ID is unavailable"));

    public Guid GroupId => Guid.Parse(_httpContextAccessor.HttpContext?.User
                                          .Claims.FirstOrDefault(c => c.Type == "group_id")?.Value
                                      ?? throw new ApplicationException("Group ID is unavailable"));
    
    public string Email => _httpContextAccessor.HttpContext?.User
        .Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value 
                           ?? throw new ApplicationException("User email is unavailable");

    public bool IsEmailVerified => _httpContextAccessor.HttpContext?.User
        .Claims.FirstOrDefault(c => c.Type == "email_verified")?.Value == "true";
}