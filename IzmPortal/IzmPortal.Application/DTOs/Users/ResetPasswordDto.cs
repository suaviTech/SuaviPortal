namespace IzmPortal.Application.DTOs.Users;

public class ResetPasswordDto
{
    public string UserId { get; set; } = "";
    public string NewPassword { get; set; } = "";
}
