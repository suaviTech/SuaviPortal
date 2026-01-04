namespace IzmPortal.Admin.ViewModels.MenuTree;

public class MenuTreeVm
{
    public Guid MenuId { get; set; }
    public string MenuTitle { get; set; } = null!;

    public List<SubMenuNodeVm> SubMenus { get; set; } = new();
}

public class SubMenuNodeVm
{
    public Guid SubMenuId { get; set; }
    public string Title { get; set; } = null!;

    public List<DocumentNodeVm> Documents { get; set; } = new();
}

public class DocumentNodeVm
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string FilePath { get; set; } = null!;
    public bool IsActive { get; set; }
}
