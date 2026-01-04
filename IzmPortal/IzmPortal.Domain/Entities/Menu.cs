using IzmPortal.Domain.Common;

namespace IzmPortal.Domain.Entities;

public class Menu : BaseEntity
{
    public string Title { get; private set; } = null!;
    public int Order { get; private set; }
    public bool IsActive { get; private set; }

    public ICollection<SubMenu> SubMenus { get; private set; } = new List<SubMenu>();

    protected Menu() { }

    public Menu(string title, int order)
    {
        Title = title;
        Order = order;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string title, int order)
    {
        Title = title;
        Order = order;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
