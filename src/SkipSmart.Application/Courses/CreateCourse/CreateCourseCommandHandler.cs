using SkipSmart.Application.Abstractions.Data;
using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Abstractions;
using SkipSmart.Domain.Courses;
using SkipSmart.Domain.Groups;

namespace SkipSmart.Application.Courses.CreateCourse;

internal sealed class CreateCourseCommandHandler : ICommandHandler<CreateCourseCommand, Guid> {
    private readonly ICourseRepository _courseRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseCommandHandler(ICourseRepository courseRepository, IGroupRepository groupRepository, IUnitOfWork unitOfWork) {
        _courseRepository = courseRepository;
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateCourseCommand request, CancellationToken cancellationToken) {
        var group = await _groupRepository.GetByIdAsync(request.GroupId, cancellationToken);
        if (group is null) {
            return Result.Failure<Guid>(GroupErrors.NotFound);
        }

        var course = new Course(Guid.NewGuid(), request.CourseName, request.Semester, request.GroupId, request.Hours);

        _courseRepository.Add(course);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return course.Id;
    }
}
