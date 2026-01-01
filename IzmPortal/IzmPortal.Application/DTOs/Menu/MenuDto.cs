namespace IzmPortal.Application.DTOs.Menu;

public class MenuDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public int Order { get; set; }
    public bool IsActive { get; set; }

    public List<SubMenuDto> SubMenus { get; set; } = new();
}

