using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkipSmart.Domain.Groups;

namespace SkipSmart.Infrastructure.Configurations;

internal sealed class GroupConfiguration : IEntityTypeConfiguration<Group> {
    public void Configure(EntityTypeBuilder<Group> builder) {
        builder.ToTable("groups");

        builder.HasKey(group => group.Id);

        builder.Property(group => group.GroupName)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(groupName => groupName.Value, value => new GroupName(value));

        builder.Property(group => group.EdupageClassId)
            .IsRequired();
    }
}