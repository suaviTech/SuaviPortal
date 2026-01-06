namespace IzmPortal.Admin.Models.Users;

public class PersonalUserInfoVm
{
    public required string Card { get; set; }
    public string? Departman { get; set; }
    public string? Phone { get; set; }
    public required string TcMasked { get; set; }
}
