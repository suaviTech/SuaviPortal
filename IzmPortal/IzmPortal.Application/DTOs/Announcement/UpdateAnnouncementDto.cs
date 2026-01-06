namespace IzmPortal.Application.DTOs.Announcement;

public class UpdateAnnouncementDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public bool IsActive { get; set; }
    // 🔑 Mevcut PDF yolu
    public string? ExistingPdfUrl { get; set; }
}
