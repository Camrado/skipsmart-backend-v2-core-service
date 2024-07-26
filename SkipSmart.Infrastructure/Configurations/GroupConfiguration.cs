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

        builder.HasData(
            new Group(Guid.NewGuid(), new GroupName("A-24"), -110),
            new Group(Guid.NewGuid(), new GroupName("B-24"), -111),
            new Group(Guid.NewGuid(), new GroupName("C-24"), -112),
            new Group(Guid.NewGuid(), new GroupName("D-24"), -113),
            new Group(Guid.NewGuid(), new GroupName("E-24"), -118),
            new Group(Guid.NewGuid(), new GroupName("L1 CE-23"), -96),
            new Group(Guid.NewGuid(), new GroupName("L1 CS1-23"), -95),
            new Group(Guid.NewGuid(), new GroupName("L1 CS2-23"), -119),
            new Group(Guid.NewGuid(), new GroupName("L1 GE-23"), -94),
            new Group(Guid.NewGuid(), new GroupName("L1 PE-23"), -93),
            new Group(Guid.NewGuid(), new GroupName("L2 CE-22"), -61),
            new Group(Guid.NewGuid(), new GroupName("L2 CS-22"), -62),
            new Group(Guid.NewGuid(), new GroupName("L2 GE-22"), -63),
            new Group(Guid.NewGuid(), new GroupName("L2 PE-22"), -64),
            new Group(Guid.NewGuid(), new GroupName("L3 CE-21"), -65),
            new Group(Guid.NewGuid(), new GroupName("L3 CS-21"), -66),
            new Group(Guid.NewGuid(), new GroupName("L3 GE-21"), -67),
            new Group(Guid.NewGuid(), new GroupName("L3 PE-21"), -68)
            );
    }
}