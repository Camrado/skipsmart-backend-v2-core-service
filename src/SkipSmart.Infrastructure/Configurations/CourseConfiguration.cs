using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkipSmart.Domain.Courses;
using SkipSmart.Domain.Groups;

namespace SkipSmart.Infrastructure.Configurations;

internal sealed class CourseConfiguration : IEntityTypeConfiguration<Course> {
    public void Configure(EntityTypeBuilder<Course> builder) {
        builder.ToTable("courses");
        
        builder.HasKey(course => course.Id);

        builder.Property(course => course.CourseName)
            .HasMaxLength(300)
            .IsRequired()
            .HasConversion(courseName => courseName.Value, value => new CourseName(value));

        builder.Property(course => course.Semester)
            .IsRequired();

        builder.HasOne<Group>()
            .WithMany()
            .HasForeignKey(course => course.GroupId);
    }
}