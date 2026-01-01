using IzmPortal.Domain.Common;

public class Announcement : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string Content { get; private set; } = null!;
    public int CategoryId { get; private set; }
    public bool IsActive { get; private set; } = true;

    protected Announcement() { }

    public Announcement(string title, string content, int categoryId)
    {
        Title = title;
        Content = content;
        CategoryId = categoryId;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
