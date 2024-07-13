using SkipSmart.Domain.Users;

namespace SkipSmart.Application.Abstractions.Authentication;

public interface IJwtService {
    string CreateToken(User user);
}