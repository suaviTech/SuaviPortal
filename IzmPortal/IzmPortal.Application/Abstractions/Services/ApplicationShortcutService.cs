using IzmPortal.Application.Abstractions.Repositories;
using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.DTOs.ApplicationShortcut;
using IzmPortal.Domain.Entities;
using IzmPortal.Domain.Enums;

namespace IzmPortal.Application.Services;

public class ApplicationShortcutService : IApplicationShortcutService
{
    private readonly IApplicationShortcutRepository _repository;
    private readonly IAuditService _auditService;

    public ApplicationShortcutService(
        IApplicationShortcutRepository repository,
        IAuditService auditService)
    {
        _repository = repository;
        _auditService = auditService;
    }

    // --------------------------------------------------
    // PUBLIC
    // --------------------------------------------------

    public async Task<List<ApplicationShortcutPublicDto>> GetPublicAsync()
    {
        var items = await _repository.GetActiveAsync();

        return items.Select(x => new ApplicationShortcutPublicDto
        {
            Title = x.Title,
            Icon = x.Icon,
            Url = x.Url,
            IsExternal = x.IsExternal
        }).ToList();
    }

    // --------------------------------------------------
    // ADMIN
    // --------------------------------------------------

    public async Task<List<ApplicationShortcutAdminDto>> GetAdminAsync()
    {
        var items = await _repository.GetAllAsync();

        return items.Select(x => new ApplicationShortcutAdminDto
        {
            Id = x.Id,
            Title = x.Title,
            Icon = x.Icon,
            Url = x.Url,
            IsExternal = x.IsExternal,
            IsActive = x.IsActive,
            Order = x.Order
        }).ToList();
    }

    public async Task CreateAsync(CreateUpdateApplicationShortcutDto dto)
    {
        var maxOrder = await _repository.GetMaxOrderAsync();

        var entity = new ApplicationShortcut
        {
            Title = dto.Title,
            Icon = dto.Icon,
            Url = dto.Url,
            IsExternal = dto.IsExternal,
            IsActive = dto.IsActive,
            Order = maxOrder + 1
        };

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        await _auditService.LogAsync(
            AuditAction.Create,
            AuditEntity.ApplicationShortcut,
            entity.Id.ToString());
    }

    public async Task UpdateAsync(Guid id, CreateUpdateApplicationShortcutDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("ApplicationShortcut not found.");

        entity.Title = dto.Title;
        entity.Icon = dto.Icon;
        entity.Url = dto.Url;
        entity.IsExternal = dto.IsExternal;
        entity.IsActive = dto.IsActive;

        _repository.Update(entity);
        await _repository.SaveChangesAsync();

        await _auditService.LogAsync(
            AuditAction.Update,
            AuditEntity.ApplicationShortcut,
            id.ToString());
    }

    public async Task ActivateAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("ApplicationShortcut not found.");

        entity.IsActive = true;
        await _repository.SaveChangesAsync();

        await _auditService.LogAsync(
            AuditAction.Activate,
            AuditEntity.ApplicationShortcut,
            id.ToString());
    }

    public async Task DeactivateAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("ApplicationShortcut not found.");

        entity.IsActive = false;
        await _repository.SaveChangesAsync();

        await _auditService.LogAsync(
            AuditAction.Deactivate,
            AuditEntity.ApplicationShortcut,
            id.ToString());
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException("ApplicationShortcut not found.");

        _repository.Delete(entity);
        await _repository.SaveChangesAsync();

        await _auditService.LogAsync(
            AuditAction.Delete,
            AuditEntity.ApplicationShortcut,
            id.ToString());
    }

    public async Task<ApplicationShortcutAdminDto?> GetAdminByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            return null;

        return new ApplicationShortcutAdminDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Icon = entity.Icon,
            Url = entity.Url,
            IsExternal = entity.IsExternal,
            IsActive = entity.IsActive,
            Order = entity.Order
        };
    }
}
