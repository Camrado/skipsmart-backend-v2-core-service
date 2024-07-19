using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Application.Users.Shared;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Attendances;
using SkipSmart.Domain.Groups;
using SkipSmart.Domain.Users;

namespace SkipSmart.Application.Users.ChangeGroup;

internal sealed class ChangeGroupCommandHandler : ICommandHandler<ChangeGroupCommand, AccessTokenResponse> {
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserContext _userContext;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _unitOfWork;
    
    public ChangeGroupCommandHandler(
        IAttendanceRepository attendanceRepository,
        IUserRepository userRepository,
        IGroupRepository groupRepository,
        IUserContext userContext,
        IJwtService jwtService,
        IUnitOfWork unitOfWork) 
    {
        _attendanceRepository = attendanceRepository;
        _userRepository = userRepository;
        _groupRepository = groupRepository;
        _userContext = userContext;
        _jwtService = jwtService;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<AccessTokenResponse>> Handle(ChangeGroupCommand request, CancellationToken cancellationToken) {
        var newGroup = await _groupRepository.GetByIdAsync(request.NewGroupId, cancellationToken);

        if (newGroup is null) {
            return Result.Failure<AccessTokenResponse>(GroupErrors.NotFound);
        }
        
        var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken);
        
        user.ChangeGroup(newGroup.Id);
        _attendanceRepository.DeleteByUserId(user.Id, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        var accessToken = _jwtService.CreateToken(user);
        
        return new AccessTokenResponse(accessToken.Value);
    }
}