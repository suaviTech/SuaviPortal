using IzmPortal.Domain.Common;

namespace IzmPortal.Domain.Entities;

public class Menu : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string? Url { get; private set; }
    public int? ParentId { get; private set; }
    public int Order { get; private set; }
    public bool IsActive { get; private set; } = true;

    protected Menu() { }

    public Menu(string title, string? url, int order, int? parentId = null)
    {
        Title = title;
        Url = url;
        Order = order;
        ParentId = parentId;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
