using IzmPortal.Domain.Common;

public class Menu : BaseEntity
{
    public string Title { get; private set; } = null!;
    public int Order { get; private set; }
    public bool IsActive { get; private set; }

    public Guid? ParentId { get; private set; }
    public Menu? Parent { get; private set; }
    public ICollection<Menu> Children { get; private set; } = new List<Menu>();

    protected Menu() { }

    public Menu(string title, int order, Guid? parentId = null)
    {
        SetTitle(title);
        SetOrder(order);

        ParentId = parentId;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string title, int order)
    {
        SetTitle(title);
        SetOrder(order);
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetParent(Guid? parentId)
    {
        ParentId = parentId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    private void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Menü başlığı boş olamaz.");

        Title = title.Trim();
    }

    private void SetOrder(int order)
    {
        if (order <= 0)
            throw new ArgumentException("Menü sırası 0'dan büyük olmalıdır.");

        Order = order;
    }
}
