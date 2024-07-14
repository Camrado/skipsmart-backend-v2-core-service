using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Clock;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Application.Users.Shared;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Shared;
using SkipSmart.Domain.Users;

namespace SkipSmart.Application.Users.VerifyEmail;

internal sealed class VerifyEmailCommandHandler : ICommandHandler<VerifyEmailCommand, AccessTokenResponse> {
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserContext _userContext;
    private readonly IJwtService _jwtService;
    
    public VerifyEmailCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IUserContext userContext,
        IJwtService jwtService) 
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _userContext = userContext;
        _jwtService = jwtService;
    }
    
    public async Task<Result<AccessTokenResponse>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken) {
        var isEmailVerified = _userContext.IsEmailVerified;
        
        if (isEmailVerified) {
            return Result.Failure<AccessTokenResponse>(EmailErrors.EmailIsAlreadyVerified);
        }
        
        var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken);

        if (user.EmailVerificationCode != new EmailVerificationCode(request.EmailVerificationCode)) {
            return Result.Failure<AccessTokenResponse>(UserErrors.InvalidEmailVerificationCode);
        }
        
        if (user.EmailVerificationSentAt.AddHours(1) < _dateTimeProvider.UtcNow) {
            return Result.Failure<AccessTokenResponse>(UserErrors.EmailVerificationCodeExpired);
        }

        user.VerifyEmail();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        var accessTokenResult = _jwtService.CreateToken(user);

        return accessTokenResult.IsSuccess
            ? new AccessTokenResponse(accessTokenResult.Value)
            : Result.Failure<AccessTokenResponse>(UserErrors.JwtTokenWasNotCreated);
    }
}