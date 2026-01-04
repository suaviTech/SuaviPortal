using System;
using System.Collections.Generic;
using System.Text;

namespace IzmPortal.Application.DTOs.Users;

public class PersonalUserInfoDto
{
    public string? Card { get; set; }
    public string? Departman { get; set; }
    public string? Phone { get; set; }
    public string TcMasked { get; set; } = default!;
}
