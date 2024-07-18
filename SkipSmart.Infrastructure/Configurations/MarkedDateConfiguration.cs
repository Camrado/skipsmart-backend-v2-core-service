using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkipSmart.Domain.MarkedDates;
using SkipSmart.Domain.Users;

namespace SkipSmart.Infrastructure.Configurations;

internal sealed class MarkedDateConfiguration : IEntityTypeConfiguration<MarkedDate> {
    public void Configure(EntityTypeBuilder<MarkedDate> builder) {
        builder.ToTable("marked_dates");

        builder.HasKey(markedDate => markedDate.Id);

        builder.Property(markedDate => markedDate.RecordedDate)
            .IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(markedDate => markedDate.UserId);
    }
}