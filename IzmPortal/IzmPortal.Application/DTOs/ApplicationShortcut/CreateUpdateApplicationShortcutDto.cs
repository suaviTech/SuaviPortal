namespace IzmPortal.Application.DTOs.ApplicationShortcut;

public class CreateUpdateApplicationShortcutDto
{
    public string Title { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public string Url { get; set; } = null!;

    public bool IsExternal { get; set; }
    public bool IsActive { get; set; } = true;
}
