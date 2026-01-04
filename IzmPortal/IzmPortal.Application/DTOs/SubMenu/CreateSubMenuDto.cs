namespace IzmPortal.Application.DTOs.SubMenu;

public class CreateSubMenuDto
{
    public string Title { get; set; } = null!;
    public int Order { get; set; }
    public Guid MenuId { get; set; }
}

