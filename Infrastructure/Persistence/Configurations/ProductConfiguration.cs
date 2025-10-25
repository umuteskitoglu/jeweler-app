using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();
        builder.Property(p => p.CategoryId).IsRequired();
        builder.HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId);
        builder.OwnsOne(p => p.Name, pn =>
        {
            pn.Property(p => p.Value)
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(100);
            pn.HasIndex(p => p.Value).IsUnique();
            pn.Property(p => p.Slug)
                .HasColumnName("Slug")
                .IsRequired()
                .HasMaxLength(100);
            pn.HasIndex(p => p.Slug).IsUnique();
        });
        builder.OwnsOne(p => p.Price, m =>
        {
            m.Property(p => p.Amount)
                .HasColumnName("Price")
                .IsRequired();
            m.Property(p => p.Currency)
                .HasColumnName("Currency")
                .IsRequired()
                .HasMaxLength(3);
        });
        builder.Property(p => p.Stock).IsRequired();
        builder.OwnsOne(p => p.Created, ai =>
        {
            ai.Property(a => a.By)
                .HasColumnName("CreatedBy")
                .IsRequired()
                .HasMaxLength(100);
            ai.Property(a => a.At)
                .HasColumnName("CreatedAt")
                .IsRequired();
        });
        builder.OwnsOne(p => p.Updated, ai =>
        {
            ai.Property(a => a.By)
                .HasColumnName("UpdatedBy")
                .HasMaxLength(100);
            ai.Property(a => a.At)
                .HasColumnName("UpdatedAt");
        });
        builder.OwnsOne(p => p.Deleted, ai =>
        {
            ai.Property(a => a.By)
                .HasColumnName("DeletedBy")
                .HasMaxLength(100);
            ai.Property(a => a.At)
                .HasColumnName("DeletedAt");
        });
        builder.HasQueryFilter(p => p.Deleted == null || p.Deleted.At == null);
    }
}