using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.ToTable("Favorites");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).ValueGeneratedNever();

        // User relationship
        builder.Property(f => f.UserId).IsRequired();
        builder.HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Product relationship
        builder.Property(f => f.ProductId).IsRequired();
        builder.HasOne(f => f.Product)
            .WithMany()
            .HasForeignKey(f => f.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique constraint: one user can only favorite a product once
        builder.HasIndex(f => new { f.UserId, f.ProductId })
            .IsUnique();

        // Audit info
        builder.OwnsOne(f => f.Created, ai =>
        {
            ai.Property(a => a.By)
                .HasColumnName("CreatedBy")
                .IsRequired();
            ai.Property(a => a.At)
                .HasColumnName("CreatedAt")
                .IsRequired();
        });
    }
}

