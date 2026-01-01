namespace IzmPortal.Application.DTOs.Menu;

public class CreateSubMenuDto
{
    public Guid MenuId { get; set; }
    public string Title { get; set; } = null!;
    public int Order { get; set; }
}

