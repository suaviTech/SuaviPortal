namespace IzmPortal.Application.DTOs.Users;

public class UserLookupResultDto
{
    public bool ExistsInIdentity { get; set; }
    public bool ExistsInPersonal { get; set; }

    public IdentityUserInfoDto? Identity { get; set; }
    public PersonalUserInfoDto? Personal { get; set; }
}
