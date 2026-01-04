using IzmPortal.Domain.Common;

namespace IzmPortal.Domain.Entities;

public class MenuDocument : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string FilePath { get; private set; } = null!;
    public bool IsActive { get; private set; } = true;

    public Guid SubMenuId { get; private set; }

    protected MenuDocument() { }

    public MenuDocument(
        string title,
        string filePath,
        Guid subMenuId)
    {
        Title = title;
        FilePath = filePath;
        SubMenuId = subMenuId;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string title)
    {
        Title = title;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
