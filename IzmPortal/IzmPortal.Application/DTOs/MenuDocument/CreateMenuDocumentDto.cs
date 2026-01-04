namespace IzmPortal.Application.DTOs.MenuDocument;

public class CreateMenuDocumentDto
{
    public Guid SubMenuId { get; set; }
    public string Title { get; set; } = null!;
}
