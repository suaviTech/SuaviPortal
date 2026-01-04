namespace IzmPortal.Application.DTOs.MenuHierarchy;

public class MenuHierarchyDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;

    public List<SubMenuHierarchyDto> SubMenus { get; set; } = new();
}
