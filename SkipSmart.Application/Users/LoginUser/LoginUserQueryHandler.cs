using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Application.Users.Shared;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Users;

namespace SkipSmart.Application.Users.LoginUser;

internal sealed class LoginUserQueryHandler : IQueryHandler<LoginUserQuery, AccessTokenResponse> {
    private readonly IJwtService _jwtService;
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasherService _passwordHasherService;
    private readonly string _pepper;
    private readonly int _iterations;
    
    public LoginUserQueryHandler(IJwtService jwtService, IUserRepository userRepository, PasswordHasherService passwordHasherService) {
        _jwtService = jwtService;
        _userRepository = userRepository;
        _passwordHasherService = passwordHasherService;
        _pepper = Environment.GetEnvironmentVariable("PASSWORD_HASHER_PEPPER") ??
                  throw new ApplicationException("Password hasher pepper is missing.");
        _iterations = Convert.ToInt32(Environment.GetEnvironmentVariable("PASSWORD_HASHER_ITERATIONS") ??
                                      throw new ApplicationException("Password hasher iterations are missing."));
    }
    
    public async Task<Result<AccessTokenResponse>> Handle(LoginUserQuery request, CancellationToken cancellationToken) {
        var user = await _userRepository.GetByEmailAsync(new Email(request.Email), cancellationToken);

        if (user is null) {
            return Result.Failure<AccessTokenResponse>(UserErrors.InvalidCredentials);
        }

        var hashedPassword =
            _passwordHasherService.ComputeHash(request.Password, user.Password.PasswordSalt, _pepper, _iterations);
        
        if (hashedPassword != user.Password.HashedPassword) {
            return Result.Failure<AccessTokenResponse>(UserErrors.InvalidCredentials);
        }

        var accessToken = _jwtService.CreateToken(user);

        return new AccessTokenResponse(accessToken.Value);
    }
}