namespace IzmPortal.Application.DTOs.Menu;

public class UpdateSubMenuDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public int Order { get; set; }
}

