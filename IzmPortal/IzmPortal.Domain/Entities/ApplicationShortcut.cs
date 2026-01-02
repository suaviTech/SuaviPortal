using IzmPortal.Domain.Common;

namespace IzmPortal.Domain.Entities;

public class ApplicationShortcut : BaseEntity
{
    public string Title { get; set; } = null!;

    public string Icon { get; set; } = null!;
    // Örnek: "bi bi-envelope", "fa-solid fa-gear"

    public string Url { get; set; } = null!;
    // https://..., /documents, vb.

    public bool IsExternal { get; set; }

    public bool IsActive { get; set; } = true;

    public int Order { get; set; }
}
