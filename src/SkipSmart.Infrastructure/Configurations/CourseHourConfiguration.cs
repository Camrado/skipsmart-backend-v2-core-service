using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkipSmart.Domain.CourseHours;
using SkipSmart.Domain.Courses;

namespace SkipSmart.Infrastructure.Configurations;

internal sealed class CourseHourConfiguration : IEntityTypeConfiguration<CourseHour> {
    public void Configure(EntityTypeBuilder<CourseHour> builder) {
        builder.ToTable("course_hours");

        builder.HasKey(courseHour => courseHour.Id);

        builder.Property(courseHour => courseHour.Hours)
            .IsRequired();

        builder.HasOne<Course>()
            .WithMany()
            .HasForeignKey(courseHour => courseHour.CourseId);
    }
}