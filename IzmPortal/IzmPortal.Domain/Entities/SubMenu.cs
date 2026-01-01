using IzmPortal.Domain.Common;

namespace IzmPortal.Domain.Entities;

public class SubMenu : BaseEntity
{
    public string Title { get; private set; } = null!;
    public int Order { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Guid MenuId { get; private set; }

    // 🔥 EKLENECEK SATIR
    public ICollection<MenuDocument> Documents { get; private set; } = new List<MenuDocument>();
    protected SubMenu() { }

    public SubMenu(
        string title,
        int order,
        Guid menuId)
    {
        Title = title;
        Order = order;
        MenuId = menuId;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string title, int order)
    {
        Title = title;
        Order = order;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
