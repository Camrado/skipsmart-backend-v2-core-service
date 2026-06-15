using SkipSmart.Application.Abstractions.Data;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Courses;

namespace SkipSmart.Application.Courses.DeleteCourse;

internal sealed class DeleteCourseCommandHandler : ICommandHandler<DeleteCourseCommand> {
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCourseCommandHandler(ICourseRepository courseRepository, IUnitOfWork unitOfWork) {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteCourseCommand request, CancellationToken cancellationToken) {
        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);

        if (course is null) {
            return Result.Failure(new Error("Course.NotFound", $"The course with Id {request.CourseId} was not found"));
        }

        _courseRepository.Remove(course);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
