namespace IzmPortal.Admin.ViewModels.Users;

public class UserAdminVm
{
    public string Id { get; set; } = "";
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public bool IsActive { get; set; }
    public List<string> Roles { get; set; } = new();
}
