namespace IzmPortal.Admin.ViewModels;

public class AuditLogVm
{
    public string UserName { get; init; } = "";
    public string Action { get; init; } = "";
    public string EntityName { get; init; } = "";
    public string? EntityId { get; init; }
    public string IpAddress { get; init; } = "";
    public DateTime CreatedAt { get; init; }
}
