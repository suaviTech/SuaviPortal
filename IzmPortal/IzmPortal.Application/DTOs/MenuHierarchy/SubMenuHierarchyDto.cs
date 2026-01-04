namespace IzmPortal.Application.DTOs.MenuHierarchy;

public class SubMenuHierarchyDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public bool IsActive { get; set; }
}
