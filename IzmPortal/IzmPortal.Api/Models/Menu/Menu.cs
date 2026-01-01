using Microsoft.AspNetCore.Http;

namespace IzmPortal.Api.Models.Menu;

public class UploadMenuDocumentRequest
{
    public Guid SubMenuId { get; set; }
    public string Title { get; set; } = null!;
    public IFormFile File { get; set; } = null!;
}

