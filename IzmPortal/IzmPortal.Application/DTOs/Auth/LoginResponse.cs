namespace IzmPortal.Application.DTOs.Auth;

public class LoginResponse
{
    public string AccessToken { get; set; } = null!;
    public bool ForcePasswordChange { get; set; }
}
