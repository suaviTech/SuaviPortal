namespace IzmPortal.Admin.ViewModels.Users;

public class UserListVm
{
    public string Id { get; set; } = "";
    public string Email { get; set; } = "";
    public List<string> Roles { get; set; } = new();
    public bool IsActive { get; set; }
}

