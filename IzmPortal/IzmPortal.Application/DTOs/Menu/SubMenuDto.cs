namespace IzmPortal.Application.DTOs.Menu;

public class SubMenuDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public int Order { get; set; }
    public bool IsActive { get; set; }
}
