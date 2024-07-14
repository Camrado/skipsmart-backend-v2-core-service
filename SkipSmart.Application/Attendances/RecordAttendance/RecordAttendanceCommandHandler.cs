using SkipSmart.Application.Abstractions.Authentication;
using SkipSmart.Application.Abstractions.Data;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Attendances;
using SkipSmart.Domain.MarkedDates;

namespace SkipSmart.Application.Attendances.RecordAttendance;

internal sealed class RecordAttendanceCommandHandler : ICommandHandler<RecordAttendanceCommand, Result> {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IMarkedDateRepository _markedDateRepository;
    
    public RecordAttendanceCommandHandler(
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IAttendanceRepository attendanceRepository,
        IMarkedDateRepository markedDateRepository) {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _attendanceRepository = attendanceRepository;
        _markedDateRepository = markedDateRepository;
    }
    
    public async Task<Result<Result>> Handle(RecordAttendanceCommand request, CancellationToken cancellationToken) {
        var attendance = await _attendanceRepository
            .GetByDetailsAsync(request.AttendanceDate, (Period)request.Period, _userContext.UserId, cancellationToken);

        if (attendance is not null) {
            attendance.HasAttended = request.HasAttended;
        } else {
            var newAttendance = Attendance.Create(
                request.CourseId, 
                _userContext.UserId, 
                request.HasAttended, 
                request.AttendanceDate, 
                (Period)request.Period);

            _attendanceRepository.Add(newAttendance);
        }

        if (request.IsDateMarked) {
            var markedDate = await _markedDateRepository
                .GetByDetailsAsync(request.AttendanceDate, _userContext.UserId, cancellationToken);

            if (markedDate is null) {
                var newMarkedDate = MarkedDate.Create(request.AttendanceDate, _userContext.UserId);
                _markedDateRepository.Add(newMarkedDate);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}