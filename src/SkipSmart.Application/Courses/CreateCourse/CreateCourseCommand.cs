using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Courses;

namespace SkipSmart.Application.Courses.CreateCourse;

public record CreateCourseCommand(string CourseName, Semester Semester, Guid GroupId, decimal Hours) : ICommand<Guid>;
