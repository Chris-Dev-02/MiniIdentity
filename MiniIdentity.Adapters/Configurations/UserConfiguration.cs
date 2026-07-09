using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniIdentity.Enterprise.Models;

namespace MiniIdentity.Adapters.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasMaxLength(50);

            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.NormalizedUsername)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.NormalizedEmail)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Salt)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Created)
                .IsRequired();

            builder.HasIndex(x => x.NormalizedUsername)
                .IsUnique();

            builder.HasIndex(x => x.NormalizedEmail)
                .IsUnique();
        }
    }
}
