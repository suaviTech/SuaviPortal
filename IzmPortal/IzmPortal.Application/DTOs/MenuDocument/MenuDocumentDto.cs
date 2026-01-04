namespace IzmPortal.Application.DTOs.MenuDocument;

public class MenuDocumentDto
{
    public Guid Id { get; set; }
    public Guid SubMenuId { get; set; }

    public string Title { get; set; } = null!;
    public string FilePath { get; set; } = null!;

    public int Order { get; set; }
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}








