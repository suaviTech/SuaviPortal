namespace IzmPortal.Application.DTOs.Announcement;

public class PublicAnnouncementDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string? PdfUrl { get; set; }

}

