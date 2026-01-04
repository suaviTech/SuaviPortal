using IzmPortal.Domain.Common;

namespace IzmPortal.Domain.Entities;

public class MenuDocument : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string FilePath { get; private set; } = null!;
    public int Order { get; private set; }
    public bool IsActive { get; private set; }

    public Guid SubMenuId { get; private set; }

    protected MenuDocument() { }

    public MenuDocument(
        string title,
        string filePath,
        int order,
        Guid subMenuId)
    {
        SetTitle(title);
        SetFilePath(filePath);
        SetOrder(order);

        SubMenuId = subMenuId;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateTitle(string title)
    {
        SetTitle(title);
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeFile(string filePath)
    {
        SetFilePath(filePath);
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeOrder(int order)
    {
        SetOrder(order);
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    private void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Doküman başlığı boş olamaz.");

        Title = title.Trim();
    }

    private void SetFilePath(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Dosya yolu boş olamaz.");

        FilePath = filePath.Trim();
    }

    private void SetOrder(int order)
    {
        if (order <= 0)
            throw new ArgumentException("Doküman sırası 0'dan büyük olmalıdır.");

        Order = order;
    }
}
