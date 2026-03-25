using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Shared;
using SkipSmart.Domain.Users;

namespace SkipSmart.Application.Users.SendNewVerificationEmail;

internal sealed class SendNewVerificationEmailCommandHandler : ICommandHandler<SendNewVerificationEmailCommand, Result> {
    private readonly IEmailVerificationService _emailVerificationService;
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;
    
    public SendNewVerificationEmailCommandHandler(IEmailVerificationService emailVerificationService, IUserContext userContext, IUserRepository userRepository) {
        _emailVerificationService = emailVerificationService;
        _userContext = userContext;
        _userRepository = userRepository;
    }
    
    public async Task<Result<Result>> Handle(SendNewVerificationEmailCommand request, CancellationToken cancellationToken) {
        var isEmailVerified = _userContext.IsEmailVerified;
        
        if (isEmailVerified) {
            return Result.Failure(EmailErrors.EmailIsAlreadyVerified);
        }
        
        var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken);
        
        var emailResult = await _emailVerificationService
            .SendVerificationEmailAsync(user, cancellationToken);

        return emailResult.IsSuccess
            ? Result.Success()
            : Result.Failure(EmailErrors.VerificationEmailWasNotSent);
    }
}