namespace IzmPortal.Application.DTOs.SubMenu;

public class UpdateSubMenuDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public int Order { get; set; }
}
