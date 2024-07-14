using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Users;

namespace SkipSmart.Application.Abstractions.Authentication;

public interface IJwtService {
    Result<string> CreateToken(User user);
}