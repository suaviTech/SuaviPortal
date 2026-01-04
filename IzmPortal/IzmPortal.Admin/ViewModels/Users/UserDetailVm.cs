namespace IzmPortal.Admin.ViewModels.Users;

public class UserDetailVm
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public bool IsActive { get; set; }
    public bool ForcePasswordChange { get; set; }
    public required List<string> Roles { get; set; }
}
