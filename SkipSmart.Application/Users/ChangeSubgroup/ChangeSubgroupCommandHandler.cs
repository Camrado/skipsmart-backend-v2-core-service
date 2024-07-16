using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Application.Users.ChangeGroup;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Attendances;
using SkipSmart.Domain.Groups;
using SkipSmart.Domain.Users;

namespace SkipSmart.Application.Users.ChangeSubgroup;

internal sealed class ChangeSubgroupCommandHandler : ICommandHandler<ChangeSubgroupCommand, Result> {
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;
    
    public ChangeSubgroupCommandHandler(
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
    
    public async Task<Result<Result>> Handle(ChangeSubgroupCommand request, CancellationToken cancellationToken) {
        var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken);
        
        user.ChangeSubgroup(request.NewSubgroup);
        _attendanceRepository.DeleteByUserId(user.Id, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}