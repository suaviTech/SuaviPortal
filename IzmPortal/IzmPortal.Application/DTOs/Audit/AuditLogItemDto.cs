using System;
using System.Collections.Generic;
using System.Text;

namespace IzmPortal.Application.DTOs.Audit;

public class AuditLogItemDto
{
    public string UserName { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string EntityName { get; set; } = null!;
    public string? EntityId { get; set; }
    public string? IpAddress { get; set; }
    public DateTime CreatedAt { get; set; }
}
