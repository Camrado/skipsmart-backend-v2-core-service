using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkipSmart.Domain.Groups;
using SkipSmart.Domain.Users;

namespace SkipSmart.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User> {
    public void Configure(EntityTypeBuilder<User> builder) {
        builder.ToTable("users");

        builder.HasKey(user => user.Id);
        
        builder.Property(user => user.FirstName)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(firstName => firstName.Value, value => new FirstName(value));

        builder.Property(user => user.LastName)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(lastName => lastName.Value, value => new LastName(value));
        
        builder.Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(150)
            .HasConversion(email => email.Value, value => new Domain.Users.Email(value));

        builder.Property(user => user.IsEmailVerified)
            .HasDefaultValue(false);

        builder.Property(user => user.EmailVerificationCode)
            .IsRequired(false)
            .HasMaxLength(6);

        builder.Property(user => user.EmailVerificationSentAt)
            .IsRequired(false);

        builder.Property(user => user.LanguageSubgroup)
            .IsRequired();
        
        builder.Property(user => user.FacultySubgroup)
            .IsRequired();

        builder.OwnsOne(user => user.Password, passwordBuilder => {
            passwordBuilder.Property(password => password.HashedPassword)
                .IsRequired();

            passwordBuilder.Property(password => password.PasswordSalt)
                .IsRequired();
        });

        builder.HasIndex(user => user.Email).IsUnique();

        builder.HasOne<Group>()
            .WithMany()
            .HasForeignKey(user => user.GroupId);
    }
}