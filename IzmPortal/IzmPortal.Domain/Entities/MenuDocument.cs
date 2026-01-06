using IzmPortal.Domain.Common;

namespace IzmPortal.Domain.Entities;

public class MenuDocument : BaseEntity
{
    public string? Title { get; private set; }
    public string FilePath { get; private set; } = null!;
    public int Order { get; private set; }
    public bool IsActive { get; private set; }

    public Guid MenuId { get; private set; }
    public Menu Menu { get; private set; } = null!;

    protected MenuDocument() { }

    public MenuDocument(
        string? title,
        string filePath,
        int order,
        Guid menuId)
    {
        Title = title;
        FilePath = filePath;
        Order = order;
        MenuId = menuId;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateTitle(string? title)
    {
        Title = title;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
