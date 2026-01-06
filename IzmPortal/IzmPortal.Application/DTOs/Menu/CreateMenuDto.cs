namespace IzmPortal.Application.DTOs.Menu;

public class CreateMenuDto
{
    public string Title { get; set; } = null!;

    public int Order { get; set; }

    /// <summary>
    /// Null → Ana Menü
    /// Dolu → Alt Menü
    /// </summary>
    public Guid? ParentId { get; set; }
}


