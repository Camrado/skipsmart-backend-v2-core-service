using SkipSmart.Application.Users.Shared;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Users;

namespace SkipSmart.Application.Abstractions.Authentication;

public interface IJwtService {
    Result<AccessTokenResponse> CreateToken(User user);
}