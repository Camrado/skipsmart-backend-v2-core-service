using SkipSmart.Application.Abstractions.Messaging;
using SkipSmart.Domain.Courses;

namespace SkipSmart.Application.Courses.UpdateCourse;

public record UpdateCourseCommand(Guid CourseId, string CourseName, Semester Semester, Guid GroupId, decimal Hours) : ICommand;
