using Domain.Entities;
using Domain.ValueObjects;
using Domain.Enums;
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
        
        // Basic properties
        builder.Property(p => p.Description).HasMaxLength(1000);
        builder.Property(p => p.SKU).IsRequired().HasMaxLength(50);
        builder.HasIndex(p => p.SKU).IsUnique();
        builder.Property(p => p.CollectionName).HasMaxLength(100);
        builder.Property(p => p.IsCustomizable).IsRequired();
        builder.Property(p => p.CertificateNumber).HasMaxLength(100);
        
        // Jewelry type and target audience
        builder.Property(p => p.JewelryType)
            .IsRequired()
            .HasConversion<int>();
        builder.Property(p => p.TargetGender)
            .IsRequired()
            .HasConversion<int>();
        
        // Category relationship
        builder.Property(p => p.CategoryId).IsRequired();
        builder.HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId);
        
        // Product Name (Value Object)
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
        
        // Price (Value Object)
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
        
        // Jewelry Material (Value Object)
        builder.OwnsOne(p => p.Material, m =>
        {
            m.Property(mt => mt.Type)
                .HasColumnName("MaterialType")
                .HasMaxLength(50);
            m.Property(mt => mt.Purity)
                .HasColumnName("MaterialPurity")
                .HasMaxLength(20);
            m.Property(mt => mt.Weight)
                .HasColumnName("MaterialWeight")
                .HasColumnType("decimal(10,3)");
            m.Property(mt => mt.WeightUnit)
                .HasColumnName("MaterialWeightUnit")
                .HasMaxLength(20);
        });
        
        // Jewelry Dimensions (Value Object)
        builder.OwnsOne(p => p.Dimensions, d =>
        {
            d.Property(dm => dm.Length)
                .HasColumnName("DimensionLength")
                .HasColumnType("decimal(10,2)");
            d.Property(dm => dm.Width)
                .HasColumnName("DimensionWidth")
                .HasColumnType("decimal(10,2)");
            d.Property(dm => dm.Height)
                .HasColumnName("DimensionHeight")
                .HasColumnType("decimal(10,2)");
            d.Property(dm => dm.RingSize)
                .HasColumnName("RingSize")
                .HasColumnType("decimal(5,2)");
            d.Property(dm => dm.Unit)
                .HasColumnName("DimensionUnit")
                .HasMaxLength(10);
        });
        
        // Gemstones collection (stored as JSON)
        builder.OwnsMany(p => p.Gemstones, g =>
        {
            g.ToJson("Gemstones");
            g.Property(gs => gs.Type).IsRequired();
        });
        
        // Type-specific specifications
        builder.OwnsOne(p => p.NecklaceSpec, n =>
        {
            n.Property(ns => ns.ChainLength)
                .HasColumnName("NecklaceChainLength")
                .HasColumnType("decimal(10,2)");
            n.Property(ns => ns.ChainType)
                .HasColumnName("NecklaceChainType")
                .HasConversion<int>();
            n.Property(ns => ns.ClaspType)
                .HasColumnName("NecklaceClaspType")
                .HasConversion<int>();
            n.Property(ns => ns.HasPendant)
                .HasColumnName("NecklaceHasPendant");
            n.Property(ns => ns.PendantDescription)
                .HasColumnName("NecklacePendantDescription")
                .HasMaxLength(500);
            n.Property(ns => ns.IsAdjustable)
                .HasColumnName("NecklaceIsAdjustable");
            n.Property(ns => ns.MinLength)
                .HasColumnName("NecklaceMinLength")
                .HasColumnType("decimal(10,2)");
            n.Property(ns => ns.MaxLength)
                .HasColumnName("NecklaceMaxLength")
                .HasColumnType("decimal(10,2)");
        });
        
        builder.OwnsOne(p => p.RingSpec, r =>
        {
            r.Property(rs => rs.Size)
                .HasColumnName("RingSize")
                .HasColumnType("decimal(5,2)");
            r.Property(rs => rs.IsResizable)
                .HasColumnName("RingIsResizable");
            r.Property(rs => rs.MinSize)
                .HasColumnName("RingMinSize")
                .HasColumnType("decimal(5,2)");
            r.Property(rs => rs.MaxSize)
                .HasColumnName("RingMaxSize")
                .HasColumnType("decimal(5,2)");
            r.Property(rs => rs.Style)
                .HasColumnName("RingStyle")
                .HasMaxLength(100);
            r.Property(rs => rs.Setting)
                .HasColumnName("RingSetting")
                .HasMaxLength(100);
        });
        
        builder.OwnsOne(p => p.EarringSpec, e =>
        {
            e.Property(es => es.BackingType)
                .HasColumnName("EarringBackingType")
                .HasMaxLength(50);
            e.Property(es => es.IsPair)
                .HasColumnName("EarringIsPair");
            e.Property(es => es.DropLength)
                .HasColumnName("EarringDropLength")
                .HasColumnType("decimal(10,2)");
            e.Property(es => es.IsHypoallergenic)
                .HasColumnName("EarringIsHypoallergenic");
        });
        
        // Image URLs collection (stored as JSON)
        builder.Property(p => p.ImageUrls)
            .HasColumnName("ImageUrls")
            .HasColumnType("jsonb")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>()
            );
        
        // Audit fields
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