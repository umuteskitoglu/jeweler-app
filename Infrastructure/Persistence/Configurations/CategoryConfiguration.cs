using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).ValueGeneratedNever();
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(c => c.Slug)
            .IsRequired()
            .HasMaxLength(200);
        builder.HasIndex(c => c.Slug).IsUnique();

        builder.Property(c => c.Description)
            .HasMaxLength(1000);

        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(c => c.Created, ai =>
        {
            ai.Property(a => a.By)
                .HasColumnName("CreatedBy");
            ai.Property(a => a.At)
                .HasColumnName("CreatedAt");
        });

        builder.OwnsOne(c => c.Updated, ai =>
        {
            ai.Property(a => a.By)
                .HasColumnName("UpdatedBy");
            ai.Property(a => a.At)
                .HasColumnName("UpdatedAt");
        });

        builder.OwnsOne(c => c.Deleted, ai =>
        {
            ai.Property(a => a.By)
                .HasColumnName("DeletedBy");
            ai.Property(a => a.At)
                .HasColumnName("DeletedAt");
        });

        builder.HasQueryFilter(c => c.Deleted == null || c.Deleted.At == null);
    }
}

