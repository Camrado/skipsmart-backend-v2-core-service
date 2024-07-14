using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Attendances;
using SkipSmart.Domain.Groups;
using SkipSmart.Domain.Users;

namespace SkipSmart.Application.Users.ChangeGroup;

internal sealed class ChangeGroupCommandHandler : ICommandHandler<ChangeGroupCommand, Result> {
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;
    
    public ChangeGroupCommandHandler(
        IAttendanceRepository attendanceRepository,
        IUserRepository userRepository,
        IGroupRepository groupRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork) 
    {
        _attendanceRepository = attendanceRepository;
        _userRepository = userRepository;
        _groupRepository = groupRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Result>> Handle(ChangeGroupCommand request, CancellationToken cancellationToken) {
        var newGroup = await _groupRepository.GetByGroupNameAsync(request.NewGroupName, cancellationToken);

        if (newGroup is null) {
            return Result.Failure(GroupErrors.NotFound);
        }
        
        var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken);
        
        user.ChangeGroup(newGroup.Id);
        await _attendanceRepository.DeleteByUserId(user.Id, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}