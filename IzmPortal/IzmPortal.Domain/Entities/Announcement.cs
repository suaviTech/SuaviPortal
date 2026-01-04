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

    // 📄 PDF
    public string? PdfUrl { get; private set; }

    protected Announcement() { }

    // ✅ CREATE constructor (5 PARAMETRE)
    public Announcement(
        string title,
        string content,
        Guid categoryId,
        string createdBy,
        string? pdfUrl)
    {
        Title = title;
        Content = content;
        CategoryId = categoryId;
        CreatedBy = createdBy;
        PdfUrl = pdfUrl;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    // ✅ UPDATE (5 PARAMETRE)
    public void Update(
        string title,
        string content,
        Guid categoryId,
        bool isActive,
        string? pdfUrl)
    {
        Title = title;
        Content = content;
        CategoryId = categoryId;
        IsActive = isActive;
        PdfUrl = pdfUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
