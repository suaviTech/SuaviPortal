using System;

namespace IzmPortal.Domain.Entities;

public class AdminAuditLog
{
    public Guid Id { get; set; }

    // Kim yaptı
    public string? UserId { get; set; }
    public string UserName { get; set; } = default!;

    // Ne yaptı
    public string Action { get; set; } = default!;
    public string EntityName { get; set; } = default!;
    public string? EntityId { get; set; }

    // Değişiklik detayları (ADIM 8.2 için hazır)
    public string? BeforeData { get; set; }
    public string? AfterData { get; set; }

    // Nereden / Ne zaman
    public string? IpAddress { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
