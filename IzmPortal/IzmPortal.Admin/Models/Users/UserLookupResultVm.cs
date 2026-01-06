namespace IzmPortal.Admin.Models.Users;

public class UserLookupResultVm
{
    public bool ExistsInIdentity { get; set; }
    public bool ExistsInPersonal { get; set; }

    public IdentityUserInfoVm? Identity { get; set; }
    public PersonalUserInfoVm? Personal { get; set; }
}
