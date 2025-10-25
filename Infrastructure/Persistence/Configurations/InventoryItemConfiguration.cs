using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.ToTable("InventoryItems");
        builder.HasKey(ii => ii.Id);

        builder.Property(ii => ii.Id).ValueGeneratedNever();
        builder.Property(ii => ii.Quantity)
            .IsRequired();
        builder.Property(ii => ii.ReservedQuantity)
            .IsRequired();

        builder.HasOne(ii => ii.Product)
            .WithOne()
            .HasForeignKey<InventoryItem>(ii => ii.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(ii => ii.Created, ai =>
        {
            ai.Property(a => a.By)
                .HasColumnName("CreatedBy");
            ai.Property(a => a.At)
                .HasColumnName("CreatedAt");
        });

        builder.OwnsOne(ii => ii.Updated, ai =>
        {
            ai.Property(a => a.By)
                .HasColumnName("UpdatedBy");
            ai.Property(a => a.At)
                .HasColumnName("UpdatedAt");
        });

        builder.OwnsOne(ii => ii.Deleted, ai =>
        {
            ai.Property(a => a.By)
                .HasColumnName("DeletedBy");
            ai.Property(a => a.At)
                .HasColumnName("DeletedAt");
        });

        builder.HasQueryFilter(ii => ii.Deleted == null || ii.Deleted.At == null);
    }
}

