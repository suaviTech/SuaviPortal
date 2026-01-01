using Microsoft.AspNetCore.Identity;

namespace IzmPortal.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public bool ForcePasswordChange { get; set; } = true;

    // TcNumber DB'de var ama:
    // ❌ DTO yok
    // ❌ Response yok
    // ❌ Log yok
    public string TcNumber { get; set; } = null!;
}

