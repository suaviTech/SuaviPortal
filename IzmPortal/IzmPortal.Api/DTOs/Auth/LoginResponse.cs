namespace IzmPortal.Api.DTOs.Auth;

public class LoginResponse
{
    public string Token { get; set; } = null!;
    public bool ForcePasswordChange { get; set; }
}
