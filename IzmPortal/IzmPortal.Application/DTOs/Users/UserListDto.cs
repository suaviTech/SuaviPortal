using System;
using System.Collections.Generic;
using System.Text;

namespace IzmPortal.Application.DTOs.Users;

public class UserListDto
{
    public string Id { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool IsActive { get; set; }
    public List<string> Roles { get; set; } = new();
}