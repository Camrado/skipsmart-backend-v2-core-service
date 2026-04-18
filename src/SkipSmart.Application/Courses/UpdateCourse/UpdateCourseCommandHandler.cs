using SkipSmart.Application.Abstractions.Data;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Courses;

namespace SkipSmart.Application.Courses.UpdateCourse;

internal sealed class UpdateCourseCommandHandler : ICommandHandler<UpdateCourseCommand> {
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseCommandHandler(ICourseRepository courseRepository, IUnitOfWork unitOfWork) {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateCourseCommand request, CancellationToken cancellationToken) {
        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);

        if (course is null) {
            return Result.Failure(new Error("Course.NotFound", $"The course with Id {request.CourseId} was not found"));
        }

        course.Update(request.CourseName, request.Semester, request.GroupId, request.Hours);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
