namespace IzmPortal.Application.DTOs.Menu;

public class MenuDocumentDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string FileUrl { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}

