using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).ValueGeneratedNever();
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.OwnsOne(u => u.PasswordHash, ph =>
        {
            ph.Property(p => p.Value)
                .HasColumnName("PasswordHash")
                .IsRequired();
        });

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(u => u.Role)
            .IsRequired()
            .HasMaxLength(50);

        builder.OwnsOne(u => u.Created, ai =>
        {
            ai.Property(a => a.By)
                .HasColumnName("CreatedBy")
                .IsRequired();
            ai.Property(a => a.At)
                .HasColumnName("CreatedAt")
                .IsRequired();
        });

        builder.OwnsOne(u => u.Updated, ai =>
        {
            ai.Property(a => a.By)
                .HasColumnName("UpdatedBy");
            ai.Property(a => a.At)
                .HasColumnName("UpdatedAt");
        });

        builder.OwnsOne(u => u.Deleted, ai =>
        {
            ai.Property(a => a.By)
                .HasColumnName("DeletedBy");
            ai.Property(a => a.At)
                .HasColumnName("DeletedAt");
        });

        builder.HasMany(u => u.RefreshTokens)
            .WithOne()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(u => u.Deleted == null || !u.Deleted.At.HasValue);
    }
}

