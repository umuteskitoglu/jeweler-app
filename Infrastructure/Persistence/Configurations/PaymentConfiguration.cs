using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).ValueGeneratedNever();
        builder.Property(p => p.OrderId).IsRequired();
        builder.Property(p => p.PaymentMethod)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(p => p.Provider)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(p => p.Status).IsRequired();
        builder.Property(p => p.TransactionId).HasMaxLength(200);
        builder.Property(p => p.ProviderResponse).HasMaxLength(2000);
        builder.Property(p => p.ErrorMessage).HasMaxLength(1000);

        builder.OwnsOne(p => p.Amount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("Amount")
                .IsRequired();
            money.Property(m => m.Currency)
                .HasColumnName("Currency")
                .IsRequired()
                .HasMaxLength(3);
        });

        builder.OwnsOne(p => p.Created, ai =>
        {
            ai.Property(a => a.By)
                .HasColumnName("CreatedBy");
            ai.Property(a => a.At)
                .HasColumnName("CreatedAt");
        });

        builder.OwnsOne(p => p.Updated, ai =>
        {
            ai.Property(a => a.By)
                .HasColumnName("UpdatedBy");
            ai.Property(a => a.At)
                .HasColumnName("UpdatedAt");
        });

        builder.HasIndex(p => p.OrderId);
        builder.HasIndex(p => p.TransactionId);
    }
}

