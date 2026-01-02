namespace IzmPortal.Admin.ViewModels;

public class AuditLogVm
{
    public string UserName { get; set; } = "";
    public string Action { get; set; } = "";
    public string EntityName { get; set; } = "";
    public string? EntityId { get; set; }
    public string IpAddress { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
