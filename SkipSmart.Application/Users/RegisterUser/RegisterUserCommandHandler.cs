using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Application.Exceptions;
using SkipSmart.Application.Users.Shared;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Shared;
using SkipSmart.Domain.Users;

namespace SkipSmart.Application.Users.RegisterUser;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, AccessTokenResponse> {
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    // private readonly IEmailVerificationService _emailVerificationService;
    private readonly IJwtService _jwtService;
    private readonly PasswordHasherService _passwordHasherService;
    private readonly string _pepper;
    private readonly int _iterations;
    
    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        // IEmailVerificationService emailVerificationService,
        IJwtService jwtService,
        PasswordHasherService passwordHasherService) {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        // _emailVerificationService = emailVerificationService;
        _jwtService = jwtService;
        _passwordHasherService = passwordHasherService;
        _pepper = Environment.GetEnvironmentVariable("PASSWORD_HASHER_PEPPER")!;
        _iterations = Convert.ToInt32(Environment.GetEnvironmentVariable("PASSWORD_HASHER_ITERATIONS")!);
    }
    
    public async Task<Result<AccessTokenResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken) {
        try {
            var userSalt = _passwordHasherService.GenerateSalt();
            var hashedPassword = _passwordHasherService.ComputeHash(request.Password, userSalt, _pepper, _iterations);
            
            var user = User.Create(
                request.UserId ?? Guid.NewGuid(),
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

            // var emailResult = await _emailVerificationService.SendVerificationEmailAsync(user, cancellationToken);
            //
            // if (emailResult.IsFailure) {
            //     return Result.Failure<AccessTokenResponse>(EmailErrors.VerificationEmailWasNotSent);
            // }

            var accessTokenResult = _jwtService.CreateToken(user);

            return accessTokenResult.IsSuccess
                ? accessTokenResult.Value
                : Result.Failure<AccessTokenResponse>(UserErrors.JwtTokenWasNotCreated);
        } catch (DuplicateEmailException) {
            return Result.Failure<AccessTokenResponse>(UserErrors.EmailIsAlreadyTaken);
        }
    }
}