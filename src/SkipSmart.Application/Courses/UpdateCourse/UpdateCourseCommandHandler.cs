using SkipSmart.Application.Abstractions.Data;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Courses;
using SkipSmart.Domain.Groups;

namespace SkipSmart.Application.Courses.UpdateCourse;

internal sealed class UpdateCourseCommandHandler : ICommandHandler<UpdateCourseCommand> {
    private readonly ICourseRepository _courseRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseCommandHandler(ICourseRepository courseRepository, IGroupRepository groupRepository, IUnitOfWork unitOfWork) {
        _courseRepository = courseRepository;
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateCourseCommand request, CancellationToken cancellationToken) {
        var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);
        if (group is null) {
            return Result.Failure(GroupErrors.NotFound);
        }

        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);

        if (course is null) {
            return Result.Failure(CourseErrors.NotFound);
        }

        course.Update(request.CourseName, request.Semester, request.GroupId, request.Hours);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
