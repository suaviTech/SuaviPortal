using System.ComponentModel.DataAnnotations;

namespace IzmPortal.Application.DTOs.Users;

public class AuthorizeFromPersonalDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Role { get; set; } = null!;
}

