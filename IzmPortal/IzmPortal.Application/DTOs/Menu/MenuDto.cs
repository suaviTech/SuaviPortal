namespace IzmPortal.Application.DTOs.Menu;

public class MenuDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public int Order { get; set; }

    public bool IsActive { get; set; }

    /// <summary>
    /// Null ise ana menü, dolu ise alt menü
    /// </summary>
    public Guid? ParentId { get; set; }
}


