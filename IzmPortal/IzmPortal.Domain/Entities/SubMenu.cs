using IzmPortal.Domain.Common;

namespace IzmPortal.Domain.Entities;

public class SubMenu : BaseEntity
{
    public string Title { get; private set; } = null!;
    public int Order { get; private set; }
    public bool IsActive { get; private set; }

    public Guid MenuId { get; private set; }

    public ICollection<MenuDocument> Documents { get; private set; } = new List<MenuDocument>();

    protected SubMenu() { }

    public SubMenu(string title, int order, Guid menuId)
    {
        SetTitle(title);
        SetOrder(order);

        MenuId = menuId;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateTitle(string title)
    {
        SetTitle(title);
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
            throw new ArgumentException("Alt menü başlığı boş olamaz.");

        Title = title.Trim();
    }

    private void SetOrder(int order)
    {
        if (order <= 0)
            throw new ArgumentException("Alt menü sırası 0'dan büyük olmalıdır.");

        Order = order;
    }
}
