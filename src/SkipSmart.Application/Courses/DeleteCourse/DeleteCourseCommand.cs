using SkipSmart.Application.Abstractions.Messaging;

namespace SkipSmart.Application.Courses.DeleteCourse;

public record DeleteCourseCommand(Guid CourseId) : ICommand;
