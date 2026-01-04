namespace IzmPortal.Application.DTOs.ApplicationShortcut;

public class ApplicationShortcutDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string? Icon { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
}
