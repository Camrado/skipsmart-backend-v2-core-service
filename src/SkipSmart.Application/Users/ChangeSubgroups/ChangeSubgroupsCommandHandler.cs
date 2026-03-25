using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Application.Users.ChangeGroup;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Attendances;
using SkipSmart.Domain.Groups;
using SkipSmart.Domain.MarkedDates;
using SkipSmart.Domain.Users;

namespace SkipSmart.Application.Users.ChangeSubgroups;

internal sealed class ChangeSubgroupsCommandHandler : ICommandHandler<ChangeSubgroupsCommand, Result> {
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMarkedDateRepository _markedDateRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;
    
    public ChangeSubgroupsCommandHandler(
        IAttendanceRepository attendanceRepository,
        IUserRepository userRepository,
        IMarkedDateRepository markedDateRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork) 
    {
        _attendanceRepository = attendanceRepository;
        _userRepository = userRepository;
        _markedDateRepository = markedDateRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Result>> Handle(ChangeSubgroupsCommand request, CancellationToken cancellationToken) {
        var user = await _userRepository.GetByIdAsync(_userContext.UserId, cancellationToken);
        
        user.ChangeLanguageSubgroup(request.NewLanguageSubgroup);
        user.ChangeFacultySubgroup(request.NewFacultySubgroup);
        _attendanceRepository.DeleteByUserId(user.Id);
        _markedDateRepository.DeleteByUserId(user.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}