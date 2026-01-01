using IzmPortal.Domain.Common;
using IzmPortal.Domain.Enums;

namespace IzmPortal.Domain.Entities;

public class Announcement : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string Content { get; private set; } = null!;
    public DateTime PublishAt { get; private set; }
    public DateTime? ExpireAt { get; private set; }
    public AnnouncementStatus Status { get; private set; }

    protected Announcement() { }

    public Announcement(string title, string content, DateTime publishAt)
    {
        Title = title;
        Content = content;
        PublishAt = publishAt;
        Status = AnnouncementStatus.Draft;
    }

    public void Publish()
    {
        Status = AnnouncementStatus.Published;
    }

    public void Expire()
    {
        Status = AnnouncementStatus.Expired;
    }
}
