using IzmPortal.Domain.Common;
namespace IzmPortal.Domain.Entities;
public class Announcement : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string Content { get; private set; } = null!;

    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;

    public bool IsActive { get; private set; }

 
    public string CreatedBy { get; private set; } = null!;

    protected Announcement() { } // EF Core

    public Announcement(
        string title,
        string content,
        Guid categoryId,
        string createdBy)
    {
        Title = title;
        Content = content;
        CategoryId = categoryId;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
    public void Update(
    string title,
    string content,
    Guid categoryId,
    bool isActive)
    {
        Title = title;
        Content = content;
        CategoryId = categoryId;
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }

}
