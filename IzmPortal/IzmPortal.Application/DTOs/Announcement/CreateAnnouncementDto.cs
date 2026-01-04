namespace IzmPortal.Application.DTOs.Announcement;

public class CreateAnnouncementDto
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public string? PdfUrl { get; set; }   // ✅
}
