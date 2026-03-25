using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Domain.Courses;

public static class CourseErrors {
    public static Error NotFound = new(
        "Course.NotFound",
        "The course with the specified identifier was not found");
}