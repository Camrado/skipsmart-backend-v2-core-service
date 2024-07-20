using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Application.Users.Shared;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Shared;
using SkipSmart.Domain.Users;

namespace SkipSmart.Application.Users.RegisterUser;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, AccessTokenResponse> {
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailVerificationService _emailVerificationService;
    private readonly IJwtService _jwtService;
    private readonly PasswordHasherService _passwordHasherService;
    private readonly string _pepper;
    private readonly int _iterations;
    
    // TODO: Add PasswordHasherPepper and PasswordHasherIterations to the environment variables
    
    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IEmailVerificationService emailVerificationService,
        IJwtService jwtService,
        PasswordHasherService passwordHasherService) {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _emailVerificationService = emailVerificationService;
        _jwtService = jwtService;
        _passwordHasherService = passwordHasherService;
        _pepper = Environment.GetEnvironmentVariable("PasswordHasherPepper")!;
        _iterations = Convert.ToInt32(Environment.GetEnvironmentVariable("PasswordHasherIterations")!);
    }
    
    public async Task<Result<AccessTokenResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken) {
        var userSalt = _passwordHasherService.GenerateSalt();
        var hashedPassword = _passwordHasherService.ComputeHash(request.Password, userSalt, _pepper, _iterations);

        var user = User.Create(
            new FirstName(request.FirstName),
            new LastName(request.LastName),
            new Email(request.Email),
            request.LanguageSubgroup,
            request.FacultySubgroup,
            new Password(hashedPassword, userSalt),
            request.GroupId
        );

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        // TODO
        // it does:
        // 1. creating random verification code
        // 2. saving it to the users table EmailVerificationCode column
        // 3. saving DateTimeProvider.UtcNow() to the EmailVerificationSentAt column 
        // 4. sending email with the verification code
        var emailResult = await _emailVerificationService.SendVerificationEmailAsync(user.Email, cancellationToken);

        if (emailResult.IsFailure) {
            return Result.Failure<AccessTokenResponse>(EmailErrors.VerificationEmailWasNotSent);
        }
        
        var accessTokenResult = _jwtService.CreateToken(user);

        return accessTokenResult.IsSuccess
            ? new AccessTokenResponse(accessTokenResult.Value)
            : Result.Failure<AccessTokenResponse>(UserErrors.JwtTokenWasNotCreated);
    }
}