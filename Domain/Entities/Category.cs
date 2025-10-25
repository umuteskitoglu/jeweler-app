using System.Collections.Generic;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Category
{
    private Category()
    {
    }

    public Category(string name, string slug, string? description, AuditInfo created)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Category name cannot be empty", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentException("Category slug cannot be empty", nameof(slug));
        }

        Name = name;
        Slug = slug;
        Description = description;
        Created = created;
        Updated = new AuditInfo(created.At, created.By);
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public string? Description { get; private set; }
    public Guid? ParentCategoryId { get; private set; }
    public Category? ParentCategory { get; private set; }
    public ICollection<Category> Children { get; private set; } = new List<Category>();
    public AuditInfo Created { get; private set; } = null!;
    public AuditInfo Updated { get; private set; } = null!;
    public AuditInfo? Deleted { get; private set; }

    public bool IsDeleted => Deleted?.At.HasValue ?? false;

    public void Update(string name, string slug, string? description, AuditInfo auditInfo)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Category name cannot be empty", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentException("Category slug cannot be empty", nameof(slug));
        }

        Name = name;
        Slug = slug;
        Description = description;
        Updated = auditInfo;
    }

    public void SetParent(Category? parent, AuditInfo auditInfo)
    {
        ParentCategory = parent;
        ParentCategoryId = parent?.Id;
        Updated = auditInfo;
    }

    public void Delete(AuditInfo auditInfo)
    {
        Deleted = auditInfo;
    }

    public void Restore()
    {
        Deleted = null;
    }
}

