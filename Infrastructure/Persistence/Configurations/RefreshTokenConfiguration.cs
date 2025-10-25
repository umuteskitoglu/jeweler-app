using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Id).ValueGeneratedNever();
        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(rt => rt.IpAddress)
            .HasMaxLength(100);
        builder.Property(rt => rt.UserAgent)
            .HasMaxLength(256);
        builder.Property(rt => rt.RevokedReason)
            .HasMaxLength(256);

        builder.Property(rt => rt.ExpiresAt)
            .IsRequired();

        builder.Property(rt => rt.UserId)
            .IsRequired();

        builder.OwnsOne(rt => rt.Created, ai =>
        {
            ai.Property(a => a.By)
                .HasColumnName("CreatedBy");
            ai.Property(a => a.At)
                .HasColumnName("CreatedAt");
        });

        builder.OwnsOne(rt => rt.Updated, ai =>
        {
            ai.Property(a => a.By)
                .HasColumnName("UpdatedBy");
            ai.Property(a => a.At)
                .HasColumnName("UpdatedAt");
        });

        builder.HasIndex(rt => rt.Token).IsUnique();
    }
}

