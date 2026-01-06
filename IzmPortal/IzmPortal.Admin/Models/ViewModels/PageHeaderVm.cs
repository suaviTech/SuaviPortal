using Microsoft.AspNetCore.Html;

namespace IzmPortal.Admin.Models.ViewModels;

public class PageHeaderVm
{
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public IHtmlContent? Actions { get; set; }
}
