namespace IzmPortal.Application.DTOs.ApplicationShortcut;

public class CreateApplicationShortcutDto
{
    public string Title { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public int Order { get; set; }
}


