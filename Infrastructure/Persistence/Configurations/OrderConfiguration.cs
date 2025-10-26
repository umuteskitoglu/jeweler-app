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

        // Shipping Address
        builder.OwnsOne(o => o.ShippingAddress, address =>
        {
            address.Property(a => a.FullName)
                .HasColumnName("ShippingFullName")
                .IsRequired()
                .HasMaxLength(200);
            address.Property(a => a.PhoneNumber)
                .HasColumnName("ShippingPhoneNumber")
                .IsRequired()
                .HasMaxLength(20);
            address.Property(a => a.AddressLine1)
                .HasColumnName("ShippingAddressLine1")
                .IsRequired()
                .HasMaxLength(500);
            address.Property(a => a.AddressLine2)
                .HasColumnName("ShippingAddressLine2")
                .HasMaxLength(500);
            address.Property(a => a.City)
                .HasColumnName("ShippingCity")
                .IsRequired()
                .HasMaxLength(100);
            address.Property(a => a.District)
                .HasColumnName("ShippingDistrict")
                .HasMaxLength(100);
            address.Property(a => a.State)
                .HasColumnName("ShippingState")
                .HasMaxLength(100);
            address.Property(a => a.PostalCode)
                .HasColumnName("ShippingPostalCode")
                .IsRequired()
                .HasMaxLength(20);
            address.Property(a => a.Country)
                .HasColumnName("ShippingCountry")
                .IsRequired()
                .HasMaxLength(100);
            address.Property(a => a.TaxId)
                .HasColumnName("ShippingTaxId")
                .HasMaxLength(50);
            address.Property(a => a.TaxOffice)
                .HasColumnName("ShippingTaxOffice")
                .HasMaxLength(200);
        });

        // Billing Address
        builder.OwnsOne(o => o.BillingAddress, address =>
        {
            address.Property(a => a.FullName)
                .HasColumnName("BillingFullName")
                .IsRequired()
                .HasMaxLength(200);
            address.Property(a => a.PhoneNumber)
                .HasColumnName("BillingPhoneNumber")
                .IsRequired()
                .HasMaxLength(20);
            address.Property(a => a.AddressLine1)
                .HasColumnName("BillingAddressLine1")
                .IsRequired()
                .HasMaxLength(500);
            address.Property(a => a.AddressLine2)
                .HasColumnName("BillingAddressLine2")
                .HasMaxLength(500);
            address.Property(a => a.City)
                .HasColumnName("BillingCity")
                .IsRequired()
                .HasMaxLength(100);
            address.Property(a => a.District)
                .HasColumnName("BillingDistrict")
                .HasMaxLength(100);
            address.Property(a => a.State)
                .HasColumnName("BillingState")
                .HasMaxLength(100);
            address.Property(a => a.PostalCode)
                .HasColumnName("BillingPostalCode")
                .IsRequired()
                .HasMaxLength(20);
            address.Property(a => a.Country)
                .HasColumnName("BillingCountry")
                .IsRequired()
                .HasMaxLength(100);
            address.Property(a => a.TaxId)
                .HasColumnName("BillingTaxId")
                .HasMaxLength(50);
            address.Property(a => a.TaxOffice)
                .HasColumnName("BillingTaxOffice")
                .HasMaxLength(200);
        });

        // Additional order information
        builder.Property(o => o.CustomerNote)
            .HasMaxLength(1000);
        builder.Property(o => o.TrackingNumber)
            .HasMaxLength(100);
        builder.Property(o => o.ShippedAt);
        builder.Property(o => o.DeliveredAt);

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

