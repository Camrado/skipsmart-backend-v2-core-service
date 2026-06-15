using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkipSmart.Domain.Attendances;
using SkipSmart.Domain.Courses;
using SkipSmart.Domain.Users;

namespace SkipSmart.Infrastructure.Configurations;

internal sealed class AttendanceConfiguration : IEntityTypeConfiguration<Attendance> {
    public void Configure(EntityTypeBuilder<Attendance> builder) {
        builder.ToTable("attendances");
        
        builder.HasKey(attendance => attendance.Id);

        builder.Property(attendance => attendance.AttendanceDate)
            .IsRequired();

        builder.Property(attendance => attendance.Period)
            .IsRequired();

        builder.Property(attendance => attendance.HasAttended)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(attendance => attendance.UserId);

        builder.HasOne<Course>()
            .WithMany()
            .HasForeignKey(attendance => attendance.CourseId);
    }
}