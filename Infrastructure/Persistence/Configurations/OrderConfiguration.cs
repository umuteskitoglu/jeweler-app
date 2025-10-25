using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id).ValueGeneratedNever();
        builder.Property(o => o.CustomerId)
            .IsRequired();
        builder.Property(o => o.Currency)
            .IsRequired()
            .HasMaxLength(3);
        builder.Property(o => o.Status)
            .IsRequired();

        builder.Metadata.FindNavigation(nameof(Order.Items))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsOne(o => o.TotalAmount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("TotalAmount")
                .IsRequired();
            money.Property(m => m.Currency)
                .HasColumnName("TotalCurrency")
                .IsRequired()
                .HasMaxLength(3);
        });

        builder.OwnsOne(o => o.Created, ai =>
        {
            ai.Property(a => a.By)
                .HasColumnName("CreatedBy");
            ai.Property(a => a.At)
                .HasColumnName("CreatedAt");
        });

        builder.OwnsOne(o => o.Updated, ai =>
        {
            ai.Property(a => a.By)
                .HasColumnName("UpdatedBy");
            ai.Property(a => a.At)
                .HasColumnName("UpdatedAt");
        });

        builder.OwnsOne(o => o.Deleted, ai =>
        {
            ai.Property(a => a.By)
                .HasColumnName("DeletedBy");
            ai.Property(a => a.At)
                .HasColumnName("DeletedAt");
        });

        builder.HasQueryFilter(o => o.Deleted == null || o.Deleted.At == null);
    }
}

