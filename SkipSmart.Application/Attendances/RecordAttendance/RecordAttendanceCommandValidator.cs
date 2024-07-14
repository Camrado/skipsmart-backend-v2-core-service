using FluentValidation;
using SkipSmart.Domain.Attendances;

namespace SkipSmart.Application.Attendances.RecordAttendance;

internal sealed class RecordAttendanceCommandValidator : AbstractValidator<RecordAttendanceCommand> {
    public RecordAttendanceCommandValidator() {
        RuleFor(c => c.AttendanceDate).NotEmpty();
        
        RuleFor(c => c.CourseId).NotEmpty();
        
        RuleFor(c => c.Period).NotEmpty();
    }
}