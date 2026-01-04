using IzmPortal.Domain.Common;

public class ApplicationShortcut : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string Url { get; private set; } = null!;
    public string Icon { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public int Order { get; private set; }

    protected ApplicationShortcut() { }

    public ApplicationShortcut(
        string title,
        string url,
        string icon,
        bool isActive,
        int order)
    {
        Title = title;
        Url = url;
        Icon = icon;
        IsActive = isActive;
        Order = order;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(
        string title,
        string url,
        string icon,
        int order)
    {
        Title = title;
        Url = url;
        Icon = icon;
        Order = order;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
